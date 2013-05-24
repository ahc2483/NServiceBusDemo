namespace NServiceBusDemo.ContentPublishingService
{
    using System.Collections.Generic;
    using NServiceBus.Saga;

    public class ContentPostData : IContainSagaData
    {
        public string PostScheduleId { get; set; }
        public Dictionary<string, string> PagePosts { get; set; }
    }
}
