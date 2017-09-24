using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientCtrl
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            initSocket();
            addData();
        }
        public const int IP_PORT = 6666;
        private void initSocket()
        {
            this.textBox_host.Text = getIpAddress() + ":" + IP_PORT;



            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(IPAddress.Any, IP_PORT);
            server.Bind(point);
            server.Listen(999);
            Thread thread = new Thread(acceptClient);
            thread.IsBackground = true;
            thread.Start(server);
            listBox_log.Items.Add("=====init======");

        }

        private string getIpAddress()
        {
            IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            return ipAddr.ToString();
        }

        private void acceptClient(Object obj)
        {
            Socket serverSocket = obj as Socket;
            while (true)
            {
                Socket client = serverSocket.Accept();
                var sendIpoint = client.RemoteEndPoint.ToString();
                listBox_log.Items.Add("Accept======" + sendIpoint);
                Console.WriteLine($"{sendIpoint}Connection");
                //开启一个新线程不停接收消息
                Thread thread = new Thread(ReciveContent);
                thread.IsBackground = true;
                thread.Start(client);
            }

        }
        //接受客户端消息
        private void ReciveContent(Object obj)
        {
            Socket socketServer = obj as Socket;

            int index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = "第" + index + "条";
            this.dataGridView1.Rows[index].Cells[1].Value = "alis";
            IPEndPoint clientipe = (IPEndPoint)socketServer.RemoteEndPoint;
            this.dataGridView1.Rows[index].Cells[2].Value = "" + clientipe.Address.ToString();

            byte[] headerByte = new byte[3];
            socketServer.Receive(headerByte);
            String header = System.Text.UTF8Encoding.Default.GetString(headerByte);
            listBox_log.Items.Add("client:===header:"+ header);

            byte[] buffer = new byte[10 * 1024 * 1024];
            int length = 0;

            if ("str".Equals(header)) {
                StringBuilder sb = new StringBuilder();
                while ((length = socketServer.Receive(buffer)) > 0)
                {
                    sb.Append(UTF8Encoding.UTF8.GetChars(buffer));

                }
                ClientInfoBean clientInfo= AllUtils.jsonToObj(sb.ToString(), typeof(ClientInfoBean)) as ClientInfoBean;
                UIAppList.updataAppsData(this.dataGridView_apps, clientInfo.appList);
                UIAppList.updataPhoneInfo(this.listBox_phone_info, clientInfo);

                return;
            }

            MemoryStream ms = new MemoryStream();

            while ((length = socketServer.Receive(buffer)) > 0)
            {
                ms.Write(buffer, 0, buffer.Length);
            }

            //string words = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

            listBox_log.Items.Add("client:===img====");
            this.pictureBox1.Image = new Bitmap(ms);
            ms.Close();
          

        }

        private void addData()
        {


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
          //  MessageBox.Show("SelectionChanged:" + e);
        }
    }
}
