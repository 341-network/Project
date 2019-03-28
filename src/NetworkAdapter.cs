using System.Collections.Generic;
using System.Management;

namespace NetworkInfo
{
    public class NetworkAdapter
    {
        #region Fields

        private ManagementObject _adapter;

        #endregion Fields

        #region Constructors

        private NetworkAdapter()
        {
        }

        #endregion Constructors

        #region Properties

        public string AdapterType
        {
            get
            {
                return (string)(_adapter["AdapterType"] ?? "");
            }
        }

        public uint Index
        {
            get
            {
                return (uint)(_adapter["Index"] ?? -1);
            }
        }

        public string IPAddress
        {
            get
            {
                return (string)(_adapter["PermanentAddress"] ?? "");
            }
        }

        public bool IsNetEnabled
        {
            get
            {
                return (bool)(_adapter["NetEnabled"] ?? false);
            }
        }

        public bool IsPhysicalAdapter
        {
            get
            {
                return (bool)(_adapter["PhysicalAdapter"] ?? false);
            }
        }

        public string MACAddress
        {
            get
            {
                return (string)(_adapter["MACAddress"] ?? "");
            }
        }

        public string NetConnectionID
        {
            get
            {
                return (string)(_adapter["NetConnectionID"] ?? "");
            }
        }

        public string ProductName
        {
            get
            {
                return (string)(_adapter["ProductName"] ?? "");
            }
        }

        public string ServiceName
        {
            get
            {
                return (string)(_adapter["ServiceName"] ?? "");
            }
        }

        #endregion Properties

        #region Methods

        public static List<NetworkAdapter> GetAllNetworkAdapters()
        {
            var adapters = new List<NetworkAdapter>();
            foreach (var adapter in new ManagementClass("Win32_NetworkAdapter").GetInstances())
            {
                adapters.Add(new NetworkAdapter
                {
                    _adapter = (ManagementObject)adapter
                });
            }
            return adapters;
        }

        public bool CanDisableOrEnable()
        {
            return !string.IsNullOrEmpty(NetConnectionID);
        }

        public void Disable()
        {
            if (!string.IsNullOrEmpty(NetConnectionID))
            {
                _adapter.InvokeMethod("Disable", null);
            }
        }

        public void Enable()
        {
            if (!string.IsNullOrEmpty(NetConnectionID))
            {
                _adapter.InvokeMethod("Enable", null);
            }
        }

        #endregion Methods
    }
}