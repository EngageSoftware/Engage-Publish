using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Engage.Dnn.Publish.Data")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Engage.Dnn.Publish.Search")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", Scope = "member", 
        Target = "Engage.Dnn.Publish.Data.DataProvider.GetAdminKeywordSearch(System.String,System.Int32,System.Int32):System.Data.DataSet", 
        MessageId = "0#")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", Scope = "member", 
        Target =
            "Engage.Dnn.Publish.Data.DataProvider.GetAdminKeywordSearch(System.String,System.Int32,System.Int32,System.Int32):System.Data.DataSet", 
        MessageId = "0#")]
[assembly:
    SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", 
        Target =
            "Engage.Dnn.Publish.Data.SqlDataProvider.CreateDateTimeParam(System.String,System.Nullable`1<System.DateTime>):System.Data.SqlClient.SqlParameter"
        , Justification = "Private API, could be used in the future")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Scope = "member", 
        Target = "Engage.Dnn.Publish.Data.SqlDataProvider.GetItemVersionSettings(System.Int32,System.String):System.Data.IDataReader", 
        Justification = "Code Analysis doesn't see HasValue as validation")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.Admin")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.ArticleControls")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.CategoryControls")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.Controls")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.Data")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.Search")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Engage.Dnn.Publish.Security")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.Security")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Engage.Dnn.Publish.Services")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.Services")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.Util")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Util", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.Util")]
[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss", Scope = "type", 
        Target = "Engage.Dnn.Publish.EpRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplay.#upnlPublish")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#btnConfigure")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#btnConfigure_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#chkAllowTitleUpdate")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#chkLogBreadcrumb")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Overrideable", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#chkOverrideable")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#chkOverrideable")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#ddlChooseDisplayType")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#ddlChooseDisplayType_SelectedIndexChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dv", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#dvAdvanced")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dv", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#dvBasic")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#lblAllowTitleUpdate")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#lblCacheTime")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#lblChooseDisplayType")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#lblLogBreadcrumb")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Overrideable", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#lblOverrideable")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#lblOverrideable")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Overrideable", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#pnlConfigureOverrideable")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#pnlConfigureOverrideable")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sh", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#shAdvanced")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sh", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#shArticleDisplay")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sh", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#shCategoryDisplay")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sh", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#shCurrentDisplay")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemDisplayOptions.#upnlSettings")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemLink.#lblFooter")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemLink.#lblMessage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemLink.#lblPossible")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemLink.#lblPossibleA")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemLink.#lblPossibleB")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemLink.#lblTitle")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Overrideable", Scope = "member", 
        Target = "Engage.Dnn.Publish.ItemManager.#IsOverrideable")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Venexus", Scope = "member", 
        Target = "Engage.Dnn.Publish.ModuleBase.#AllowVenexusSearch")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Venexus", Scope = "member", 
        Target = "Engage.Dnn.Publish.ModuleBase.#AllowVenexusSearchForPortal(System.Int32)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss", Scope = "member", 
        Target = "Engage.Dnn.Publish.ModuleBase.#GetRssLinkUrl(System.Object,System.Int32,System.Int32,System.Int32,System.String)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Overrideable", Scope = "member", 
        Target = "Engage.Dnn.Publish.ModuleBase.#Overrideable")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.PrinterFriendly.#lblArticleText")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.PrinterFriendly.#lblArticleTitle")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.PrinterFriendly.#lnkPortalLogo")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminInstructions.#lblMessage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ams", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMain.#lblAmsSettings")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMain.#lblAmsSettings")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMain.#lblArticles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMain.#lblCategories")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMain.#lblComments")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMain.#lblDeleteItem")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#ApprovalStatusDropDownList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#ddlApprovalStatus_SelectedIndexChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#lblApprovalResults")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#lblEp")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "tb", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#tbEPAdmin")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkAllowRichTextDescriptions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkAuthorCategoryEdit")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkDefaultArticleComments")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkDefaultArticleRatings")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkDefaultEmailAFriend")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkDefaultPrinterFriendly")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkDefaultReturnToList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkEmailNotification")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkEnableComments")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkEnableDisplayNameAsHyperlink")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkEnablePaging")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkEnablePublishFriendlyUrls")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkEnableRatings")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkEnableRatings_CheckedChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkEnableSimpleGallery")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkEnableUltraMediaGallery")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Venexus", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkEnableVenexus")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkEnableVenexus")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkEnableViewTracking")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkReturnToListSession")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkShowItemId")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkUseApprovals")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#chkUseEmbeddedArticles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#ddlAuthor")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#ddlDefaultDisplay")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#ddlEmailRoles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#ddlRoles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#lblAdminEdit")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#lblArticleEditDefaults")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#lblAuthorCategoryEdit")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#lblDefaultSettings")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#lblDisplayFunctionality")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#lblFriendlyUrlsNotOn")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#lblMailNotConfigured")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#lblMessage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#lblRolesAndPermissions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#lnkUpdate")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#lnkUpdate_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Venexus", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#plEnableVenexus")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rad", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#radThumbnailSelection")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rfv", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminSettings.#rfvDefaultDisplayPage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ias", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.CommentEdit.#Ias")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.CommentEdit.#lblCommentId")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.CommentEdit.#lblEmailAddress")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.CommentEdit.#lblFirstName")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.CommentEdit.#lblLastName")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.CommentEdit.#lblUserId")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "cbo", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.CommentList.#cboCategories")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "cbo", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.CommentList.#cboWorkflow")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "cbo", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.CommentList.#cboWorkflow_SelectedIndexChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.CommentList.#lblItemType")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.CommentList.#lblMessage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.CommentList.#lblWorkflow")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.DeleteItem.#lblDeleteItem")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.DeleteItem.#lblItemCreated")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.DeleteItem.#lblItemId")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.DeleteItem.#lblItemIdValue")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.DeleteItem.#lblItemVersion")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.DeleteItem.#lblResults")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.DeleteItem.#lnkItemVersion")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.ItemCreated.#lblItemCreated")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.ItemCreated.#lblItemIdValue")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.ItemCreated.#lblItemId")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.ItemCreated.#lblItemVersion")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.ItemCreated.#lnkItemVersion")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#btnCancelComment")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#btnCancelComment_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#btnComment")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#btnConfirmationClose")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#btnConfirmationClose_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#btnSubmitComment")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#btnSubmitComment_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lblArticleText")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lblArticleTitle")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lblAuthor")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lblAuthorInfo")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lblCommentHeading")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lblEmailAddressComment")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lblFirstNameComment")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lblInstructions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lblLastNameComment")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lblLastUpdated")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lnkConfigure")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lnkNextPage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lnkPreviousPage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#lnkReturnToList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "mpe", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#mpeComment")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#pnlAuthor")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#pnlComment")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#pnlCommentConfirmation")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#pnlCommentEntry")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#pnlComments")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#pnlEmailAFriend")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#pnlPrinterFriendly")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#pnlReturnToList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rfv", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#rfvCommentText")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rfv", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#rfvEmailAddressComment")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rfv", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#rfvFirstNameComment")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rfv", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#rfvLastNameComment")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#upnlComments")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#upnlRating")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "val", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplay.#valCommentSummary")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#chkCommentDisplay")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#chkCommentSubmit")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#chkDisplayPhotoGallery")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#chkEmailAddressCollect")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#ddlArticleList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#ddlDisplayComments")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#ddlDisplayRatings")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#ddlFirstNameCollect")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#ddlLastNameCollect")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblArticleList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblCommentDisplay")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblCommentSubmit")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblDisplayComments")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblDisplayPhotoGallery")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblDisplayRatings")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblEmailAddressCollect")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblEnableComments")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblEnablePhotoGallery")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblEnableRatings")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblFirstNameCollect")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblLastNameCollect")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblLastUpdatedFormat")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblPhotoGalleryHoverThumbnailHeight")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblPhotoGalleryHoverThumbnailWidth")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblPhotoGalleryMaxCount")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblPhotoGalleryThumbnailHeight")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#lblPhotoGalleryThumbnailWidth")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#pnlCommentSettings")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#pnlPhotoGallerySettings")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#pnlRatingsSettings")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions.#upnlArticleDisplayOptions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#chkComments")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#chkEmailAFriend")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#chkForceDisplayTab")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#chkIncludeOtherArticlesFromSameList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#chkIncludeRelatedArticles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#chkIncludeRelatedArticles_CheckedChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#chkPrinterFriendly")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#chkRatings")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#chkReturnList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#chkShowAuthor")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#chkUseApprovals")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#chkUseApprovals_CheckedChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#ddlDisplayTabId")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#ddlPhotoGalleryAlbum")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblApproval")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblArticleId")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblArticleText")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblDisplayOnCurrentPage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblDisplayOptions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblEmbeddedArticle")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblForceDisplayTab")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblIncludeRelatedArticles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblNotUsingApprovals")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblParentCategory")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblPhotoGalleryAlbum")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblPreviousVersionDescription")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblPublishInstructions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblRelatedArticles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblRelatedCategories")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblVersionDescription")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#lblVersionNumber")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#rblDisplayOnCurrentPage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#rblDisplayOnCurrentPage_SelectedIndexChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sh", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#shArticleEdit")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sh", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#shPublishInstructions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "tbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#tblArticleEdit")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "tr", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#trArticleId")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#upnlApproval")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#upnlDisplayLocationOptions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#upnlForceDisplayTab")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#upnlForceDisplayTabLabel")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#upnlParentCategory")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#upnlRelatedArticles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleEdit.#upnlRelatedCategories")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "cbo", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleList.#cboCategories")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "cbo", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleList.#cboWorkflow")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleList.#lblItemType")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleList.#lblMessage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleList.#lblWorkflow")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleList.#lnkAddNewArticle")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleList.#upnlArticleList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryDisplay.#lblNoData")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryDisplayOptions.#ddlCategoryList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryDisplayOptions.#ddlChildDisplay")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryDisplayOptions.#ddlItemTypeList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryDisplayOptions.#ddlSortOption")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryDisplayOptions.#ddlViewOptions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryDisplayOptions.#lblChooseCategory")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryDisplayOptions.#lblChooseChildDisplay")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryDisplayOptions.#lblChooseDisplayType")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryDisplayOptions.#lblChooseItemType")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryDisplayOptions.#lblSortOption")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#chkForceDisplayTab")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#chkForceDisplayTab_CheckedChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#chkUseApprovals")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#chkUseApprovals_CheckedChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#ddlChildDisplayTabId")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#ddlDisplayTabId")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#lblApproval")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#lblCategoryId")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#lblChildDisplayTabId")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#lblChooseRoles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#lblDisplayOnCurrentPage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#lblFeaturedArticles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#lblForceDisplayTab")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#lblNotUsingApprovals")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#lblParentCategory")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#lblPublishInstructions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#lblSortOrder")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#rblDisplayOnCurrentPage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#rblDisplayOnCurrentPage_SelectedIndexChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sh", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#shCategoryEdit")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sh", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#shPublishInstructions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "tr", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#trCategoryId")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "tr", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#trCategoryPermissions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#upnlApproval")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#upnlCategoryPermissions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#upnlDisplayLocationOptions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#upnlFeaturedArticles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#upnlForceDisplayTab")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#upnlForceDisplayTabLabel")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryEdit.#upnlParentCategory")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryFeature.#lblNoData")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryFeature.#lnkRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryFeature.#lnkRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryFeatureOptions.#chkEnableRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryFeatureOptions.#chkEnableRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryFeatureOptions.#chkRandomize")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryFeatureOptions.#ddlCategoryList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryFeatureOptions.#ddlViewOptions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryFeatureOptions.#lblChooseCategory")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryFeatureOptions.#lblChooseDisplayType")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryFeatureOptions.#lblEnableRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryFeatureOptions.#lblEnableRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryFeatureOptions.#lblRandomize")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "cbo", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryList.#cboItemType")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "cbo", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryList.#cboWorkflow")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryList.#lblMessage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryList.#lblWorkflow")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryList.#lnkAddNewCategory")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevels.#lblNoData")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#chkHighlightCurrentItem")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#chkShowParentItem")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#ddlCategoryList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#DdlCategoryListSelectedIndexChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#imgDown")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#imgDown_Click(System.Object,System.Web.UI.ImageClickEventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#imgUp")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#imgUp_Click(System.Object,System.Web.UI.ImageClickEventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#lblChooseCategory")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#lblChooseMItems")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#lblChooseNLevels")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#lblHighlightCurrentItem")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#lblShowParentItem")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#lblSortItems")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lst", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions.#lstItems")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryPermissions.#imgAdd")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryPermissions.#imgRemove")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryPermissions.#lblAvailableRoles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryPermissions.#lblMessage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryPermissions.#lblSelectedRoles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lst", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryPermissions.#lstItems")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lst", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategoryPermissions.#lstSelectedItems")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySearch.#btnCategorySearch")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySearch.#ddlCategoryList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySearch.#lblCategoryList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySearch.#lblCategorySearch")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySearch.#lblNoResults")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySearchOptions.#chkAllowCategorySelection")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySearchOptions.#chkDescription")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySearchOptions.#ddlCategorySearchList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySearchOptions.#lblAllowCategorySelection")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySearchOptions.#lblChooseCategorySearch")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySearchOptions.#lblSearchUrl")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.AdminItemSearch.#btnItemSearch")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.AdminItemSearch.#lblAvailableArticles")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.AdminItemSearch.#lblItemSearch")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.AdminItemSearch.#lblMessage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lst", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.AdminItemSearch.#lstItems")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lst", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.AdminItemSearch.#lstSelectedItems")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.AdminItemSearch.#pnlItemSearch")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.Breadcrumb.#lblBreadcrumb")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.Breadcrumb.#lblYouAreHere")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CommentDisplay.#btnNext")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CommentDisplay.#btnNext_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CommentDisplay.#btnPrevious")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CommentDisplay.#btnPrevious_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CommentDisplay.#lblNoComments")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CommentDisplay.#upnlCommentDisplay")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplay.#lblCategory")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplay.#lblMessage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplay.#lnkRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplay.#lnkRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lst", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplay.#lstItems")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lst", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplay.#lstItems_ItemDataBound(System.Object,System.Web.UI.WebControls.RepeaterItemEventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplay.#pnlCategory")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#chkDisplayOptions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#chkEnableRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#chkEnableRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#chkRelatedItem")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#chkShowAll")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#chkShowAll_CheckedChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#chkShowParent")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#ddlCategory")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#ddlItemTypeList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#ddlCategory_SelectedIndexChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#ddlItemTypeList_SelectedIndexChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#ddlSortOption")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#lblDateFormat")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#lblEnableRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#lblEnableRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#lblLoadRelated")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#lblShowParent")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rb", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#rbSortDirection")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.CustomDisplayOptions.#lblSortOption")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.EmailAFriend.#btnCancel")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.EmailAFriend.#btnCancel_Click1(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.EmailAFriend.#btnEmailAFriend")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.EmailAFriend.#btnSend")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.EmailAFriend.#btnSend_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.EmailAFriend.#lblFrom")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.EmailAFriend.#lblMessage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.EmailAFriend.#lblMultiple")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.EmailAFriend.#lblPrivacy")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.EmailAFriend.#lblTo")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "mpe", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.EmailAFriend.#mpeEmailAFriend")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.EmailAFriend.#pnlEmailAFriend")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemApproval.#chkSubmitForApproval")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rad", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemApproval.#radApprovalStatus")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#btnChangeDescriptionEditorMode")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#btnChangeDescriptionEditorMode_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#chkDisplayAsHyperlink")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#chkSearchEngine")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#chkSearchEngine_CheckedChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#imgEndCalendarIcon")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#imgStartCalendarIcon")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#lblDescription")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#lblDisplayAsHyperlink")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#lblEndDate")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#lblMetaDescription")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#lblMetaKeywords")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#lblMetaTitle")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#lblName")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#lblPostingDates")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#lblStartDate")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#lblSearchEngine")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#lblUploadFile")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#pnlSearchEngine")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemEdit.#upnlSearchEngine")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListing.#lblMessage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListing.#lnkRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListing.#lnkRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lst", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListing.#lstItems")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lst", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListing.#lstItems_ItemDataBound(System.Object,System.Web.UI.WebControls.RepeaterItemEventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListingOptions.#chkEnableRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListingOptions.#chkEnableRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "chk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListingOptions.#chkShowParent")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListingOptions.#ddlCategory")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListingOptions.#ddlCategory_SelectedIndexChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListingOptions.#ddlDataType")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListingOptions.#ddlDataType_SelectedIndexChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListingOptions.#ddlDisplayFormat")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListingOptions.#ddlItemTypeList")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListingOptions.#lblEnableRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListingOptions.#lblEnableRss")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemListingOptions.#lblShowParent")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#btnItemSearch")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#btnStoreRelationshipDate")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#imgAdd")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#imgDown")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#imgRemove")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#imgUp")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#lblAvailableCategories")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#lblEndDate")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#lblItemSearch")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#lblMessage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#lblSelectedCategories")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#lblStartDate")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lst", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#lstItems")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lst", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#lstSelectedItems")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#pnlItemSearch")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "tr", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#trDownImage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "tr", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemRelationships.#trUpImage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ItemSearch.#lnkSearch")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.PrinterFriendly.#lnkPrinterFriendly")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.RelatedArticleLinks.#btnShowRelatedItem")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lst", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.RelatedArticleLinks.#lstItems")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lst", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.RelatedArticleLinks.#lstItems_DataBound(System.Object,System.Web.UI.WebControls.RepeaterItemEventArgs)")
]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#btnUploadThumbnail")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "btn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#btnUploadThumbnail_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ctl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#ctlMediaFile")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ddl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#ddlThumbnailLibrary")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#lblImageName")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#lblImageName2")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#lblImageUrl")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "mv", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#mvThumbnailImage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#pnlDnnThumb")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#pnlDnnThumb")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#pnlEngageThumb")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#rblThumbnailImage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#rblThumbnailImage_SelectedIndexChanged(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "upnl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#upnlThumbnailImage")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "vw", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#vwExternal")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "vw", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#vwInternal")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "vw", Scope = "member", 
        Target = "Engage.Dnn.Publish.Controls.ThumbnailSelector.#vwUpload")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Braindump", Scope = "member", 
        Target =
            "Engage.Dnn.Publish.Data.DataProvider.#UpdateVenexusBraindump(System.Int32,System.String,System.String,System.String,System.Int32,System.String)"
        )]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Venexus", Scope = "member", 
        Target =
            "Engage.Dnn.Publish.Data.DataProvider.#UpdateVenexusBraindump(System.Int32,System.String,System.String,System.String,System.Int32,System.String)"
        )]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Util.Utility.#DnnFriendlyModuleName")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Overrideable", Scope = "member", 
        Target = "Engage.Dnn.Publish.Util.Utility.#IsPageOverrideable(System.Int32,System.Int32)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Venexus", Scope = "member", 
        Target = "Engage.Dnn.Publish.Util.Utility.#PublishEnableVenexusSearch")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "member", 
        Target = "Engage.Dnn.Publish.Util.Utility.#DnnTagsFriendlyModuleName")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.Admin.Tools")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.Forum")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.Portability")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dnn", Scope = "namespace", 
        Target = "Engage.Dnn.Publish.Tags")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#lnkUpdateStatus")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#lnkSaveApprovalStatusCancel")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#lnkSaveApprovalStatus")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#lblVersionComments")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#lblCurrentVersionComments")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "lbl", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#lblApprovalComments")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#LnkUpdateStatusClick(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#lnkSaveApprovalStatus_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.AdminMenu.#lnkSaveApprovalStatusCancel_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "dg", Scope = "member", 
        Target =
            "Engage.Dnn.Publish.ArticleControls.ArticleList.#dgItems_PageIndexChanging(System.Object,System.Web.UI.WebControls.GridViewPageEventArgs)"
        )]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "dg", Scope = "member", 
        Target = "Engage.Dnn.Publish.ArticleControls.ArticleList.#dgItems_Sorting(System.Object,System.Web.UI.WebControls.GridViewSortEventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.Tools.ItemViewReport.#imgStartCalendarIcon")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "img", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.Tools.ItemViewReport.#imgEndCalendarIcon")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "dg", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.CommentList.#dgItems_PageIndexChanging(System.Object,System.Web.UI.WebControls.GridViewPageEventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "gv", Scope = "member", 
        Target =
            "Engage.Dnn.Publish.Admin.Tools.ItemViewReport.#gvReport_PageIndexChanging(System.Object,System.Web.UI.WebControls.GridViewPageEventArgs)"
        )]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "lnk", Scope = "member", 
        Target = "Engage.Dnn.Publish.Admin.Tools.ItemViewReport.#lnkGenerate_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "lb", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySort.#lbCancel_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "lb", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySort.#lbMoveToSort_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "lb", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySort.#lbSaveSort_Click(System.Object,System.EventArgs)")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "rl", Scope = "member", 
        Target =
            "Engage.Dnn.Publish.CategoryControls.CategorySort.#rlCategorySort_Reorder(System.Object,AjaxControlToolkit.ReorderListItemReorderEventArgs)"
        )]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rl", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySort.#rlCategorySort")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sh", Scope = "member", 
        Target = "Engage.Dnn.Publish.CategoryControls.CategorySort.#shPublishInstructions")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rl", Scope = "member", 
        Target =
            "Engage.Dnn.Publish.CategoryControls.CategorySort.#rlCategorySort_Reorder(System.Object,AjaxControlToolkit.ReorderListItemReorderEventArgs)"
        )]