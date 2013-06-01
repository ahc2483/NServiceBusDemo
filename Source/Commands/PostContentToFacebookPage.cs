namespace NServiceBusDemo.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NServiceBus;

    public class PostContentToFacebookPage : ICommand
    {
        public string PostScheduleId { get; set; }
        public string PostId { get; set; }
        public string PageId { get; set; }
        public string PostContent { get; set; }
        public DateTime TimeToPost { get; set; }
    }
}
