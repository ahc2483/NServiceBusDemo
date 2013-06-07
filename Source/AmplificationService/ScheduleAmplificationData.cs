namespace NServiceBusDemo.AmplificationService
{
    using System;
    using System.Collections.Generic;
    using NServiceBus.Saga;

    public class ScheduleAmplificationData : IContainSagaData
    {
        public ScheduleAmplificationData()
        {
            this.CompletedAmplifications = new List<string>();
        }

        #region IContainSagaData Members

        public Guid Id { get; set; }
        public string OriginalMessageId { get; set; }
        public string Originator { get; set; }

        #endregion

        [Unique]
        public string PostScheduleId { get; set; }
        public IEnumerable<string> PostsToAmplify { get; set; }
        public IEnumerable<string> CompletedAmplifications { get; set; }
    }
}
