namespace NServiceBusDemo.ContentPublishingService
{
    using System;
    using System.Collections.Generic;
    using NServiceBus.Saga;
    using System.Collections.ObjectModel;

    public class ProcessPostsData : IContainSagaData
    {
        #region IContainSagaData Members

        public System.Guid Id { get; set; }
        public string OriginalMessageId { get; set; }
        public string Originator { get; set; }

        #endregion

        public string PostScheduleId { get; set; }
        public Dictionary<string, string> PagePosts { get; set; }
        public List<string> RemainingPosts { get; set; }
        public bool UndoStillAllowed { get; set; }
    }
}
