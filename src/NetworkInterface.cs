using System;
using System.Collections.Generic;

namespace NetworkInfo
{
    public class NetworkInterface
    {
        public enum OperationalStatus
        {
            //
            // Summary:
            //     The network interface is up; it can transmit data packets.
            Up = 1,

            //
            // Summary:
            //     The network interface is unable to transmit data packets.
            Down = 2,

            //
            // Summary:
            //     The network interface is running tests.
            Testing = 3,

            //
            // Summary:
            //     The network interface status is not known.
            Unknown = 4,

            //
            // Summary:
            //     The network interface is not in a condition to transmit data packets; it is waiting
            //     for an external event.
            Dormant = 5,

            //
            // Summary:
            //     The network interface is unable to transmit data packets because of a missing
            //     component, typically a hardware component.
            NotPresent = 6,

            //
            // Summary:
            //     The network interface is unable to transmit data packets because it runs on top
            //     of one or more other interfaces, and at least one of these &quot;lower layer&quot;
            //     interfaces is down.
            LowerLayerDown = 7
        }

        public enum NetworkInterfaceType
        {
            //
            // Summary:
            //     The interface type is not known.
            Unknown = 1,

            //
            // Summary:
            //     The network interface uses an Ethernet connection. Ethernet is defined in IEEE
            //     standard 802.3.
            Ethernet = 6,

            //
            // Summary:
            //     The network interface uses a Token-Ring connection. Token-Ring is defined in
            //     IEEE standard 802.5.
            TokenRing = 9,

            //
            // Summary:
            //     The network interface uses a Fiber Distributed Data Interface (FDDI) connection.
            //     FDDI is a set of standards for data transmission on fiber optic lines in a local
            //     area network.
            Fddi = 15,

            //
            // Summary:
            //     The network interface uses a basic rate interface Integrated Services Digital
            //     Network (ISDN) connection. ISDN is a set of standards for data transmission over
            //     telephone lines.
            BasicIsdn = 20,

            //
            // Summary:
            //     The network interface uses a primary rate interface Integrated Services Digital
            //     Network (ISDN) connection. ISDN is a set of standards for data transmission over
            //     telephone lines.
            PrimaryIsdn = 21,

            //
            // Summary:
            //     The network interface uses a Point-To-Point protocol (PPP) connection. PPP is
            //     a protocol for data transmission using a serial device.
            Ppp = 23,

            //
            // Summary:
            //     The network interface is a loopback adapter. Such interfaces are often used for
            //     testing; no traffic is sent over the wire.
            Loopback = 24,

            //
            // Summary:
            //     The network interface uses an Ethernet 3 megabit/second connection. This version
            //     of Ethernet is defined in IETF RFC 895.
            Ethernet3Megabit = 26,

            //
            // Summary:
            //     The network interface uses a Serial Line Internet Protocol (SLIP) connection.
            //     SLIP is defined in IETF RFC 1055.
            Slip = 28,

            //
            // Summary:
            //     The network interface uses asynchronous transfer mode (ATM) for data transmission.
            Atm = 37,

            //
            // Summary:
            //     The network interface uses a modem.
            GenericModem = 48,

            //
            // Summary:
            //     The network interface uses a Fast Ethernet connection over twisted pair and provides
            //     a data rate of 100 megabits per second. This type of connection is also known
            //     as 100Base-T.
            FastEthernetT = 62,

            //
            // Summary:
            //     The network interface uses a connection configured for ISDN and the X.25 protocol.
            //     X.25 allows computers on public networks to communicate using an intermediary
            //     computer.
            Isdn = 63,

            //
            // Summary:
            //     The network interface uses a Fast Ethernet connection over optical fiber and
            //     provides a data rate of 100 megabits per second. This type of connection is also
            //     known as 100Base-FX.
            FastEthernetFx = 69,

            //
            // Summary:
            //     The network interface uses a wireless LAN connection (IEEE 802.11 standard).
            Wireless80211 = 71,

            //
            // Summary:
            //     The network interface uses an Asymmetric Digital Subscriber Line (ADSL).
            AsymmetricDsl = 94,

            //
            // Summary:
            //     The network interface uses a Rate Adaptive Digital Subscriber Line (RADSL).
            RateAdaptDsl = 95,

            //
            // Summary:
            //     The network interface uses a Symmetric Digital Subscriber Line (SDSL).
            SymmetricDsl = 96,

            //
            // Summary:
            //     The network interface uses a Very High Data Rate Digital Subscriber Line (VDSL).
            VeryHighSpeedDsl = 97,

            //
            // Summary:
            //     The network interface uses the Internet Protocol (IP) in combination with asynchronous
            //     transfer mode (ATM) for data transmission.
            IPOverAtm = 114,

            //
            // Summary:
            //     The network interface uses a gigabit Ethernet connection and provides a data
            //     rate of 1,000 megabits per second (1 gigabit per second).
            GigabitEthernet = 117,

            //
            // Summary:
            //     The network interface uses a tunnel connection.
            Tunnel = 131,

            //
            // Summary:
            //     The network interface uses a Multirate Digital Subscriber Line.
            MultiRateSymmetricDsl = 143,

            //
            // Summary:
            //     The network interface uses a High Performance Serial Bus.
            HighPerformanceSerialBus = 144,

            //
            // Summary:
            //     The network interface uses a mobile broadband interface for WiMax devices.
            Wman = 237,

            //
            // Summary:
            //     The network interface uses a mobile broadband interface for GSM-based devices.
            Wwanpp = 243,

            //
            // Summary:
            //     The network interface uses a mobile broadband interface for CDMA-based devices.
            Wwanpp2 = 244
        }

        #region Singleton Properties

        private static NetworkInterface[] InstanceArray = null;

        #endregion Singleton Properties

        #region Properties

        //
        // Summary:
        //     Gets the identifier of the network adapter.
        //
        // Returns:
        //     A int32 that contains the identifier.
        public string Id { get; private set; }

        //
        // Summary:
        //     Gets the name of the network adapter.
        //
        // Returns:
        //     A string that contains the adapter name.
        public string Name { get; private set; }

        //
        // Summary:
        //     Gets the description of the interface.
        //
        // Returns:
        //     A string that describes this interface.
        public string Description { get; private set; }

        //
        // Summary:
        //     Returns the Media Access Control (MAC) or physical address for this adapter.
        //
        // Returns:
        //     A string that contains the physical address.
        public string PhysicalAddress { get; private set; }

        //
        // Summary:
        //     Gets the interface type.
        //
        // Returns:
        //     An NA.Domain.Models.Enums.NetworkInterfaceType value that specifies the
        //     network interface type.
        public NetworkInterfaceType Type { get; private set; }

        //
        // Summary:
        //     Gets the current operational state of the network connection.
        //
        // Returns:
        //     One of the NA.Domain.Models.Enums.OperationalStatus values.
        public OperationalStatus Status { get; private set; }

        //
        // Summary:
        //     Gets a boolean value that indicates whether the network interface is set
        //     to only receive data packets.
        //
        // Returns:
        //     true if the interface only receives network traffic; otherwise, false.
        //
        // Exceptions:
        //   T:System.PlatformNotSupportedException:
        //     This property is not valid on computers running operating systems earlier than
        //     Windows XP.
        public bool IsReceiveOnly { get; private set; }

        //
        // Summary:
        //     Gets a boolean value that indicates whether NetBt is configured to use
        //     DNS name resolution on this interface.
        //
        // Returns:
        //     true if NetBt is configured to use DNS name resolution on this interface; otherwise,
        //     false.
        public bool IsDnsEnabled { get; private set; }

        //
        // Summary:
        //     Gets a boolean value that indicates whether this interface is configured
        //     to automatically register its IP address information with the Domain Name System
        //     (DNS).
        //
        // Returns:
        //     true if this interface is configured to automatically register a mapping between
        //     its dynamic IP address and static domain names; otherwise, false.
        public bool IsDynamicDnsEnabled { get; private set; }

        //
        // Summary:
        //     Gets the Domain Name System (DNS) suffix associated with this interface.
        //
        // Returns:
        //     A string that contains the DNS suffix for this interface, or System.String.Empty
        //     if there is no DNS suffix for the interface.
        //
        // Exceptions:
        //   T:System.PlatformNotSupportedException:
        //     This property is not valid on computers running operating systems earlier than
        //     Windows 2000.
        public string DnsSuffix { get; private set; }

        //
        // Summary:
        //     Gets the speed of the network interface.
        //
        // Returns:
        //     A int64 value that specifies the speed in bits per second.
        public long Speed { get; private set; }

        //
        // Summary:
        //     Gets a boolean value that indicates whether the network interface is enabled
        //     to receive multicast packets.
        //
        // Returns:
        //     true if the interface receives multicast packets; otherwise, false.
        //
        // Exceptions:
        //   T:System.PlatformNotSupportedException:
        //     This property is not valid on computers running operating systems earlier than
        //     Windows XP.
        public bool SupportsMulticast { get; private set; }

        #endregion Properties

        #region Constructor

        private NetworkInterface(System.Net.NetworkInformation.NetworkInterface networkInterface)
        {
            var properties = networkInterface.GetIPProperties();

            Id = networkInterface.Id;
            Name = networkInterface.Name;
            Description = networkInterface.Description;
            PhysicalAddress = networkInterface.GetPhysicalAddress().ToString();
            if (Enum.TryParse(networkInterface.NetworkInterfaceType.ToString(), out NetworkInterfaceType type)) Type = type;
            if (Enum.TryParse(networkInterface.OperationalStatus.ToString(), out OperationalStatus status)) Status = status;
            IsReceiveOnly = networkInterface.IsReceiveOnly;
            IsDnsEnabled = properties.IsDnsEnabled;
            IsDynamicDnsEnabled = properties.IsDynamicDnsEnabled;
            DnsSuffix = properties.DnsSuffix;
            Speed = networkInterface.Speed;
            SupportsMulticast = networkInterface.SupportsMulticast;
        }

        public static NetworkInterface[] GetNetworkInterfaces()
        {
            if (InstanceArray == null)
            {
                var adapters = new List<NetworkInterface>();
                foreach (var adapter in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                    adapters.Add(new NetworkInterface(adapter));
                InstanceArray = adapters.ToArray();
            }
            return InstanceArray;
        }

        #endregion Constructor
    }
}