// <copyright file="OverrideableDisplayOptionsBase.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    using DotNetNuke.Entities.Modules;

    public class OverrideableDisplayOptionsBase : ModuleSettingsBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether to set the control to display its initial values (regardless of whether it's in a post-back or not).
        /// </summary>
        /// <value>
        /// <c>true</c> if this control should set the fields to their initial values (i.e. the values stored in the database); otherwise, <c>false</c>.
        /// </value>
        public bool ForceSetInitialValues { get; set; }
    }
}