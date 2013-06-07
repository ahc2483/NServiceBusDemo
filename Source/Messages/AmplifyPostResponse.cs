namespace NServiceBusDemo.Messages
{
    using NServiceBus;

    public class AmplifyPostResponse : IMessage
    {
        public string PostId;
    }
}
