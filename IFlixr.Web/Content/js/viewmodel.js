/* #region KO Viewmodel */
var app = app || {};
app.browseMovie =
 (function () {
     var url = '/Api/Query/Movie/?';
     //Methods

     var generateQueryUrl = function (page) {

         function getOptions($select) {
             var options = [];
             $('option:selected', $select).each(function () { options.push(this.value); });
             return options;
         }

         page = page || 1;
         var l = getOptions($('#movie-language')) || '';
         var y = getOptions($('#movie-year')) || '';
         var r = getOptions($('#movie-rating')) || '0';
         var query = url + 'query=' + $('.search-query.movie').val() +
         '&language=' + l + '&year=' + y + '&rating=' + r + '&page=' + page;
         return query;
     }

     var bindKO = function (data) {
         var viewModel = ko.mapping.fromJS(data);
         ko.applyBindings(viewModel, $('.vm-global')[0]);
         return viewModel;
     }

     var addMovies = function (json) {
         var $container = $('.isotope');
         $.each(json.Movies, function () {
             app.browseMovie.vmMovies.Movies.push(this);
             $container.isotope('appended', $('.iso').last());
         });

         $container.imagesLoaded(function () {
             $container.isotope('reLayout');
         });
     };

     var reloadMovies = function (json) {
         var $container = $('.isotope');
         app.browseMovie.vmMovies.Movies.removeAll();
         addMovies(json);
     };


     var init = function (data) {

         app.browseMovie.vmMovies = bindKO(data);

         //MULTI SELECT
         $(".multiselect").each(function () {
             var $self = $(this);
             $(this).multiselect({
                 buttonClass: 'btn btn-small btn-primary',
                 buttonWidth: '150px',
                 maxHeight: 200,
                 nonSelectedText: $(this).attr('data-non-selected-text'),
                 enableFiltering: $(this).is("[data-enable-filter]"),
                 onChange: function (element, checked) {
                     $.getJSON(generateQueryUrl(), {}, function (json) {
                         reloadMovies(json);
                         return true;
                     });
                 }
             });
         });


         //ISOTOPE
         var $container = $('.isotope');
         var startIso = function () {

             $container.imagesLoaded(function () {
                 $container.isotope({
                     // options
                     itemSelector: '.iso',
                     layoutMode: 'fitRows',
                     animationOptions: {
                         duration: 750,
                         easing: 'linear',
                         queue: false
                     }
                 });
             });
         };

         startIso();

         //INFINITE SCROLL
         $container.infinitescroll({
             debug: true,
             dataType: 'json',
             appendCallback: false,
             navSelector: '#next-page:last',
             nextSelector: 'a#next-page:last',
             itemSelector: '.isotope .iso',
             behavior: 'twitter',
             loading: {
                 msgText: 'Loading...',
                 finishedMsg: 'Loaded all!'
             },
             path: function (pageNo) {
                 return generateQueryUrl(pageNo);
             }
         }, function (json, opts) {
             addMovies(json);
         });

         //TYPEAHEAD
         $('.typeahead').typeahead({
             source: function (query, process) {
                 return $.getJSON(generateQueryUrl(1), {}
                    , function (json) {
                        reloadMovies(json); 
                        return process(json);
                    });
             }
         });

     };

     return {
         vmMovies: {},
         init: init
     };
 })();

/* #endregion */


