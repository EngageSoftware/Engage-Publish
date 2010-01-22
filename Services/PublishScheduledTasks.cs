using Engage.Dnn.Publish.Data;

namespace Engage.Dnn.Publish.Services
{
    using System;


    public class PublishScheduledTasks : DotNetNuke.Services.Scheduling.SchedulerClient
    {
        public PublishScheduledTasks(DotNetNuke.Services.Scheduling.ScheduleHistoryItem shi)
        {
            ScheduleHistoryItem = shi;
        }

    public override void DoWork()
        {
            try
            {
                Progressing();
                //DO THE WORK
                DataProvider.Instance().RunPublishStats();
                string returnMessage = "";
                returnMessage += " <br /> Stats Parsed Successfully";
                ScheduleHistoryItem.Succeeded = true;
                ScheduleHistoryItem.AddLogNote(returnMessage);

            }
            catch (Exception exc)
            {
                ScheduleHistoryItem.Succeeded = false;
                ScheduleHistoryItem.AddLogNote("Stats Failed, did you configure the settings?");
                Errored(ref exc);
            }
        }
    }
}
