using System;

namespace NetworkInfo
{
    public class NetworkProperties
    {
        public enum NetBiosNodeType
        {
            //
            // Summary:
            //     An unknown node type.
            Unknown = 0,

            //
            // Summary:
            //     A broadcast node.
            Broadcast = 1,

            //
            // Summary:
            //     A peer-to-peer node.
            Peer2Peer = 2,

            //
            // Summary:
            //     A mixed node.
            Mixed = 4,

            //
            // Summary:
            //     A hybrid node.
            Hybrid = 8
        }

        #region Singleton Properties

        private static NetworkProperties Instance = null;

        #endregion Singleton Properties

        #region Properties

        //
        // Summary:
        //     Gets the host name for the local computer.
        //
        // Returns:
        //     A string instance that contains the computer's NetBIOS name.
        //
        // Exceptions:
        //   T:System.Net.NetworkInformation.NetworkInformationException:
        //     A Win32 function call failed.
        public string HostName { get; private set; }

        //
        // Summary:
        //     Gets the domain in which the local computer is registered.
        //
        // Returns:
        //     A string instance that contains the computer's domain name. If the
        //     computer does not belong to a domain, returns empty string.
        //
        // Exceptions:
        //   T:System.Net.NetworkInformation.NetworkInformationException:
        //     A Win32 function call failed.
        public string DomainName { get; private set; }

        //
        // Summary:
        //     Gets the Dynamic Host Configuration Protocol (DHCP) scope name.
        //
        // Returns:
        //     A string instance that contains the computer's DHCP scope name.
        //
        // Exceptions:
        //   T:System.Net.NetworkInformation.NetworkInformationException:
        //     A Win32 function call failed.
        public string DhcpScopeName { get; private set; }

        //
        // Summary:
        //     Gets a boolean value that specifies whether the local computer is acting
        //     as a Windows Internet Name Service (WINS) proxy.
        //
        // Returns:
        //     true if the local computer is a WINS proxy; otherwise, false.
        //
        // Exceptions:
        //   T:System.Net.NetworkInformation.NetworkInformationException:
        //     A Win32 function call failed.
        public bool IsWinsProxy { get; private set; }

        //
        // Summary:
        //     Gets the Network Basic Input/Output System (NetBIOS) node type of the local computer.
        //
        // Returns:
        //     A NA.Domain.Models.Enums.NetBiosNodeType value.
        //
        // Exceptions:
        //   T:System.Net.NetworkInformation.NetworkInformationException:
        //     A Win32 function call failed.
        public NetBiosNodeType NodeType { get; private set; }

        //
        // Summary:
        //     Indicates whether any network connection is available.
        //
        // Returns:
        //     true if a network connection is available; otherwise, false.
        public bool IsNetworkAvailable =>
            System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

        //
        // Summary:
        //     Gets the number of bytes that were received on the all interfaces.
        //
        // Returns:
        //     Returns System.Int64. The total number of bytes that were received on the interface.
        public long BytesReceived
        {
            get
            {
                var networkInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                long data = 0;
                foreach (var networkInterface in networkInterfaces)
                    data += networkInterface.GetIPv4Statistics().BytesReceived;
                return data;
            }
        }

        //
        // Summary:
        //     Gets the number of bytes that were sent on the all interfaces.
        //
        // Returns:
        //     Returns System.Int64. The total number of bytes that were sent on the interface.
        public long BytesSent
        {
            get
            {
                var networkInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                long data = 0;
                foreach (var networkInterface in networkInterfaces)
                    data += networkInterface.GetIPv4Statistics().BytesSent;
                return data;
            }
        }

        //
        // Summary:
        //     Gets NetworkInterface array of configuration information for a network interfaces.
        public NetworkInterface[] NetworkInterfaces { get; private set; }

        //
        // Summary:
        //     Gets IPEndPoint array of configuration information for a IP end points.
        public IPEndPoint[] IPEndPoints { get; private set; }

        #endregion Properties

        #region Constructors

        private NetworkProperties()
        {
            var properties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();

            HostName = properties.HostName;
            DomainName = properties.DomainName;
            DhcpScopeName = properties.DhcpScopeName;
            IsWinsProxy = properties.IsWinsProxy;
            if (Enum.TryParse(properties.NodeType.ToString(), out NetBiosNodeType netBiosNodeType))
                NodeType = netBiosNodeType;

            NetworkInterfaces = NetworkInterface.GetNetworkInterfaces();
            IPEndPoints = IPEndPoint.GetIPEndPoints();
        }

        public static NetworkProperties GetNetworkProperties() =>
            Instance ?? (Instance = new NetworkProperties());

        #endregion Constructors
    }
}