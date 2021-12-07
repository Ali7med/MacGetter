using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace MacGetter2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);
        public MainWindow()
        {
            InitializeComponent();
            txtIp.Text = GetLocalIPAddress();
            MenList.Items.Add(new IpMac() { IP = txtIp.Text, ID = 1, Mac = GetMacUsingARP(txtIp.Text) });

        }


        #region "get ip , mac"

        private string GetMacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                Console.Write(
                   "Found MAC Address: " + nic.GetPhysicalAddress() +
                   " Type: " + nic.NetworkInterfaceType);

                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !string.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    Console.Write("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
        }

        private string GetMacAddress2()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddresses;
        }


        private string GetMacUsingARP(string IPAddr)
        {
            IPAddress IP = IPAddress.Parse(IPAddr);
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;

            if (SendARP((int)IP.Address, 0, macAddr, ref macAddrLen) != 0)
                throw new Exception("ARP command failed");

            string[] str = new string[(int)macAddrLen];
            for (int i = 0; i < macAddrLen; i++)
                str[i] = macAddr[i].ToString("x2");

            return string.Join(":", str);
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        #endregion
        #region "btn"
        private void BtnGetMac_Click(object sender, RoutedEventArgs e)
        {
            if (txtIp.Text == "") return;
            txtMac.Text = GetMacUsingARP(txtIp.Text);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtMac.Text == "") return;
                string fileName = txtMac.Text.Replace(":", "_") + ".txt";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }    
                using (FileStream fs = File.Create(fileName))
                {  
                    Byte[] title = new UTF8Encoding(true).GetBytes(txtMac.Text);
                    fs.Write(title, 0, title.Length); 
                }
                MessageBox.Show("save Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GetLocalIPAddress());
            }
        }


        #endregion

        #region "get mac Network"
        private static List<Ping> pingers = new List<Ping>();
        private static int instances = 0;

        private static object @lock = new object();

        private static int result = 0;
        private static int timeOut = 250;

        private static int ttl = 5;
        private void BtnGetMacNetwork_Click(object sender, RoutedEventArgs e)
        {
            string baseIP = "192.168.0.";

            Console.WriteLine("Pinging 255 destinations of D-class in {0}*", baseIP);

            CreatePingers(255);

            PingOptions po = new PingOptions(ttl, true);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            byte[] data = enc.GetBytes("abababababababababababababababab");

            SpinWait wait = new SpinWait();
            int cnt = 1;

            Stopwatch watch = Stopwatch.StartNew();

            foreach (Ping p in pingers)
            {
                lock (@lock)
                {
                    instances += 1;
                }

                p.SendAsync(string.Concat(baseIP, cnt.ToString()), timeOut, data, po);
                cnt += 1;
            }

            while (instances > 0)
            {
                wait.SpinOnce();
            }

            watch.Stop();

            DestroyPingers();

            Console.WriteLine("Finished in {0}. Found {1} active IP-addresses.", watch.Elapsed.ToString(), result);
            Console.ReadKey();

        }
        public static void Ping_completed(object s, PingCompletedEventArgs e)
        {
            lock (@lock)
            {
                instances -= 1;
            }

            if (e.Reply.Status == IPStatus.Success)
            { 
                Console.WriteLine(string.Concat("Active IP: ", e.Reply.Address.ToString()));
                //MenList.Items.Add(new IpMac() { IP = MainWindow.txtIp.Text, ID = 1, Mac = GetMacUsingARP(txtIp.Text) });
                result += 1;
            }
            else
            {
                //Console.WriteLine(String.Concat("Non-active IP: ", e.Reply.Address.ToString()))
            }
        }


        private static void CreatePingers(int cnt)
        {
            for (int i = 1; i <= cnt; i++)
            {
                Ping p = new Ping();
                p.PingCompleted += Ping_completed;
                pingers.Add(p);
            }
        }

        private static void DestroyPingers()
        {
            foreach (Ping p in pingers)
            {
                p.PingCompleted -= Ping_completed;
                p.Dispose();
            }

            pingers.Clear();

        }
        #endregion
    }

    class IpMac
    {
        public int ID { get; set; }
        public string IP { get; set; }
        public string Mac { get; set; }
    }
}
