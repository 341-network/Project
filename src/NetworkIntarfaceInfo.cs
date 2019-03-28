using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace NetworkInfo
{
    public class NetworkIntarfaceInfo
    {
        public uint Index { get; private set; }
        public string ServiceName { get; private set; }
        public string MACAddress { get; private set; }
        public string IPAddress { get; private set; }
        public string Description { get; private set; }

        public NetworkIntarfaceInfo()
        {
        }

        public static List<NetworkIntarfaceInfo> GetAllNetworkInterfaces()
        {
            var adapters = new List<NetworkIntarfaceInfo>();
            foreach (var adapter in new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances())
            {
                adapters.Add(new NetworkIntarfaceInfo
                {
                    Index = (uint)adapter["Index"],
                    ServiceName = (string)adapter["ServiceName"],
                    MACAddress = (string)adapter["MACAddress"],
                    IPAddress = ((string[])adapter["IPAddress"])?[0] ?? "",
                    Description = (string)adapter["Description"]
                });
            }
            return adapters;
        }
    }
}