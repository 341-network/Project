namespace NetworkInfo
{
    public class NetworkPropertiesService
    {
        private readonly NetworkProperties properties = NetworkProperties.GetNetworkProperties();

        public NetworkProperties GetNetworkProperties() =>
            properties;
    }
}