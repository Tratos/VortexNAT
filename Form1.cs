using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Open.Nat;
using System.Threading;

namespace Vortex_Test_IP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Helper.box = rtb1;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string HostName = System.Net.Dns.GetHostName();
            IPHostEntry host = Dns.GetHostEntry(HostName);
            string IpAdresse = host.AddressList[0].ToString();

            Helper.Log(HostName);

            foreach (IPAddress address in host.AddressList)
            {
                Helper.Log($"    {address}");
            }

            Helper.Log("--------------");


        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            TestIP();



        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            TestMapping();
        }


        private async void TestIP()
        {
            try
            {
                var discoverer = new NatDiscoverer();
                var cts = new CancellationTokenSource(10000);
                var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);
                var externTest = await device.GetExternalIPAsync();
                string foo = externTest.ToString();

                Helper.Log(foo);
                Helper.Log("--------------");
            }
            catch (Exception Ex)
            {
                Helper.Log("Error: " + Ex);
            }


        }

        private async void TestMapping()
        {
           try
            {

                var discoverer = new NatDiscoverer();
                var device = await discoverer.DiscoverDeviceAsync();
                await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, 1601, 1701, "Open.Nat (Session lifetime)"));

                var mappings = await device.GetAllMappingsAsync();

                foreach (var mapping in mappings)
                {
                    Helper.Log($" {mapping}");
                }

                await device.DeletePortMapAsync(new Mapping(Protocol.Tcp, 1601, 1701));


            }
             catch (Exception Ex)
            {
                Helper.Log("Error: " + Ex);
            }

        }


    }
}
