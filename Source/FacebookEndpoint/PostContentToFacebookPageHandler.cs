namespace FacebookEndpoint
{
    using System;
    using NServiceBus;
    using NServiceBusDemo.Commands;

    public class PostContentToFacebookPageHandler : IHandleMessages<PostContentToFacebookPage>
    {
        public void Handle(PostContentToFacebookPage message)
        {
            throw new NotImplementedException();
        }
    }
}
