/*globals Engage, $get, CategoriesDropDownListId, ArticlesDropDownListId */
(function () {
    'use strict';
    var clearDropDown = function (dropDownList) {
            /// <summary>
            /// Clear the items from the drop down.
            /// Based on AJAX Control Toolkit's CascadingDropDownList control's implementation.
            /// </summary>
            /// <returns />

            while (0 < dropDownList.options.length) {
                dropDownList.remove(0);
            }
        };
    
    window.Engage_Publish_ArticleSelector_PopulateArticlesList = function () {
        var categoryId = parseInt($get(CategoriesDropDownListId).value, 10);
        if (isFinite(categoryId)) {
            Engage.Dnn.Publish.Services.PublishServices.GetArticlesByCategory(categoryId, function (articlesList) {
                var articlesDropDown = $get(ArticlesDropDownListId),
                    i = 0;

                clearDropDown(articlesDropDown);
                for (; i < articlesList.length; i += 1) {
                    articlesDropDown.options[i] = new Option(articlesList[i].First, articlesList[i].Second);
                }
            });
        } else {
            clearDropDown($get(ArticlesDropDownListId));
        }
    };
}());