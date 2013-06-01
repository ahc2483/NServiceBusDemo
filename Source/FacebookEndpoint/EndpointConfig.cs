namespace FacebookEndpoint 
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://nservicebus.com/GenericHost.aspx
	*/
    [EndpointName("facebook_endpoint")]
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization, ISpecifyMessageHandlerOrdering
    {
        public void Init()
        {
            Configure.With(AllAssemblies.Except("FacebookEndpoint.vshost.exe"));
        }

        /// <summary>
        /// Define the ordering of handlers
        /// </summary>
        /// <param name="order"></param>
        public void SpecifyOrder(Order order)
        {
            order.Specify(First<FacebookEndpointAuthHandler>.Then<PostContentToFacebookPageHandler>());
        }
    }
}