// <copyright file="ResetDisplayPage.ascx.cs" company="Engage Software">
// Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Admin.Tools
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    /// <summary>
    /// A control which allows users to reset the display page of all children of a category to that category's Child Display Page.
    /// </summary>
    public partial class ResetDisplayPage : ModuleBase
    {
        /// <summary>
        /// Backing field for <see cref="ParentCategory"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Category parentCategory;

        /// <summary>
        /// Gets the currently selected parent category.
        /// </summary>
        /// <value>The parent category.</value>
        private Category ParentCategory
        {
            get
            {
                if (this.parentCategory == null)
                {
                    int parentCategoryId;
                    if (int.TryParse(this.ParentCategoryDropDownList.SelectedValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out parentCategoryId))
                    {
                        this.parentCategory = Category.GetCategory(parentCategoryId);
                    }
                }

                return this.parentCategory;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += Page_Load;
            this.ParentCategoryDropDownList.SelectedIndexChanged += this.ParentCategoryDropDownList_SelectedIndexChanged;
            this.ResetButton.Click += this.ResetButton_Click;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.FillParentCategoryDropDown();
                    this.FillDisplayPageLabel();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ParentCategoryDropDownList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ParentCategoryDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FillDisplayPageLabel();
        }

        /// <summary>
        /// Handles the Click event of the ResetButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ResetButton_Click(object sender, EventArgs e)
        {
            if (this.ParentCategory != null)
            {
                int affectedCount = this.ParentCategory.CascadeChildDisplayTab(this.UserId);

                this.SuccessMessage.Text = string.Format(
                    CultureInfo.CurrentCulture, 
                    Localization.GetString("SuccessMessage.Text", this.LocalResourceFile), 
                    affectedCount,
                    this.ParentCategory.Name);
            }
        }

        /// <summary>
        /// Fills the ParentCategoryDropDownList control.
        /// </summary>
        private void FillParentCategoryDropDown()
        {
            ItemRelationship.DisplayCategoryHierarchy(this.ParentCategoryDropDownList, -1, this.PortalId, false);
        }

        /// <summary>
        /// Fills the DisplayPageLabel control with the ChildDisplayTabName of the current <see cref="ParentCategory"/>.
        /// </summary>
        private void FillDisplayPageLabel()
        {
            if (this.ParentCategory != null)
            {
                this.DisplayPageLabel.Text = this.ParentCategory.ChildDisplayTabName;
            }
        }
    }
}

