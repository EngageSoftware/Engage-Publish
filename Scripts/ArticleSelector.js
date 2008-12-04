function PopulateArticlesList() {
    var categoryId = parseInt($get(CategoriesDropDownListId).value, 10);
    Engage.Dnn.Publish.Services.PublishServices.GetArticlesByCategory(categoryId, GetArticlesSuccessFunction);
}

function GetArticlesSuccessFunction(articlesList) {
    var articlesDropDown = $get(ArticlesDropDownListId);

    articlesDropDown.options.length = 0;
    for (var i = 0; i < articlesList.length; ++i) {
        articlesDropDown.options[i] = new Option(articlesList[i].First, articlesList[i].Second);
    }
}
