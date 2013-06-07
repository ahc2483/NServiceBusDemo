using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Unicast.Queuing.Msmq;
using NServiceBus.Unicast.Transport;
using NServiceBus.Unicast.Transport.Transactional;
using NServiceBus.Utils;

namespace NServiceBusDemo.SystemAuditor
{
    /// <summary>
    /// Create an in-memory TransactionalTransport and point it to the AuditQ, and serialize any message
    /// that is received in the Q to the persistent store
    /// </summary>
    public class AuditMessagePersister : IWantToRunAtStartup
    {
        public IBus Bus { get; set; }
        public ITransport AuditMessageQueueTransport;
        public TransactionalTransport CurrentEndpointTransport { get; set; }

        public void Run()
        {
            string auditQueue = "system_auditor";
            string machineName = Environment.MachineName;

            // Make sure that the queue being monitored, exists!
            if (!MessageQueue.Exists(MsmqUtilities.GetFullPathWithoutPrefix(auditQueue)))
            {
                // The error queue being monitored must be local to this endpoint
                throw new Exception(string.Format("The audit queue {0} being monitored must be local to this endpoint and must exist. Make sure a transactional queue by the specified name exists. The audit queue to be monitored is specified in the app.config", auditQueue));
            }

            // Create an in-memory transport with the same configuration as that of the current endpoint.
            AuditMessageQueueTransport = new TransactionalTransport()
            {
                IsTransactional = CurrentEndpointTransport.IsTransactional,
                MaxRetries = CurrentEndpointTransport.MaxRetries,
                IsolationLevel = CurrentEndpointTransport.IsolationLevel,
                MessageReceiver = new MsmqMessageReceiver(),
                NumberOfWorkerThreads = CurrentEndpointTransport.NumberOfWorkerThreads,
                TransactionTimeout = CurrentEndpointTransport.TransactionTimeout,
                FailureManager = CurrentEndpointTransport.FailureManager
            };

            AuditMessageQueueTransport.Start(new Address(auditQueue, machineName));
            AuditMessageQueueTransport.TransportMessageReceived += new EventHandler<TransportMessageReceivedEventArgs>(AuditMessageQueueTransport_TransportMessageReceived);
        }

        public void Stop()
        {
            AuditMessageQueueTransport.TransportMessageReceived -= new EventHandler<TransportMessageReceivedEventArgs>(AuditMessageQueueTransport_TransportMessageReceived);
        }

        void AuditMessageQueueTransport_TransportMessageReceived(object sender, TransportMessageReceivedEventArgs e)
        {
            var message = e.Message;

            var messageBodyXml = System.Text.Encoding.UTF8.GetString(message.Body);

            // Save the message
            Console.WriteLine("Saving {0}", messageBodyXml);
        }

    }
}
