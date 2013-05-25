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
        public string PageId { get; set; }
        public string PostContent { get; set; }
    }
}
