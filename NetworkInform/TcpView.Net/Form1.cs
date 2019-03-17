using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using MTorrent;
using System.ComponentModel;

namespace TcpView.Net
{
    public partial class Form1 : Form
    {
        private List<Connection> _allConnections = new List<Connection>();
        private int selectedProcessId = -1;

        public Form1()
        {
            InitializeComponent();

            UpdateInfo();
				//dataGridView1.DataSource = _allConnections;

          // ExecutionContext.
        }

        private void CheckBox3CheckedChanged(object sender, EventArgs e)
        {
            WriteToGrid(selectedProcessId);
        }

        private void Button1Click(object sender, EventArgs e)
        {
            UpdateInfo();
//             WriteToGrid(selectedProcessId);
            UpdateProcesses();
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

        private void ListView1SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer1.Interval = 1000;
           // UpdateProcesses();
          //  Connection.UpdateProcessList();
        //    WriteToGrid(selectedProcessId);
//             _allConnections = NetworkInformation.GetProcessTcpActivity(selectedProcessId);
//             dataGridView1.DataSource = _allConnections;
        }
    }
}