namespace NServiceBusDemo.Auditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NServiceBus;

    public class MessageHandler : IHandleMessages<IMessage>
    {
        public void Handle(IMessage message)
        {
            Console.WriteLine(message);
        }
    }
}
