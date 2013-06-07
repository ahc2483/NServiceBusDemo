namespace NServiceBusDemo.SystemAuditor
{
    using System;
    using NServiceBus;

    public class AuditMessageHandler : IHandleMessages<IMessage>
    {
        public void Handle(IMessage message)
        {
            Console.WriteLine(message);
        }
    }
}
