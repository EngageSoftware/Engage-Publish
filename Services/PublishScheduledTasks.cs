// <copyright file="PublishScheduledTasks.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

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