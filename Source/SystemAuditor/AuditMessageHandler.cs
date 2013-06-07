namespace NServiceBusDemo.SystemAuditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NServiceBus;

    public class AuditMessageHandler : IHandleMessages<IMessage>
    {
        public void Handle(IMessage message)
        {
            Console.WriteLine(message);
        }
    }
}
