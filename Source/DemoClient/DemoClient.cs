namespace DemoClient
{
    using System;
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBusDemo.Commands;

    public class DemoClient : IWantToRunAtStartup
    {
        #region properties/fields
        /// <summary>
        /// Our bus - perhaps we'll inject this differently
        /// </summary>
        public IBus Bus { get; set; }
        #endregion

        #region IWantToRunAtStartup members
        public void Run()
        {
            Console.WriteLine("Press 'enter' to post:");
            var input = Console.ReadKey();
            if (input.Key == ConsoleKey.Enter)
            {
                string postScheduleId = Guid.NewGuid().ToString();
                Bus.Send<ScheduleContentPosts>("content_publishing_scheduler", m =>
                {
                    m.PostScheduleId = postScheduleId;
                    m.PagePosts = new Dictionary<string, string>
                    {
                        {"page1", "post me!"},
                        {"page2", "post me!"},
                        {"page3", "post me!"}
                    };
                });
                Console.WriteLine("ScheduleContentPosts command sent!  Press 'enter' to cancel (within 10) seconds.");
                input = Console.ReadKey();
                if (input.Key == ConsoleKey.Enter)
                {
                    Bus.Send<UndoSchedulingOfContent>("content_publishing_scheduler", m =>
                    {
                        m.PostScheduleId = postScheduleId;
                    });
                    Console.WriteLine("");
                    Console.WriteLine("Cancelling...");
                }
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
