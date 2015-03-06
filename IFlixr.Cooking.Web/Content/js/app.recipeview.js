/// <reference path="jquery.js" />
/// <reference path="app.common.js" />

/* #region KO Viewmodel */
var app = app || {};

app.recipeView =
 (function () {
     var url = '/Api/Query/Recipe/?';
     //Methods

     var generateQueryUrl = function (page) {

         function getOptions($select) {
             var options = [];
             $('option:selected', $select).each(function () { options.push(this.value); });
             return options;
         }

         page = page || 1;
         var query = url + 'query=' + $('.search-query.recipe').val() + '&page=' + page;
         return query;
     }

     var addRecipes = function (json) {
         var $container = $('.isotope');
         app.recipeView.vm.IsMore(json.IsMore);
         $.each(json.Recipes, function () {
             app.recipeView.vm.Recipes.push(this);
         });
         $container.isotope('insert', $('.isotope .item.new'));
         $('.isotope .item.new').removeClass('new');
     };

     var reloadRecipes = function (json) {
         var $container = $('.isotope');
         $container.isotope('remove', $('.isotope .item'));
         app.recipeView.vm.Recipes.removeAll();
         addRecipes(json);
     };

     var fn = {
         launchRecipeUrl: function (data, event) {
             $.post('/Api/RecipeViewCount/', { id: data.Id() }, function (result) {
                 if (result.Success)
                     data.ViewCount(result.Data);
             });
             window.open(data.Url());
         },
         toggleFavourite: function (data, event) {
             app.common.fn.signinModel.returnUrl('/');
             app.common.fn.fn.toggleFavourite(data);
         }
     };



     var init = function (data) {
         
         var viewModel = ko.mapping.fromJS(data);
         viewModel.fn = fn;
         viewModel.common = app.common;
         
         ko.applyBindings(viewModel, $('.vm-page')[0]);
         //app.common.plugins.wireISO();
         //         $('.isotope .item').removeClass('new');
         //         app.common.plugins.wireInfiniteScroll($('.infi-scroll'), {
         //             navSelector: '#next-page:last',
         //             nextSelector: 'a#next-page:last',
         //             itemSelector: '.infi-scroll .item',
         //             pathCallback: function (pageNo) {
         //                 return generateQueryUrl(pageNo);
         //             },
         //             ajaxCallback: function (json, opts) {
         //                 addRecipes(json);
         //             }
         //         });

         app.common.plugins.wireTypeAhead($('.typeahead'), function (query, process) {
             return $.getJSON(generateQueryUrl(1), {}
                , function (json) {
                    reloadRecipes(json);
                    var names = [];
                    $.each(json.Recipes, function (index, value) {
                        names.push(value.Title);
                    });
                    return process(names);
                });
         });
     };

     return {
         init: init
     };
 })();

/* #endregion */


