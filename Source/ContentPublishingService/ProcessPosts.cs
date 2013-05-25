namespace NServiceBusDemo.ContentPublishingService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NServiceBus;
    using NServiceBus.Saga;
    using NServiceBusDemo.Commands;

    public class ProcessPosts : Saga<ContentPostData>,
        IAmStartedByMessages<ScheduleContentPosts>,
        IHandleMessages<UndoSchedulingOfContent>,
        IHandleTimeouts<UndoTimeout>,
        IHandleTimeouts<DelayBetweenPosts>
    {
        public override void ConfigureHowToFindSaga()
        {
            this.ConfigureMapping<UndoSchedulingOfContent>(sagaData => sagaData.PostScheduleId, udc => udc.PostScheduleId);
        }

        public void Handle(ScheduleContentPosts message)
        {
            this.Data.PostScheduleId = message.PostScheduleId;
            this.Data.PagePosts = message.PagePosts;
            this.Data.UndoStillAllowed = true;

            //Wait 5 minutes to allow for undo
            this.RequestUtcTimeout<UndoTimeout>(TimeSpan.FromMinutes(5));
        }

        public void Handle(UndoSchedulingOfContent message)
        {
            if(this.Data.UndoStillAllowed)
                this.MarkAsComplete();
        }

        public void Timeout(UndoTimeout undoWindow)
        {
            this.Data.UndoStillAllowed = false;
            this.ProcessNextPost();   
        }

        public void Timeout(DelayBetweenPosts delayState)
        {
            this.ProcessNextPost();
        }

        #region Private Members

        private void ProcessNextPost()
        {
            KeyValuePair<string, string> contentToPost = this.Data.PagePosts.First();

            Bus.Send<PostContentToFacebookPage>(pagePost =>
            {
                pagePost.PageId = contentToPost.Key;
                pagePost.PostContent = contentToPost.Value;
            });

            this.Data.PagePosts.Remove(contentToPost.Key);

            if (this.Data.PagePosts.Count() > 0)
            {
                //Schedule the next post to occur in 30 seconds
                this.RequestUtcTimeout<DelayBetweenPosts>(TimeSpan.FromSeconds(30));
            }
            else
            {
                this.MarkAsComplete();
            }    
        }

        #endregion
    }
}
