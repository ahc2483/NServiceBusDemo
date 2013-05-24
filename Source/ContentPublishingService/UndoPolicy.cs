namespace NServiceBusDemo.ContentPublishingService
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;
    using NServiceBusDemo.Commands;

    public class PostContentToPage : Saga<ContentPostData>,
        IAmStartedByMessages<ScheduleContentPosts>,
        IHandleMessages<UndoSchedulingOfContent>
    {
        public IBus Bus { get; set; }

        public override void ConfigureHowToFindSaga()
        {
            this.ConfigureMapping<UndoSchedulingOfContent>(sagaData => sagaData.PostScheduleId, udc => udc.PostScheduleId);
        }

        public void Handle(ScheduleContentPosts message)
        {
            this.Data.PostScheduleId = message.PostScheduleId;

            //Wait 5 minutes to allow for undo
            this.RequestUtcTimeout(TimeSpan.FromMinutes(5), "Post scheduling was not undone");
        }

        public void Handle(UndoSchedulingOfContent message)
        {
            this.MarkAsComplete();
        }

        public override void Timeout(object state)
        {
            //Undo window has expired, send the post
            Bus.Send<ProcessScheduledPosts>(command =>
            {
                command.PagePosts = this.Data.PagePosts;
            });
        }
    }
}
