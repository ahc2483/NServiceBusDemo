namespace NServiceBusDemo.AmplificationService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NServiceBus;
    using NServiceBus.Saga;
    using NServiceBusDemo.Commands;
    using NServiceBusDemo.Events;
    using NServiceBusDemo.Messages;

    public class ScheduleAmplification : Saga<ScheduleAmplificationData>,
        IAmStartedByMessages<PostScheduleAccepted>,
        IHandleMessages<PostScheduleCompleted>,
        IHandleMessages<AmplifyPostResponse>
    {
        public override void ConfigureHowToFindSaga()
        {
            this.ConfigureMapping<PostScheduleAccepted>(sagaData => sagaData.PostScheduleId, udc => udc.PostScheduleId);
            this.ConfigureMapping<PostScheduleCompleted>(sagaData => sagaData.PostScheduleId, udc => udc.PostScheduleId);
        }

        public void Handle(PostScheduleAccepted message)
        {
            this.Data.PostScheduleId = message.PostScheduleId;
            Console.WriteLine("Received schedule that should be amplified");
        }

        public void Handle(PostScheduleCompleted message)
        {
            Console.WriteLine("Schedule will now be amplified");
            this.Data.PostsToAmplify = message.Successes;
            this.AmplifyPosts(message.Successes.ToArray());
        }

        public void Handle(AmplifyPostResponse message)
        {
            ((List<string>)this.Data.CompletedAmplifications).Add(message.PostId);
            this.CheckForCompletion();
        }

        #region Private Members

        private void AmplifyPosts(string[] postIds)
        {
            foreach (string postId in postIds)
            {
                Bus.Send<AmplifyPost>(command =>
                {
                    command.PostId = postId;
                });
            }
        }

        private void CheckForCompletion()
        {
            if (this.Data.CompletedAmplifications.Count() == this.Data.PostsToAmplify.Count())
            {
                Console.WriteLine("Amplification completed.");

                Bus.Publish<PostScheduleAmplificationCompleted>(evt =>
                    {
                        evt.PostScheduleId = this.Data.PostScheduleId;
                    });

                this.MarkAsComplete();
            }
        }

        #endregion
    }
}
