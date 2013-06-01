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

            var posts = new Dictionary<string, string>{
                {"page1", "post me!"},
                {"page2", "post me!"},
                {"page3", "post me!"}
            };

            Test.Saga<ProcessPosts>()
                .ExpectTimeoutToBeSetIn<UndoTimeout>((timeoutMessage, at) => at == expectedWait)
                .When(saga => saga.Handle(new ScheduleContentPosts(){
                    PagePosts = posts,
                    PostScheduleId = "123456"
                }))
                .ExpectSend<PostContentToFacebookPage>(m => String.Equals(m.PageId, "page1"))
                .ExpectSend<PostContentToFacebookPage>(m => DateTime.Now < m.TimeToPost && string.Equals(m.PageId, "page2"))
                .ExpectSend<PostContentToFacebookPage>(m=> DateTime.Now < m.TimeToPost && string.Equals(m.PageId, "page3"))
                
                .WhenSagaTimesOut()
                .AssertSagaCompletionIs(true);
        }
    }
}
