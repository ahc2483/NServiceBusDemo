using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusDemo.Commands
{
    public static class CommandHeaders
    {
        public static class Keys
        {
            public static readonly string Originator = "originator";
        }
    }
}
