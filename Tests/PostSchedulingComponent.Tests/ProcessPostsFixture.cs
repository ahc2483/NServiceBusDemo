namespace NServiceBusDemo.PostSchedulingComponent.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NServiceBus.Testing;
    using NServiceBusDemo.Commands;
    using NServiceBusDemo.ContentPublishingService;

    [TestClass]
    public class ProcessPostsFixture
    {
        [TestMethod]
        public void AllowForUndoTest()
        {
            Test.Initialize();

            TimeSpan expectedWait = TimeSpan.FromSeconds(10);

            Test.Saga<ProcessPosts>()
                .ExpectTimeoutToBeSetIn<UndoTimeout>((timeoutMessage, at) => at == expectedWait)
                .When(saga => saga.Handle(new ScheduleContentPosts()));
        }
    }
}
