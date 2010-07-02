//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Controls
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Web.UI.WebControls;

    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class ThumbnailSelector : ModuleBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _thumbnailUrl;

        private enum ThumbnailImageType
        {
            Upload, 
            External, 
            Internal
        }

        public string ThumbnailUrl
        {
            get
            {
                // Check which tool
                if (this.ThumbnailSelectionOption == ThumbnailDisplayOption.DotNetNuke.ToString())
                {
                    return this.ctlMediaFile.Url;
                }

                if (this.rblThumbnailImage.SelectedValue == ThumbnailImageType.Internal.ToString())
                {
                    return Path.Combine(Utility.GetThumbnailLibraryPath(this.PortalId).ToString(), this.ddlThumbnailLibrary.SelectedValue);
                }

                if (this.rblThumbnailImage.SelectedValue == ThumbnailImageType.External.ToString())
                {
                    return this.txtThumbnailUrl.Text;
                }

                return null;
            }

            [DebuggerStepThrough]
            set
            {
                // check option
                if (this.ThumbnailSelectionOption == ThumbnailDisplayOption.DotNetNuke.ToString())
                {
                    this.ctlMediaFile.Url = value;
                    this.ctlMediaFile.UrlType = "F";
                    this.ctlMediaFile.FileFilter = Utility.MediaFileTypes;
                }
                else
                {
                    this._thumbnailUrl = value;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();

            // TODO: Once we upgrade to or past 4.5.3, include this statement so that there won't be problems if someone has wrapped Publish with an update panel.
            // DotNetNuke.Framework.AJAX.RegisterPostBackControl(btnUploadThumbnail);
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // check VI for null then set information
                if (!this.Page.IsPostBack)
                {
                    if (this.ThumbnailSelectionOption == ThumbnailDisplayOption.DotNetNuke.ToString())
                    {
                        this.pnlDnnThumb.Visible = true;
                        this.pnlEngageThumb.Visible = false;
                    }
                    else
                    {
                        this.pnlEngageThumb.Visible = true;
                        this.pnlDnnThumb.Visible = false;
                        this.InitializeThumbnailControl();
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void btnUploadThumbnail_Click(object sender, EventArgs e)
        {
            if (this.IsUploadValid())
            {
                string path = Utility.GetThumbnailLibraryMapPath(this.PortalId).LocalPath;
                string filename = Path.GetFileNameWithoutExtension(this.fileThumbnail.PostedFile.FileName);
                string extension = Path.GetExtension(this.fileThumbnail.PostedFile.FileName);

                if (File.Exists(Path.Combine(path, filename + extension)))
                {
                    int i = 0;
                    while (File.Exists(Path.Combine(path, String.Format(CultureInfo.InvariantCulture, "{0}[{1}]{2}", filename, i, extension))))
                    {
                        i++;
                    }

                    filename = String.Format(CultureInfo.InvariantCulture, "{0}[{1}]", filename, i);
                }

                this.fileThumbnail.PostedFile.SaveAs(Path.Combine(path, filename + extension));

                this._thumbnailUrl = Path.Combine(Utility.GetThumbnailLibraryPath(this.PortalId).ToString(), filename + extension);

                this.InitializeThumbnailControl();
            }
            else
            {
                this.txtMessage.Visible = true;
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void rblThumbnailImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            View activeView; // = null;
            if (this.rblThumbnailImage.SelectedValue == ThumbnailImageType.Internal.ToString())
            {
                activeView = this.vwInternal;
            }
            else if (this.rblThumbnailImage.SelectedValue == ThumbnailImageType.External.ToString())
            {
                activeView = this.vwExternal;
            }
            else
            {
                // ThumbnailImageType.Upload.ToString(CultureInfo.InvariantCulture)
                activeView = this.vwUpload;
            }

            this.mvThumbnailImage.SetActiveView(activeView);
        }

        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
        }

        private void InitializeThumbnailControl()
        {
            this.rblThumbnailImage.Items.Clear();
            this.rblThumbnailImage.Items.Add(
                new ListItem(
                    Localization.GetString(ThumbnailImageType.Upload.ToString(), this.LocalResourceFile), ThumbnailImageType.Upload.ToString()));
            this.rblThumbnailImage.Items.Add(
                new ListItem(
                    Localization.GetString(ThumbnailImageType.Internal.ToString(), this.LocalResourceFile), ThumbnailImageType.Internal.ToString()));
            this.rblThumbnailImage.Items.Add(
                new ListItem(
                    Localization.GetString(ThumbnailImageType.External.ToString(), this.LocalResourceFile), ThumbnailImageType.External.ToString()));

            var files = new StringCollection();
            foreach (string file in Directory.GetFiles(Utility.GetThumbnailLibraryMapPath(this.PortalId).LocalPath))
            {
                files.Add(Path.GetFileName(file));
            }

            this.ddlThumbnailLibrary.Items.Clear();
            this.ddlThumbnailLibrary.DataSource = files;
            this.ddlThumbnailLibrary.DataBind();
            this.ddlThumbnailLibrary.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", this.LocalResourceFile), string.Empty));

            if (!Utility.HasValue(this._thumbnailUrl))
            {
                this.rblThumbnailImage.SelectedValue = ThumbnailImageType.Upload.ToString();
            }
                
                // HACK: replace with a System.Uri comparison or Path.GetFullPath to prevent against canonicalization attacks.  BD
            else if (Utility.HasValue(this.ThumbnailSubdirectory) &&
                     this._thumbnailUrl.StartsWith(Utility.GetThumbnailLibraryPath(this.PortalId).ToString(), StringComparison.OrdinalIgnoreCase))
            {
                this.rblThumbnailImage.SelectedValue = ThumbnailImageType.Internal.ToString();

                if (this.ddlThumbnailLibrary.Items.Contains(new ListItem(Path.GetFileName(this._thumbnailUrl))))
                {
                    this.ddlThumbnailLibrary.SelectedValue = Path.GetFileName(this._thumbnailUrl);
                    this.mvThumbnailImage.SetActiveView(this.vwInternal);
                }
                else
                {
                    this.mvThumbnailImage.SetActiveView(this.vwUpload);
                }
            }
            else
            {
                this.rblThumbnailImage.SelectedValue = ThumbnailImageType.External.ToString();
                this.txtThumbnailUrl.Text = this._thumbnailUrl;
                this.mvThumbnailImage.SetActiveView(this.vwExternal);
            }
        }

        private bool IsUploadValid()
        {
            if (!this.fileThumbnail.HasFile)
            {
                this.txtMessage.Text = Localization.GetString("FileRequired", this.LocalResourceFile);
            }
            else
            {
                string extension = Path.GetExtension(this.fileThumbnail.PostedFile.FileName);
                if (Utility.HasValue(extension))
                {
                    extension = extension.Substring(1);
                    string[] validExtensions = Utility.MediaFileTypes.Split(',');

                    foreach (string ext in validExtensions)
                    {
                        if (extension.Equals(ext, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }

                this.txtMessage.Text = String.Format(
                    CultureInfo.CurrentCulture, Localization.GetString("FileExtension", this.LocalResourceFile), Utility.MediaFileTypes);
            }

            return false;
        }
    }
}