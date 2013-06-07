namespace NServiceBusDemo.ContentPublishingService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NServiceBus;
    using NServiceBus.Saga;
    using NServiceBusDemo.Commands;
    using NServiceBusDemo.Messages;

    public class ProcessPosts : Saga<ProcessPostsData>,
        IAmStartedByMessages<ScheduleContentPosts>,
        IHandleMessages<UndoSchedulingOfContent>,
        IHandleMessages<ContentWasPostedToFacebookPage>,
        IHandleMessages<ContentFailedToPostToFacebookPage>
    {
        #region overrides
        /// <summary>
        /// For now... I might implement a sagafinder later if there's time
        /// </summary>
        public override void ConfigureHowToFindSaga()
        {
            this.ConfigureMapping<UndoSchedulingOfContent>(sagaData => sagaData.PostScheduleId, udc => udc.PostScheduleId);
            this.ConfigureMapping<ContentWasPostedToFacebookPage>(sagaData => sagaData.PostScheduleId, m => m.PostScheduleId);
            this.ConfigureMapping<ContentFailedToPostToFacebookPage>(sagaData => sagaData.PostScheduleId, m => m.PostScheduleId);
        }
        #endregion

        #region handlers
        /// <summary>
        /// Handles the schedule posts command
        /// </summary>
        public void Handle(ScheduleContentPosts message)
        {
            if (null == message || null == message.PagePosts || message.PagePosts.Count() < 1)
            {
                this.MarkAsComplete();
                return;
            }

            Console.WriteLine("ScheduleContentPosts message recieved, posting in 10 senconds...");

            this.Data.PostScheduleId = message.PostScheduleId;
            this.Data.PagePosts = message.PagePosts;

            this.Data.RemainingPosts = new List<string>();


            this.Data.UndoStillAllowed = true;

            //Wait 10 seconds to allow for undo
            RequestUtcTimeout(TimeSpan.FromSeconds(10), "undo window closed");
        }

        /// <summary>
        /// Handles the undo command... cancels the postschedule
        /// </summary>
        public void Handle(UndoSchedulingOfContent message)
        {
            if (this.Data.UndoStillAllowed)
            {
                Console.WriteLine(string.Format("Post schedule {0} has been cancelled", message.PostScheduleId));
                this.MarkAsComplete();
                return;
            }
            Console.WriteLine("Whatever embarrassing thing you've just posted... suck it up, cause it's too late to cancel it!");
        }

        /// <summary>
        /// The window to cancel postschedule has closed
        /// </summary>
        [Obsolete]
        public override void Timeout(object state)
        {
            Console.WriteLine(string.Format("Commencing post schedule: {0}", this.Data.PostScheduleId));

            this.Data.UndoStillAllowed = false;
            this.SchedulePosts();
        }

        /// <summary>
        /// Handle the message that we've successfully posted some content
        /// </summary>
        /// <param name="message"></param>
        public void Handle(ContentWasPostedToFacebookPage message)
        {
            this.Data.RemainingPosts.Remove(message.PostId);
            CheckRemaining();
        }

        /// <summary>
        /// Schedule failed to complete.
        /// </summary>
        /// <param name="message"></param>
        public void Handle(ContentFailedToPostToFacebookPage message)
        {
            this.Data.RemainingPosts.Remove(message.PostId);
            CheckRemaining();
        }
        #endregion

        #region Private Members

        private void SchedulePosts()
        {
            int postCount = 0;

            foreach (KeyValuePair<string, string> pagePost in this.Data.PagePosts)
            {
                string postId = Guid.NewGuid().ToString();
                this.Data.RemainingPosts.Add(postId);

                var cmd = new PostContentToFacebookPage
                {
                    PostScheduleId = this.Data.PostScheduleId,
                    PostId = postId,
                    PageId = pagePost.Key,
                    PostContent = pagePost.Value,
                    TimeToPost = DateTime.Now.AddSeconds(postCount * 5) // Should configure this interval using config
                };
                // Because we're going to potentially defer this message, we need some way to identify
                // where the message originally came from
                cmd.SetHeader(CommandHeaders.Keys.Originator, EndpointConfig.EndpointName);
                Bus.Send("facebook_endpoint", cmd);

                postCount++;
            }
        }

        /// <summary>
        /// Checks if there are any remaining posts to respond.
        /// </summary>
        private void CheckRemaining()
        {
            if (this.Data.RemainingPosts.Count == 0)
            {
                Console.WriteLine("Schedule has completed. Publishing to the world...");

                Bus.Publish<PostScheduleCompleted>(m =>
                {
                    m.PostScheduleId = this.Data.PostScheduleId;
                });
                this.MarkAsComplete();  
            }
        }
        #endregion
    }
}
