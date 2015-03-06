/// <reference path="_references.js" />


(function ($) {

    $ = jQuery;
    $.iFlixr = $.iFlixr || {};

    if ($.iFlixr.initPage) {
        return;
    }


    function ajaxGet(url, param, callback) {
        $.get(url, param, function (data) {
            callback(data);
        });
    }

    function wireFancyBox() {

        //fancybox
        $('.fancybox-media').fancybox({
            openEffect: 'none',
            closeEffect: 'none',
            openEasing: 'easeInOutQuad',
            //              autoSize: false,
            //              beforeLoad: function () {
            //              this.width = '100%';
            //              this.height = '100%';
            //              },
            beforeShow: function () {
                if (this.title) {
                    // New line
                    this.title += '<br />';

                    // Add tweet button
                    this.title += '<a href="https://twitter.com/share" class="twitter-share-button" data-count="none" data-url="' + this.href + '">Tweet</a> ';

                    // Add FaceBook like button
                    this.title += '<iframe src="//www.facebook.com/plugins/like.php?href=' + this.href + '&amp;layout=button_count&amp;show_faces=true&amp;width=500&amp;action=like&amp;font&amp;colorscheme=light&amp;height=23" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:110px; height:23px;" allowTransparency="true"></iframe>';
                }
            },
            afterShow: function () {
                // Render tweet button
                twttr.widgets.load();
            },
            helpers: {
                media: {},
                overlay: {
                    css: {
                        'background': 'rgba(58, 42, 45, 0.95)'
                    }
                },
                title: {
                    type: 'inside'
                }

            }
        });
    }

    function wireHoverBox() {

        $('.show-hover-box').each(function () {

            $(this).qtip(
		    {
		        content: {
		            text: 'Loading...',
		            ajax: {
		                url: '/Home/HoverBox',
		                type: 'GET',
		                data: { 'id': $(this).attr('data-id') }
		            }
		        },
		        position: {
		            viewport: $(window),
		            adjust: {
		                method: 'flip'
		            },
		            my: 'right center',
		            at: 'left center',
		            effect: false
		        },
		        style: {
		            classes: 'qtip-shadow qtip-rounded qtip-light',
		            tip: {
		                corner: true,
		                width: 10,
		                height: 10,
		                offset: 5
		            }
		        },
		        show: {
		            delay: 1000,
		            effect: function (offset) {
		                $(this).fadeIn(1000);
		            }
		        },
		        hide: { fixed: true, delay: 100 }
		    });


        });
    }

    function wireIsotop() {

        var $container = $('#container');

        $container.isotope({
            itemSelector: '.element',
            layoutMode: 'fitRows'
        });

        $container.infinitescroll({
            navSelector: '#page_nav',    // selector for the paged navigation 
            nextSelector: '#page_nav a',  // selector for the NEXT link (to page 2)
            itemSelector: '.element',     // selector for all items you'll retrieve
            debug: true,
            loading: {
                finishedMsg: 'No more pages to load.',
                img: 'http://i.imgur.com/qkKy8.gif',
                msgText: "<em>Loading more...</em>"
            },
            errorCallback: function () {
                alert('asa');
            }
        },           // call Isotope as a callback
            function (newElements) {
                $container.isotope('appended', $(newElements));
            });

    }

    function renderBanner(id) {

        $('.banner-carousel').carouFredSel({
            items: {
                visible: 1,
                width: 1600
            },
            auto: false,
            responsive: true,
            scroll: {
                fx: 'crossfade',
                duration: 1000
            },
            prev: {
                button: '.banner-container .banner-prev',
                key: 'left'
            },
            next: {
                button: '.banner-container .banner-next',
                key: 'right'
            },
            pagination: '.banner-container .carousel-pagination'
        }).find(".slide").hover(function () {
            $(this).find(".banner-desc").slideDown();
            $(this).find(".play-hover-button.large").fadeIn();
        }, function () {
            $(this).find(".banner-desc").slideUp();
            $(this).find(".play-hover-button.large").fadeOut();
        });
    }

    function renderDynamicContent(elem) {

        $(elem).children('div').each(function () {

            $('#' + this.id + ' ul').carouFredSel({
                circular: false,
                infinite: false,
                auto: false,
                align: "left",
                items: {
                    visible: "variable"
                },
                scroll: {
                    duration: 1000
                },
                width: "100%",
                prev: {
                    button: '#' + this.id + " .carousel-prev",
                    key: "left"
                },
                next: {
                    button: '#' + this.id + " .carousel-next",
                    key: "right"
                }
            });
        });

        //bind
//        $("body").on({
//            mouseenter: function () {
//                ($(this)).find('.play-hover-button').stop().show().animate({ opacity: 1 }, 400);
//            },
//            mouseleave: function () {
//                ($(this)).find('.play-hover-button').stop().animate({ opacity: 0 }, 400);
//            }
//        }, ".highlight-module");

        $(".show-play-button")
            .mouseenter(function () {
                ($(this)).find('.play-hover-button').stop().show().animate({ opacity: 1 }, 400);
            })
            .mouseleave(function () {
                ($(this)).find('.play-hover-button').stop().animate({ opacity: 0 }, 400);
            });
    }

    $.fn.extend($.iFlixr, {
        properties: {
            dynamicDataUrl: ''
        },
        initPage: function () {
            $("img.lazy").lazyload();
            
            renderBanner();
            
            $(".chzn-select").chosen();

            //load the dynamic contents
            $('.dynamic-section').each(function (e) {
                renderDynamicContent(this);
            })
           
            //movie page
            $('#movie-container-first-column').height($('#movie-container').height());

            //bind
            $('.search-box-input').focusin(function () {
                $('.search-box-div').addClass('focus');
            }).focusout(function () {
                $('.search-box-div').removeClass('focus');
            });


            wireFancyBox();
            wireIsotop();
            wireHoverBox();
        }
    });
    $.iFlixr.initPage();

})(jQuery);