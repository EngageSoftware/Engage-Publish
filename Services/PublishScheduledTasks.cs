using Engage.Dnn.Publish.Data;

namespace Engage.Dnn.Publish.Services
{
    using System.Collections.Generic;
    using System.Globalization;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Security;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Services.Exceptions;

    using System;
    using System.Collections;
    using System.Data;
    using Engage.Dnn.Publish;
    using Engage.Dnn.Publish.Util;
    using System.Text;


    public class PublishScheduledTasks : DotNetNuke.Services.Scheduling.SchedulerClient
    {
        public PublishScheduledTasks(DotNetNuke.Services.Scheduling.ScheduleHistoryItem shi)
            : base()
        {
            this.ScheduleHistoryItem = shi;
        }

    public override void DoWork()
        {
            try
            {
                this.Progressing();
                //DO THE WORK
                DataProvider.Instance().RunPublishStats();
                string returnMessage = "";
                returnMessage += " <br /> Stats Parsed Successfully";
                this.ScheduleHistoryItem.Succeeded = true;
                this.ScheduleHistoryItem.AddLogNote(returnMessage);

            }
            catch (Exception exc)
            {
                this.ScheduleHistoryItem.Succeeded = false;
                this.ScheduleHistoryItem.AddLogNote("Stats Failed, did you configure the settings?");
                this.Errored(ref exc);
            }
        }
    }
}
