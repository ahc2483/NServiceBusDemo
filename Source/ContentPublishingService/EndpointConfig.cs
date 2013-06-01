namespace NServiceBusDemo.ContentPublishingService 
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://nservicebus.com/GenericHost.aspx
	*/
    [EndpointName("content_publishing_scheduler")]
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher
    {
        private static string _EndpointName;
        /// <summary>
        /// Gets the name of the endpoint
        /// </summary>
        public static string EndpointName 
        {
            get
            {
                if (null == _EndpointName)
                {
                    // We could probably just return the name here as a constant string... There's probably not
                    // that much value in actually reflecting on the type aside from having to specify the endpoint name in
                    // more than one place
                    var endpoingNameAttr = typeof(EndpointConfig).GetCustomAttributes(typeof(EndpointNameAttribute), false)[0] as EndpointNameAttribute;
                    if (null != endpoingNameAttr)
                        _EndpointName = endpoingNameAttr.Name;
                }
                return _EndpointName;
            }
        }
    }
}