namespace FacebookEndpoint
{
    using System;
    using NServiceBus;
    using NServiceBusDemo.Commands;
    using NServiceBusDemo.Messages;

    public class AmplifyPostHandler : IHandleMessages<AmplifyPost>
    {
        public IBus Bus { get; set; }

        public void Handle(AmplifyPost message)
        {
            Console.WriteLine("Amplifying post with id: {0}", message.PostId);

            Bus.Reply<AmplifyPostResponse>(response =>
            {
                response.PostId = message.PostId;
            });
        }
    }
}
