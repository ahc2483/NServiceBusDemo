﻿using NServiceBus;
using NServiceBus.Unicast;
using NServiceBusDemo.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoClient
{
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
            Console.WriteLine("Press enter to post");
            var input = Console.ReadKey();
            if (input.Key == ConsoleKey.Enter)
            {
                Bus.Send<ScheduleContentPosts>("content_publishing_scheduler", m =>
                {
                    m.PostScheduleId = Guid.NewGuid().ToString();
                    m.PagePosts = new Dictionary<string, string>
                    {
                        {"page1", "post me!"},
                        {"page2", "post me!"},
                        {"page3", "post me!"}
                    };
                });
                Console.WriteLine("ScheduleContentPosts command sent!");
            }

            
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
