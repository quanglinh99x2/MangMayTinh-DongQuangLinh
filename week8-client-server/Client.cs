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

namespace week8_client_server
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        IPEndPoint IP; /// 6789 là port để các máy nói chuyện với nhau
        Socket client;
        //Kết nối tới server
        void Connect()
        {

           IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6789); // IP chính là IP của server

        //    IP = new IPEndPoint(IPAddress.Any, 6789); // IP chính là IP của server
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP); /// ??????????
            try {
                client.Connect(IP); // kết nối tới IP của server
            } catch
            {
                MessageBox.Show("Không thể kết  nối tới server", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();
        }
        //Đóng kết nối
        void Close()
        {
            client.Close();
        }

        void Send()
        {
            if (txtMessage.Text != string.Empty)
            {
                client.Send(Serialize(txtMessage.Text));        
            }
        }
        //Nhận tin 
        void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000]; //1024 byte * 5000 ~ 5M
                    client.Receive(data);
                    string message = (string)Deserialize(data);
                    AddMessage(message); // push text lên listview 
                }
            }
            catch
            {
                Close();
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
            Send();
            AddMessage(txtMessage.Text);
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        /*Need
            * 1) SOCKET
            * 2) IP
            *
            */

        // tạo ip là thuộc tính điểm cuối .

    }
    }