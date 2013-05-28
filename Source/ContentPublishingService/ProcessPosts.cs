namespace NServiceBusDemo.ContentPublishingService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NServiceBus;
    using NServiceBus.Saga;
    using NServiceBusDemo.Commands;

    public class ProcessPosts : Saga<ProcessPostsData>,
        IAmStartedByMessages<ScheduleContentPosts>,
        IHandleMessages<UndoSchedulingOfContent>,
        IHandleTimeouts<UndoTimeout>
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

            //Wait 10 seconds to allow for undo
            this.RequestUtcTimeout<UndoTimeout>(TimeSpan.FromSeconds(10));
        }

        public void Handle(UndoSchedulingOfContent message)
        {
            if(this.Data.UndoStillAllowed)
                this.MarkAsComplete();
        }

        public void Timeout(UndoTimeout undoWindow)
        {
            this.Data.UndoStillAllowed = false;
            this.SchedulePosts();   
        }

        #region Private Members

        private void SchedulePosts()
        {
            int postCount = 0;

            foreach (KeyValuePair<string, string> pagePost in this.Data.PagePosts)
            {
                Bus.Send<PostContentToFacebookPage>(newPost =>
                {
                    newPost.PageId = pagePost.Key;
                    newPost.PostContent = pagePost.Value;
                    newPost.TimeToPost = DateTime.Now.AddSeconds(postCount * 30); // Should configure this interval using config
                });

                postCount++;
            }

            this.MarkAsComplete();   
        }

        #endregion
    }
}
