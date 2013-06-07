namespace NServiceBusDemo.ContentPublishingService
{
    using System;
    using System.Collections.Generic;
    using NServiceBus.Saga;
    using System.Collections.ObjectModel;

    public class ProcessPostsData : IContainSagaData
    {
        public ProcessPostsData()
        {
            this.RemainingPosts = new List<string>();
            this.CompletedPosts = new List<string>();
        }

        #region IContainSagaData Members

        public System.Guid Id { get; set; }
        public string OriginalMessageId { get; set; }
        public string Originator { get; set; }

        #endregion

        [Unique]
        public string PostScheduleId { get; set; }
        public Dictionary<string, string> PagePosts { get; set; }
        public List<string> RemainingPosts { get; set; }
        public List<string> CompletedPosts { get; set; }
        public bool UndoStillAllowed { get; set; }
    }
}
