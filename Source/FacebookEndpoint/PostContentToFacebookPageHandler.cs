namespace FacebookEndpoint
{
    using System;
    using NServiceBus;
    using NServiceBusDemo.Commands;
    using NServiceBusDemo.Messages;

    public class PostContentToFacebookPageHandler : IHandleMessages<PostContentToFacebookPage>
    {
        public IBus Bus { get; set; }

        public void Handle(PostContentToFacebookPage message)
        {
            // Again, because we're potentially deferring the message, we need some way to identify where the message originated
            string originator = message.GetHeader(CommandHeaders.Keys.Originator);

            if (DateTime.Now < message.TimeToPost)
            {
                message.SetHeader(CommandHeaders.Keys.Originator, originator);
                Bus.Defer(message.TimeToPost, message);
                return;
            }

            try
            {
                Console.WriteLine(string.Format("Posting content: \"{0}\" to facebook page: {1}", message.PostContent, message.PageId));
                Bus.Send<ContentWasPostedToFacebookPage>(originator, m =>
                { 
                    m.PageId = message.PageId;
                    m.PostId = message.PostId;
                    m.PostScheduleId = message.PostScheduleId;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Posting content: \"{0}\" to facebook page: {1} failed", message.PostContent, message.PageId));
                Bus.Send<ContentFailedToPostToFacebookPage>(originator, m =>
                {
                    m.PageId = message.PageId;
                    m.PostId = message.PostId;
                    m.PostScheduleId = message.PostScheduleId;
                });
            }
        }
    }
}
