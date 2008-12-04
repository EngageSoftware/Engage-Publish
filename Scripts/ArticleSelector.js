function PopulateArticlesList() {
    var categoryId = parseInt($get(CategoriesDropDownListId).value, 10);
    if (isFinite(categoryId)) {
        Engage.Dnn.Publish.Services.PublishServices.GetArticlesByCategory(categoryId, GetArticlesSuccessFunction);
    }
    else {
        clearDropDown($get(ArticlesDropDownListId));
    }
}

function GetArticlesSuccessFunction(articlesList) {
    var articlesDropDown = $get(ArticlesDropDownListId);

    clearDropDown(articlesDropDown);
    for (var i = 0; i < articlesList.length; ++i) {
        articlesDropDown.options[i] = new Option(articlesList[i].First, articlesList[i].Second);
    }
}

function clearDropDown(dropDownList) {
    /// <summary>
    /// Clear the items from the drop down.
    /// Based on AJAX Control Toolkit's CascadingDropDownList control's implementation.
    /// </summary>
    /// <returns />

    while (0 < dropDownList.options.length) {
        dropDownList.remove(0);
    }
}
