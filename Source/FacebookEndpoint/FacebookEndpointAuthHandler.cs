using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookEndpoint
{
    public class FacebookEndpointAuthHandler : IHandleMessages<IMessage>
    {
        #region properties/fields
        /// <summary>
        /// Dependency injection FTW!
        /// </summary>
        public IBus Bus { get; set; }
        #endregion

        #region IHandleMessages<T> members
        /// <summary>
        /// Intercept all messages, auth (just for show)
        /// </summary>
        /// <param name="message"></param>
        public void Handle(IMessage message)
        {
            // If we're not authed, do not continue
            if (!IsAuthed())
                Bus.DoNotContinueDispatchingCurrentMessageToHandlers();
        }
        #endregion

        #region private methods
        private bool IsAuthed()
        {
            return true;
        }
        #endregion
    }
}
