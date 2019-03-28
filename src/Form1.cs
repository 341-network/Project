using MTorrent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Management;

namespace NetworkInfo
{
    public partial class Form1 : Form
    {
        #region Fields

        private List<Connection> _allConnections = new List<Connection>();
        private int selectedProcessId = -1;

        #endregion Fields

        #region Constructors

        public Form1()
        {
            InitializeComponent();
            UpdateInfo();
        }

        #endregion Constructors

        #region Methods

        public void ShowNetworkProperties()
        {
            var namesOfInterfaces = new System.Text.StringBuilder();
            foreach (var item in new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances())
            {
                namesOfInterfaces.AppendLine(item["Caption"].ToString());
            }
            MessageBox.Show(namesOfInterfaces.ToString());

            //dataGridView2.Columns[0].Width = 400;
            //var properties = new NetworkPropertiesService().GetNetworkProperties();
            //dataGridView2.Rows.Clear();

            //dataGridView2.Rows.Add(" Network information of " + properties.HostName + "." + properties.DomainName);
            //dataGridView2.Rows.Add(" DHCP scope name : " + properties.DhcpScopeName);
            //dataGridView2.Rows.Add(" WINS proxy : " + properties.IsWinsProxy);
            //dataGridView2.Rows.Add(" Network BIOS node type : " + properties.NodeType);
            //dataGridView2.Rows.Add("Is network available : " + properties.IsNetworkAvailable);
            //dataGridView2.Rows.Add(" Network interfaces : " + properties.NetworkInterfaces.Length);
            //foreach (var adapter in properties.NetworkInterfaces)
            //{
            //    dataGridView2.Rows.Add();
            //    dataGridView2.Rows.Add("\n " + adapter.Description + " " + adapter.Id);
            //    dataGridView2.Rows.Add(" " + string.Empty.PadLeft(adapter.Description.Length + 1 + adapter.Id.Length, '='));
            //    dataGridView2.Rows.Add("Name : " + adapter.Name);
            //    dataGridView2.Rows.Add("Type : " + adapter.Type);
            //    dataGridView2.Rows.Add("Speed : " + adapter.Speed);
            //    dataGridView2.Rows.Add("Operational status : " + adapter.Status);
            //    dataGridView2.Rows.Add("Physical Address : " + adapter.PhysicalAddress);
            //    dataGridView2.Rows.Add("Is Receive Only : " + adapter.IsReceiveOnly);
            //    dataGridView2.Rows.Add("Is DNS enabled : " + adapter.IsDnsEnabled);
            //    dataGridView2.Rows.Add("Is dynamic DNS enabled : " + adapter.IsDynamicDnsEnabled);
            //    dataGridView2.Rows.Add("Supports Multicast : " + adapter.SupportsMulticast);
            //}
            //dataGridView2.Rows.Add("\n IP End Points .............. : " + properties.IPEndPoints.Length);

            //foreach (var ipEndPoint in properties.IPEndPoints)
            //{
            //    dataGridView2.Rows.Add("\n " + ipEndPoint);
            //    dataGridView2.Rows.Add("Type : " + ipEndPoint.Type);
            //    dataGridView2.Rows.Add("Status : " + ipEndPoint.Status);
            //    dataGridView2.Rows.Add("Status : " + ipEndPoint.Status);
            //    dataGridView2.Rows.Add("Connection : " + ipEndPoint.Connection);
            //}
        }

        private void Button1Click(object sender, EventArgs e)
        {
            UpdateInfo();
            UpdateProcesses();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowNetworkProperties();
        }

        private void CheckBox3CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            WriteToGrid(selectedProcessId);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateInfo();
            UpdateProcesses();
            ShowNetworkProperties();
        }

        private void UpdateInfo()
        {
            _allConnections.Clear();
            _allConnections.AddRange(NetworkInformation.GetTcpV4Connections());
            _allConnections.AddRange(NetworkInformation.GetTcpV6Connections());
            _allConnections.AddRange(NetworkInformation.GetUdpV4Connections());
            _allConnections.AddRange(NetworkInformation.GetUdpV6Connections());
        }

        private void UpdateProcesses()
        {
            Connection.UpdateProcessList();
            dataGridView1.Rows.Clear();
            for (int index = 0; index < Connection.Processes.Length; index++)
            {
                Process process = Connection.Processes[index];
                var item = new ListViewItem();
                item.Text = process.ProcessName;
                item.ImageKey = process.Id.ToString();
                item.Tag = process.Id;
                WriteToGrid(process.Id);
            }
        }

        private void WriteToGrid(int procId)
        {
            int endPoints = 0;

            int tcpv4 = 0, udpv4 = 0, tcpv6 = 0, udpv6 = 0;
            int listening = 0;

            for (int index = 0; index < _allConnections.Count; index++)
            {
                Connection info = _allConnections[index];

                bool ok = false;

                switch (info.ConnectType)
                {
                    case ConnectType.Tcp:
                        if (checkBoxTcp.Checked && checkBoxV4.Checked) ok = true;
                        ++tcpv4;
                        break;

                    case ConnectType.Udp:
                        if (checkBoxUdp.Checked && checkBoxV4.Checked) ok = true;
                        ++udpv4;
                        break;

                    case ConnectType.TcPv6:
                        if (checkBoxTcp.Checked && checkBoxV6.Checked) ok = true;
                        ++tcpv6;
                        break;

                    case ConnectType.UdPv6:
                        if (checkBoxUdp.Checked && checkBoxV6.Checked) ok = true;
                        ++udpv6;
                        break;
                }

                if (info.State == TcpState.Listen)
                    ++listening;

                if (info.RemoteEndPoint.Address.Equals(IPAddress.IPv6Any)
                    || info.RemoteEndPoint.Address.Equals(IPAddress.Any))
                {
                    if (!CBoxunconEndPoints.Checked)
                        continue;
                }
                else
                    ++endPoints;

                if (procId >= 0 && procId != info.OwningPid)
                    ok = false;

                if (ok)
                {
                    dataGridView1.Rows.Add(info.OwningProcess, info.ConnectType, info.LocalEndPoint,
                                           info.RemoteEndPoint, info.State /*,info.DnsName*/);
                }
            }

            toolStripStatusLabel1.Text = string.Format("TCPv4 {0}", tcpv4);
            toolStripStatusLabel2.Text = string.Format("TCPv6 {0}", tcpv6);

            toolStripStatusLabel3.Text = string.Format("UDPv4 {0}", udpv4);
            toolStripStatusLabel4.Text = string.Format("UDPv6 {0}", udpv6);

            toolStripStatusLabel5.Text = string.Format("Listen {0}", listening);
            toolStripStatusLabel6.Text = string.Format("EndPoints {0}", endPoints);
        }

        #endregion Methods
    }
}