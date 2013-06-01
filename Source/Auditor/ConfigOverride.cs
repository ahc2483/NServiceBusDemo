using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusDemo.Auditor
{
    /// <summary>
    /// Configuration
    /// </summary>
    public class ConfigOverride : IProvideConfiguration<UnicastBusConfig>
    {
        /// <summary>
        /// Configure our unicast bus
        /// </summary>
        UnicastBusConfig IProvideConfiguration<UnicastBusConfig>.GetConfiguration()
        {
            var mappings = new MessageEndpointMappingCollection();
            mappings.Add(new MessageEndpointMapping { 
                AssemblyName = "NServiceBusDemo.Messages",
                Endpoint = "content_publishing_scheduler",
                Messages = "NServiceBusDemo.Messages"
            });
            return new UnicastBusConfig
            {
                MessageEndpointMappings = mappings
            };
        }
    }
}
