using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkInfo
{
    public class IPEndPoint
    {
        public enum ProtocoleType
        {
            //
            // Summary: 
            //     TCP.
            TCP = 0, // tut buv Vasia
            //
            // Summary:
            //     UDP.
            UDP = 1
        }
        public enum IPEndPointStatus
        {
            //
            // Summary:
            //     Listen.
            Listen = 0,
            //
            // Summary:
            //     Unknown.
            Connected = 1,
            //
            // Summary:
            //     Unknown.
            Unknown = 2
        }
        #region Singleton Properties

        private static IPEndPoint[] InstanceArray = null;

        #endregion

        #region Const

        //
        // Summary:
        //     Specifies the maximum value that can be assigned to the IPEndPoint.Port
        //     property. The MaxPort value is set to 0x0000FFFF. This field is read-only.
        public const int MaxPort = 65535;
        //
        // Summary:
        //     Specifies the minimum value that can be assigned to the IPEndPoint.Port
        //     property. This field is read-only.
        public const int MinPort = 0;

        #endregion

        #region Properties

        //
        // Summary:
        //     Gets the IP address of the endpoint.
        //
        // Returns:
        //     An string containing the IP address of the endpoint.
        public string Address { get; private set; }
        //
        // Summary:
        //     Gets the port number of the endpoint.
        //
        // Returns:
        //     An integer value in the range IPEndPoint.MinPort to IPEndPoint.MaxPort indicating the port number of the endpoint.
        public int Port { get; private set; }
        //
        // Summary:
        //     Gets the trancfer protocol type of the endpoint.
        //
        // Returns:
        //     An enum value from NA.Domain.Models.Enums.ProtocoleType of the endpoint.
        public ProtocoleType Type { get; private set; }
        //
        // Summary:
        //     Gets the current status of the endpoint.
        //
        // Returns:
        //     An enum value from NA.Domain.Models.Enums.IPEndPointStatus of the endpoint.
        public IPEndPointStatus Status { get; private set; }
        //
        // Summary:
        //     Gets the current IP address of endpoint connection.
        //
        // Returns:
        //     An string value of current IP address endpoint connection.
        public string Connection { get; private set; }

        #endregion

        #region Constructor

        public IPEndPoint(string Address, int Port, ProtocoleType Type, IPEndPointStatus Status)
        {
            this.Address = !string.IsNullOrEmpty(Address) ? Address : throw new ArgumentNullException("Address");
            this.Port = Port >= MinPort && Port <= MaxPort ? Port : throw new ArgumentOutOfRangeException("Port");
            this.Type = Type;
            this.Status = Status;
        }

        public IPEndPoint(System.Net.IPEndPoint IPEndPoint, ProtocoleType Type, IPEndPointStatus Status, string Connection = "")
        {
            this.Address = IPEndPoint.Address.ToString();
            this.Port = IPEndPoint.Port;
            this.Type = Type;
            this.Status = Status;
            if (!string.IsNullOrEmpty(Connection)) this.Connection = Connection;
        }

        public static IPEndPoint[] GetIPEndPoints()
        {
            if (InstanceArray == null)
            {
                var IPEndPoints = new List<IPEndPoint>();
                foreach (var IPEndPoint in System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections())
                {
                    IPEndPoints.Add(
                        new IPEndPoint(
                            IPEndPoint.LocalEndPoint,
                            ProtocoleType.TCP,
                            IPEndPointStatus.Connected,
                            IPEndPoint.RemoteEndPoint.ToString()
                            ));
                }
                foreach (var IPEndPoint in System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners())
                {
                    IPEndPoints.Add(
                        new IPEndPoint(
                            IPEndPoint,
                            ProtocoleType.TCP,
                            IPEndPointStatus.Listen
                            ));
                }
                foreach (var IPEndPoint in System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners())
                {
                    IPEndPoints.Add(
                        new IPEndPoint(
                            IPEndPoint,
                            ProtocoleType.UDP,
                            IPEndPointStatus.Listen
                            ));
                }
                InstanceArray = IPEndPoints.ToArray();
            }
            return InstanceArray;
        }

        #endregion

        #region Methods

        public override string ToString() =>
            Address + ":" + Port.ToString();

        #endregion

    }
}
