using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace MTorrent
{
    public enum IpVersion : uint
    {
        IPv4 = 2,
        IPv6 = 23
    }

    public static class NetworkInformation
    {
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

        private const int ErrorInsufficientBuffer = 122;
        private const int Successfully = 0;

        [DllImport("iphlpapi.dll", SetLastError = true)]
        private static extern uint GetExtendedTcpTable(IntPtr pTcpTable, ref int dwOutBufLen, bool sort,
            IpVersion ipVersion, TcpTableClass tblClass, int reserved);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        private static extern uint GetExtendedUdpTable(IntPtr pTcpTable, ref int dwOutBufLen, bool sort,
            IpVersion ipVersion, UdpTableClass tblClass, int reserved);

        public static List<Connection> GetProcessTcpActivity(int pid)
        {
            Tcp6RowOwnerPid[] t6 = GetTcpConnections<Tcp6RowOwnerPid>(IpVersion.IPv6);
            TcpRowOwnerPid[] t4 = GetTcpConnections<TcpRowOwnerPid>(IpVersion.IPv4);

            List<Connection> list = new List<Connection>();

            for (int i = 0; i < t6.Length; i++)
            {
                if (pid < 0 || t6[i].OwningPid == pid)
                {
                    list.Add(new Connection(ref t6[i]));
                }
            }

            for (int i = 0; i < t4.Length; i++)
            {
                if (pid < 0 || t4[i].OwningPid == pid)
                {
                    list.Add(new Connection(ref t4[i]));
                }
            }

            return list;
        }

        public static Connection[] GetTcpV6Connections()
        {
            Tcp6RowOwnerPid[] t = GetTcpConnections<Tcp6RowOwnerPid>(IpVersion.IPv6);
            Connection[] connectInfo = new Connection[t.Length];
            for (int i = 0; i < t.Length; i++)
            {
                connectInfo[i] = new Connection(ref t[i]);
            }
            return connectInfo;
        }

        public static Connection[] GetTcpV4Connections()
        {
            TcpRowOwnerPid[] t = GetTcpConnections<TcpRowOwnerPid>(IpVersion.IPv4);
            Connection[] connectInfo = new Connection[t.Length];
            for (int i = 0; i < t.Length; i++)
            {
                connectInfo[i] = new Connection(ref t[i]);
            }
            return connectInfo;
        }

        public static Connection[] GetUdpV4Connections()
        {
            UdpRowOwnerPid[] t = GetUdpConnections<UdpRowOwnerPid>(IpVersion.IPv4);
            Connection[] connectInfo = new Connection[t.Length];
            for (int i = 0; i < t.Length; i++)
            {
                connectInfo[i] = new Connection(ref t[i]);
            }
            return connectInfo;
        }

        public static Connection[] GetUdpV6Connections()
        {
            Udp6RowOwnerPid[] t = GetUdpConnections<Udp6RowOwnerPid>(IpVersion.IPv6);

            Connection[] connectInfo = new Connection[t.Length];
            for (int i = 0; i < t.Length; i++)
            {
                connectInfo[i] = new Connection(ref t[i]);
            }
            return connectInfo;
        }

        private static void ReadData<T>(IntPtr buffer, out T[] tTable)
        {
            Type rowType = typeof(T);
            int sizeRow = Marshal.SizeOf(rowType);
            long buffAddress = buffer.ToInt64();

            int count = Marshal.ReadInt32(buffer);
            int offcet = Marshal.SizeOf(typeof(Int32));

            tTable = new T[count];
            for (int i = 0; i < tTable.Length; i++)
            {
                //calc position for next array element
                var memoryPos = new IntPtr(buffAddress + offcet);
                //read element
                tTable[i] = (T)Marshal.PtrToStructure(memoryPos, rowType);

                offcet += sizeRow;
            }
        }

        private static T[] GetUdpConnections<T>(IpVersion ipVersion)
        {
            T[] tTable;

            int buffSize = 0;

            // how much memory do we need?
            GetExtendedUdpTable(IntPtr.Zero, ref buffSize, false, ipVersion, UdpTableClass.UdpTableOwnerPid, 0);

            IntPtr buffer = Marshal.AllocHGlobal(buffSize);
            try
            {
                uint retVal = GetExtendedUdpTable(buffer, ref buffSize, false, ipVersion,
                                                  UdpTableClass.UdpTableOwnerPid, 0);

                while (retVal == ErrorInsufficientBuffer) //buffer should be greater?
                {
                    buffer = Marshal.ReAllocHGlobal(buffer, new IntPtr(buffSize));
                    retVal = GetExtendedUdpTable(buffer, ref buffSize, false, ipVersion, UdpTableClass.UdpTableOwnerPid, 0);
                }

                if (retVal != Successfully)
                    return null;

                ReadData(buffer, out tTable);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                // Free the Memory
                Marshal.FreeHGlobal(buffer);
            }
            return tTable;
        }

        private static T[] GetTcpConnections<T>(IpVersion ipVersion)
        {
            T[] tTable;

            int buffSize = 0;

            // how much memory do we need?
            GetExtendedTcpTable(IntPtr.Zero, ref buffSize, false, ipVersion, TcpTableClass.TcpTableOwnerPidAll, 0);

            IntPtr buffer = Marshal.AllocHGlobal(buffSize);
            try
            {
                uint retVal = GetExtendedTcpTable(buffer, ref buffSize, false, ipVersion,
                                                  TcpTableClass.TcpTableOwnerPidAll, 0);

                while (retVal == ErrorInsufficientBuffer) //buffer should be greater?
                {
                    buffer = Marshal.ReAllocHGlobal(buffer, new IntPtr(buffSize));
                    retVal = GetExtendedTcpTable(buffer, ref buffSize, false, ipVersion,
                                                 TcpTableClass.TcpTableOwnerPidAll, 0);
                }

                if (retVal != Successfully) return null;

                ReadData(buffer, out tTable);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                // Free the Memory
                Marshal.FreeHGlobal(buffer);
            }
            return tTable;
        }

        #region Nested type: Tcp6RowOwnerPid

        [StructLayout(LayoutKind.Sequential)]
        public struct Tcp6RowOwnerPid
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] LocalAddr;

            public uint LocalScopeId;
            public uint LocalPort;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] RemoteAddr;

            public uint RemoteScopeId;
            public uint RemotePort;
            public TcpState State;
            public uint OwningPid;
        }

        #endregion Nested type: Tcp6RowOwnerPid

        #region Nested type: TcpRowOwnerPid

        [StructLayout(LayoutKind.Sequential)]
        public struct TcpRowOwnerPid
        {
            public TcpState State;
            public uint LocalAddr;
            public uint LocalPort;
            public uint RemoteAddr;
            public uint RemotePort;
            public uint OwningPid;
        }

        #endregion Nested type: TcpRowOwnerPid

        #region Nested type: TcpTableClass

        private enum TcpTableClass
        {
            TcpTableBasicListener,
            TcpTableBasicConnections,
            TcpTableBasicAll,
            TcpTableOwnerPidListener,
            TcpTableOwnerPidConnections,
            TcpTableOwnerPidAll,
            TcpTableOwnerModuleListener,
            TcpTableOwnerModuleConnections,
            TcpTableOwnerModuleAll
        }

        #endregion Nested type: TcpTableClass

        #region Nested type: Udp6RowOwnerPid

        [StructLayout(LayoutKind.Sequential)]
        public struct Udp6RowOwnerPid
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] LocalAddr;

            public uint LocalScopeId;
            public uint LocalPort;
            public uint OwningPid;
        }

        #endregion Nested type: Udp6RowOwnerPid

        #region Nested type: UdpRowOwnerPid

        [StructLayout(LayoutKind.Sequential)]
        public struct UdpRowOwnerPid
        {
            public uint LocalAddr;
            public uint LocalPort;
            public uint OwningPid;
        }

        #endregion Nested type: UdpRowOwnerPid

        #region Nested type: UdpTableClass

        internal enum UdpTableClass
        {
            UdpTableBasic,
            UdpTableOwnerPid,
            UdpTableOwnerModule
        }

        #endregion Nested type: UdpTableClass
    }
}