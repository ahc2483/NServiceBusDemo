namespace NServiceBusDemo.PostSchedulingComponent.Tests
{
    using System;
    using System.Collections.Generic;
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

        [TestMethod]
        public void SchedulePostsTest()
        {
            Test.Initialize();

            TimeSpan expectedWait = TimeSpan.FromSeconds(10);
            Dictionary<string, string> testPosts = new Dictionary<string, string>();
            testPosts.Add("123", "test");
            testPosts.Add("456", "test");

            Test.Saga<ProcessPosts>()
                .ExpectTimeoutToBeSetIn<UndoTimeout>((timeoutMessage, at) => at == expectedWait)
            .When(saga => saga.Handle(new ScheduleContentPosts(){
                PagePosts = testPosts,
                PostScheduleId = "123456"
            }))
                .ExpectSend<PostContentToFacebookPage>(m => String.Equals(m.PageId, "123"))
                .ExpectSend<PostContentToFacebookPage>(m => DateTime.Now < m.TimeToPost && String.Equals(m.PageId, "456"))
            .WhenSagaTimesOut()
                .AssertSagaCompletionIs(true);
        }
    }
}
