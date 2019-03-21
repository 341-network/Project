namespace NetworkInfo
{
    public class NetworkPropertiesService
    {
        #region Fields

        private readonly NetworkProperties properties = NetworkProperties.GetNetworkProperties();

        #endregion Fields

        #region Methods

        public NetworkProperties GetNetworkProperties() =>
            properties;

        #endregion Methods
    }
}