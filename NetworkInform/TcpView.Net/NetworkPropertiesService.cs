using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpView.Net
{
    public class NetworkPropertiesService
    {

        private readonly NetworkProperties properties = NetworkProperties.GetNetworkProperties();

        public NetworkProperties GetNetworkProperties() =>
            properties;

    }
}
