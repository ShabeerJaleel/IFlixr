var app = app || {};
app.common = (function () {

    /* View Models */
    var vm = (function () {

        var registerModel = {
            error: ko.observable('')
        };

        var signinModel = {
            error: ko.observable(''),
            returnUrl: ko.observable('')
        };

        var forgotPwdModel = {
            error: ko.observable(''),
            isError: ko.observable(true)
        };

        var modelDialogModel = {
            Title: ko.observable('Sign In')
        };

        var footerModel = {
            show: ko.observable(false)
        };

        return {
            registerModel: registerModel,
            signinModel: signinModel,
            forgotPwdModel: forgotPwdModel,
            modelDialogModel: modelDialogModel,
            footerModel: footerModel
        };

    })();

    /* Functions */
    var fn = (function () {

        function updateSignInView(data) {
            /*app.common.session.userModel.Authenticated(data.Authenticated);
            app.common.session.userModel.UserPicUrl(data.UserPicUrl);*/
            $('#loginModal').modal('hide');
            window.location.href = signinModel.returnUrl();
        }

        var loginModelTabChange = function (data, event) {
            app.common.vm.modelDialogModel.Title($($(event.target).attr('href')).attr('data-title'));
        };
        var hideAlert = function (data, event) {
            data.error('');
        };
        var signin = function (form) {
            if ($(form).valid()) {
                $.post(form.action, $(form).serialize(), function (result) {
                    app.common.vm.signinModel.error(result.Message);
                    if (result.Success)
                        updateSignInView(result.Data);
                });
            }
            else
                return false;
        };
        var signout = function () {
            $.post('/Account/LogOff', {}, function (data) {
                window.location.href = "/";
            });
        };
        var signup = function (form) {
            if ($(form).valid()) {
                $.post(form.action, $(form).serialize(), function (result) {
                    app.common.vm.registerModel.error(result.Message);
                    if (result.Success)
                        updateSignInView(result.Data);
                });
            }
            else
                return false;

        };
        var forgotPassword = function (form) {
            if ($(form).valid()) {
                $.post(form.action, $(form).serialize(), function (result) {
                    app.common.vm.forgotPwdModel.isError(!result.Success);
                    app.common.vm.forgotPwdModel.error(result.Message);
                });
            }
            else
                return false;
        };
        var toggleFavourite = function (data) {
            if (app.common.vm.userModel.Authenticated()) {
                $.post('/Api/ToggleFavourite/', { id: data.Id() }, function (result) {
                    if (result.Success) {
                        data.IsFavourite(result.Data.IsFavourite);
                        data.FavourCount(result.Data.Count);
                    }
                });
            }
            else {
                $('#loginModal').modal();
            }
        };

        var onloadImage = function (event, data) {
            $(data.currentTarget).fadeIn(200);
        };

        return {
            tabChange: loginModelTabChange,
            hideAlert: hideAlert,
            signin: signin,
            signout: signout,
            signup: signup,
            forgotPassword: forgotPassword,
            toggleFavourite: toggleFavourite,
            onloadImage: onloadImage
        };

    })();

    /* Plugin Helpers */
    var plugins = (function () {

        var initKO = function () {
            ko.bindingHandlers.stopBinding = {
                init: function () {
                    return { controlsDescendantBindings: true };
                }
            };
            ko.virtualElements.allowedBindings.stopBinding = true;
        };
        //KNOCKOUT
        var bindKO = function (data, $selector) {
            var viewModel = ko.mapping.fromJS(data);
            ko.applyBindings(viewModel, $selector);
            return viewModel;
        };

        //ISOTOPE
        var wireISO = function ($container, itemSelector) {
            $container = $container || $('.isotope');
            itemSelector = itemSelector || '.item';
            $container.imagesLoaded(function () {
                $container.isotope({
                    itemSelector: itemSelector,
                    layoutMode: 'fitRows',
                    animationEngine: 'jquery',
                    animationOptions: {
                        duration: 250,
                        easing: 'linear',
                        queue: false
                    }
                });
            });
        };

        //PACKERY
        var wirePackery = function ($container, itemSelector) {

            $container = $container || $('.packery');
            itemSelector = itemSelector || '.span3';

            imagesLoaded($container, function () {
                $container.packery({
                    itemSelector: itemSelector,
                    gutter: 0
                })
            });

        };

        //INFINITE SCROLL
        var wireInfiniteScroll = function ($container, options) {
            var navSelector = options.navSelector || '#next-page:last';
            var nextSelector = options.nextSelector || 'a#next-page:last';
            var itemSelector = options.itemSelector || '.infi-scroll';

            $container.infinitescroll({
                debug: true,
                dataType: 'json',
                appendCallback: false,
                navSelector: navSelector,
                nextSelector: nextSelector,
                itemSelector: itemSelector,
                behavior: 'twitter',
                loading: {
                    msgText: 'Loading...',
                    finishedMsg: 'Loaded all!'
                },
                path: function (pageNo) {
                    return options.pathCallback(pageNo);
                }
            }, function (json, opts) {
                options.ajaxCallback(json);
            });
        };

        //TYPEAHEAD
        var wireTypeAhead = function ($selector, callBack) {
            $selector = $selector || $('.typeahead');
            $selector.typeahead({
                source: callBack
            });
        };

        //MULTI SELECT
        var wireMultiSelect = function ($selector, onChange) {
            $selector = $selector || $('.multiselect');
            $(".multiselect").each(function () {
                var $self = $(this);
                $(this).multiselect({
                    buttonClass: 'btn btn-small btn-primary',
                    buttonWidth: '150px',
                    maxHeight: 200,
                    nonSelectedText: $(this).attr('data-non-selected-text'),
                    enableFiltering: $(this).is("[data-enable-filter]"),
                    onChange: onChange
                });
            });
        };

        //VALIDATION
        var wireValidate = function ($selector) {

            $selector.validate(
             {
                 highlight: function (e) {
                     $(e).next().removeClass('success').addClass('invalid');
                     $(e).removeClass('success').addClass('invalid');
                 },
                 unhighlight: function (e) {
                     $(e).next().removeClass('invalid').addClass('success').addClass('valid');
                 },
                 errorPlacement: function (error, element) {
                     return false;  //Suppress all messages
                 }
             });
        };

        return {
            bindKO: bindKO,
            initKO: initKO,
            wireISO: wireISO,
            wirePackery: wirePackery,
            wireInfiniteScroll: wireInfiniteScroll,
            wireTypeAhead: wireTypeAhead,
            wireMultiSelect: wireMultiSelect,
            wireValidate: wireValidate
        };
    })();

    var init = function (context) {
        plugins.initKO();
        vm.userModel = ko.mapping.fromJS(context.user) || {};
        vm.menuModel = ko.mapping.fromJS(context.menu) || {};
        ko.applyBindings(app.common, document.getElementById('.vm-common'));
        $('.footer').show();

        app.common.plugins.wireValidate($('#signin-form'));
        app.common.plugins.wireValidate($('#register-form'));
        app.common.plugins.wireValidate($('#forgotpassword-form'));

        $('#loginModal').on('hidden.bs.modal', function () {
            $('#loginModal a[href="#login"]').tab('show');
        });
    };

    return {
        init: init,
        fn: fn,
        plugins: plugins,
        vm: vm
    };

})();
