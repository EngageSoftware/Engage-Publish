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