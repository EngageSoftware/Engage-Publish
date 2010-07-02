namespace Engage.Dnn.Publish.Services
{
    using System;

    using DotNetNuke.Services.Scheduling;

    using Engage.Dnn.Publish.Data;

    public class PublishScheduledTasks : SchedulerClient
    {
        public PublishScheduledTasks(ScheduleHistoryItem shi)
        {
            this.ScheduleHistoryItem = shi;
        }

        public override void DoWork()
        {
            try
            {
                this.Progressing();

                // DO THE WORK
                DataProvider.Instance().RunPublishStats();
                string returnMessage = string.Empty;
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