namespace FacebookEndpoint
{
    using System;
    using NServiceBus;
    using NServiceBusDemo.Commands;

    public class PostContentToFacebookPageHandler : IHandleMessages<PostContentToFacebookPage>
    {
        public IBus Bus { get; set; }

        public void Handle(PostContentToFacebookPage message)
        {
            if (DateTime.Now < message.TimeToPost)
            {
                Bus.Defer(message.TimeToPost, message);
            }

            //TODO: Post content to FB
        }
    }
}
