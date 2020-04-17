using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }
        IPEndPoint IP; /// 6789 là port để các máy nói chuyện với nhau
        Socket server;
        List<Socket> clientList;
        //Kết nối tới server
        void Connect()
        {
            clientList = new List<Socket>();
            
            IP = new IPEndPoint(IPAddress.Any, 6789); // 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP); /// ??????????

            server.Bind(IP); //Đợi bất cứ thằng nào
       

            Thread Listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(100); // đợi cho phép 100 đứa trong hàng đợi
                        Socket client = server.Accept(); // accept 
                        string IP_client = client.RemoteEndPoint.ToString();
                        clientList.Add(client);
                        client.Send(Serialize("Hello"+IP_client)); 

                         Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(client);
                    }
                }
                catch
                {
                    IP = new IPEndPoint(IPAddress.Any, 6789); // IP chính là IP của server
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP); /// ??????????
                }
            });
            Listen.IsBackground = true;
            Listen.Start();
        }
        //Đóng kết nối
        void Close()
        {
            server.Close();
        }

        void Send(Socket client)
        {
            if (txtMessage.Text != string.Empty)
            {
                client.Send(Serialize("server : "+txtMessage.Text));
            }
        }
        //Nhận tin 
        void Receive(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000]; //1024 byte * 5000 ~ 5M
                    client.Receive(data);
                    string message = (string)Deserialize(data);
                    string cl_message = "";
                    if (message == "1")
                    {
                        cl_message = "one";
                    }
                    if (message == "2")
                    {
                        cl_message = "two";
                    }
                    if (message == "3")
                    {
                        cl_message = "three";
                    }
                    if (message == "4")
                    {
                        cl_message = "four";
                    }
                    if (message == "5")
                    {
                        cl_message = "five";
                    }
                    if (message == "6")
                    {
                        cl_message = "six";
                    }
                    if (message == "7")
                    {
                        cl_message = "seven";
                    }
                    if (message == "8")
                    {
                        cl_message = "eight";
                    }
                    if (message == "9")
                    {
                        cl_message = "nine";
                    }
                    if (message == "END")
                    {
                        client.Send(Serialize("server :"+ "GOOD BYE!"));
                        AddMessage("GOOD BYE!");
                        clientList.Remove(client);
                        client.Close();
                    }
                    if (message == "1" || message == "2" || message == "3" || message == "4" || message == "5" || message == "6" || message == "7" || message == "8" || message == "9")
                    {
                        client.Send(Serialize("server : "+cl_message)); // 
                        AddMessage(message);
                    }
                    else
                    {
                        AddMessage(message); // push text lên listview 
                    }
                }
            }
            catch
            {
                clientList.Remove(client);
                client.Close();
            }


        }

        void AddMessage(string s)
        {
            lsvMessage.Items.Add(new ListViewItem() { Text = s });
            txtMessage.Clear();
        }
        //Phân mảnh để gửi 
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, obj);// Phân tách obj thành các mảnh

            return stream.ToArray(); // Mảng byte 0,1
        }
        //Gộp mảnh đối tượng đã phân mảnh
        object Deserialize(byte[] data)
        {
            {
                MemoryStream stream = new MemoryStream(data);
                BinaryFormatter formatter = new BinaryFormatter();

                return formatter.Deserialize(stream);//           
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            foreach (Socket item in clientList)
            {
                Send(item);
            }
            AddMessage(txtMessage.Text);
            txtMessage.Clear();
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void lsvMessage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
