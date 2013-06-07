namespace NServiceBusDemo.Auditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NServiceBus;

    [EndpointName("billing_service")]
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
    }
}
