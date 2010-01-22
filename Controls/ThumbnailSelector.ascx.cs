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
    using System.Globalization;
    using System.IO;
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Util;

    public partial class ThumbnailSelector : ModuleBase
	{
		#region Event Handlers
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            //TODO: Once we upgrade to or past 4.5.3, include this statement so that there won't be problems if someone has wrapped Publish with an update panel.
            //DotNetNuke.Framework.AJAX.RegisterPostBackControl(btnUploadThumbnail);
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            Load += Page_Load;
        }

		protected void Page_Load(object sender, EventArgs e)
		{
			try 
			{
				//check VI for null then set information
				if (!Page.IsPostBack)
				{

                    if (ThumbnailSelectionOption == ThumbnailDisplayOption.DotNetNuke.ToString())
                    {
                        pnlDnnThumb.Visible = true;
                        pnlEngageThumb.Visible = false;
                    }
                    else
                    {
                        pnlEngageThumb.Visible = true;
                        pnlDnnThumb.Visible = false;
                        InitializeThumbnailControl();
                    }

				}
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

        private void InitializeThumbnailControl()
        {
            rblThumbnailImage.Items.Clear();
            rblThumbnailImage.Items.Add(new ListItem(Localization.GetString(ThumbnailImageType.Upload.ToString(), LocalResourceFile), ThumbnailImageType.Upload.ToString()));
            rblThumbnailImage.Items.Add(new ListItem(Localization.GetString(ThumbnailImageType.Internal.ToString(), LocalResourceFile), ThumbnailImageType.Internal.ToString()));
            rblThumbnailImage.Items.Add(new ListItem(Localization.GetString(ThumbnailImageType.External.ToString(), LocalResourceFile), ThumbnailImageType.External.ToString()));

            var files = new StringCollection();
            foreach (string file in Directory.GetFiles(Utility.GetThumbnailLibraryMapPath(PortalId).AbsolutePath))
	        {
                files.Add(Path.GetFileName(file));		 
	        }

            ddlThumbnailLibrary.Items.Clear();
            ddlThumbnailLibrary.DataSource = files;
            ddlThumbnailLibrary.DataBind();
            ddlThumbnailLibrary.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), string.Empty));

            if (!Utility.HasValue(_thumbnailUrl))
            {
                rblThumbnailImage.SelectedValue = ThumbnailImageType.Upload.ToString();
            }
            //HACK: replace with a System.Uri comparison or Path.GetFullPath to prevent against canonicalization attacks.  BD
            else if (Utility.HasValue(ThumbnailSubdirectory) && _thumbnailUrl.StartsWith(Utility.GetThumbnailLibraryPath(PortalId).ToString(), StringComparison.OrdinalIgnoreCase))
            {
                rblThumbnailImage.SelectedValue = ThumbnailImageType.Internal.ToString();

                if (ddlThumbnailLibrary.Items.Contains(new ListItem(Path.GetFileName(_thumbnailUrl))))
                {
                    ddlThumbnailLibrary.SelectedValue = Path.GetFileName(_thumbnailUrl);
                    mvThumbnailImage.SetActiveView(vwInternal);
                }
                else
                {
                    mvThumbnailImage.SetActiveView(vwUpload);
                }
            }
            else
            {
                rblThumbnailImage.SelectedValue = ThumbnailImageType.External.ToString();
                txtThumbnailUrl.Text = _thumbnailUrl;
                mvThumbnailImage.SetActiveView(vwExternal);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void rblThumbnailImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            View activeView;// = null;
            if (rblThumbnailImage.SelectedValue == ThumbnailImageType.Internal.ToString())
            {
                activeView = vwInternal;
            }
            else if (rblThumbnailImage.SelectedValue == ThumbnailImageType.External.ToString())
            {
                activeView = vwExternal;
            }
            else //ThumbnailImageType.Upload.ToString(CultureInfo.InvariantCulture)
            {
                activeView = vwUpload;
            }

            mvThumbnailImage.SetActiveView(activeView);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void btnUploadThumbnail_Click(object sender, EventArgs e)
        {
            if (IsUploadValid())
            {
                string path = Utility.GetThumbnailLibraryMapPath(PortalId).AbsolutePath;
                string filename = Path.GetFileNameWithoutExtension(fileThumbnail.PostedFile.FileName);
                string extension = Path.GetExtension(fileThumbnail.PostedFile.FileName);

                if (File.Exists(Path.Combine(path, filename + extension)))
                {
                    int i = 0;
                    while (File.Exists(Path.Combine(path, String.Format(CultureInfo.InvariantCulture, "{0}[{1}]{2}", filename, i, extension))))
                    {
                        i++;
                    }
                    filename = String.Format(CultureInfo.InvariantCulture, "{0}[{1}]", filename, i);
                }

                fileThumbnail.PostedFile.SaveAs(Path.Combine(path, filename + extension));

                _thumbnailUrl = Path.Combine(Utility.GetThumbnailLibraryPath(PortalId).ToString(), filename + extension);

                InitializeThumbnailControl();
            }
            else
            {
                txtMessage.Visible = true;
            }
        }

        private bool IsUploadValid()
        {
            if (!fileThumbnail.HasFile)
            {
                txtMessage.Text = Localization.GetString("FileRequired", LocalResourceFile);
            }
            else
            {
                string extension = Path.GetExtension(fileThumbnail.PostedFile.FileName);
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

                txtMessage.Text = String.Format(CultureInfo.CurrentCulture, Localization.GetString("FileExtension", LocalResourceFile), Utility.MediaFileTypes);
            }
            return false;
        }
		#endregion

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _thumbnailUrl;
        public string ThumbnailUrl
        {
            get
            {
                //Check which tool

                if (ThumbnailSelectionOption == ThumbnailDisplayOption.DotNetNuke.ToString())
                {
                    return ctlMediaFile.Url;
                }
                if (rblThumbnailImage.SelectedValue == ThumbnailImageType.Internal.ToString())
                {
                    return Path.Combine(Utility.GetThumbnailLibraryPath(PortalId).ToString(), ddlThumbnailLibrary.SelectedValue);
                }
                if (rblThumbnailImage.SelectedValue == ThumbnailImageType.External.ToString())
                {
                    return txtThumbnailUrl.Text;
                }
                return null;
            }
            [DebuggerStepThrough]
            set
            {
                //check option
                if (ThumbnailSelectionOption == ThumbnailDisplayOption.DotNetNuke.ToString())
                {
                    ctlMediaFile.Url = value;
                    ctlMediaFile.UrlType = "F";
                    ctlMediaFile.FileFilter = Utility.MediaFileTypes;
                }
                else
                {
                    _thumbnailUrl = value;
                }
                
            }
        }

        private enum ThumbnailImageType
        {
            Upload,
            External,
            Internal
        }

    }
}

