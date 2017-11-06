using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.Network;
using System.Threading;
using System.IO;

namespace Client
{
    public partial class MainForm : Form
    {
        public ClientSocket m_clientSocket;
        public Thread checking_incomming_message;

        public MainForm()
        {   
            InitializeComponent();

            client_ip_tb.Text = "192.168.10.1";
            client_port_tb.Text = "1234";
        }

        public void fetch_message(string data)
        {
            client_conversation_Lb.Items.Add(data);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            client_sendBtn.Enabled = false;
            client_selectFile_btn.Enabled = false;

            m_clientSocket = new ClientSocket(this);
            //checking_incomming_message = new Thread(checking_incomming_message_CC);
            //checking_incomming_message.Start();
            //init - bind - listen - accept
            //beginaccept - endaccept
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Khi người dùng nhấn vào biểu tượng X để tắt App
            //Nó sẽ tắt tất cả các hộp thoải ẩn và hộp thoại đang hiển thị của người dùng đó
            //checking_incomming_message.Abort();
            Application.Exit();
        }

        private void client_sendBtn_Click(object sender, EventArgs e)
        {
            string client_message = client_mb.Text.Trim();

            if (client_message != "")
            {
                //sending Encoding message
                //byte[] data = new byte[1024]; 

                //data = Encoding.Unicode.GetBytes(client_message);
                m_clientSocket.SendData(client_message);

                //adding to listbox
                client_conversation_Lb.Items.Add("ME : " + client_message);
                client_mb.Text = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
             //Khi client nhấn vào nút để liên kết
            //kiểm tra input của 2 textbox
            //Nếu phù hợp => chuyển đến MainForm
            //Nếu không => hiển thị hộp thư thông báo
            string ip_tb_string = client_ip_tb.Text.Replace(" ","");
            string port_number_string = client_port_tb.Text.Replace(" ", "");
            string username = client_name_tb.Text.Trim();

            if (ip_tb_string != "" && port_number_string != "" && username != "")
            {
                //Nếu chuỗi không bị trống => gọi hàm liên kết
                Int32 port_number = Int32.Parse((port_number_string));
                connect_to_server(ip_tb_string, port_number, username);
                //MessageBox.Show(ip_tb_string + port_number_string);
            }
        }

        private void connect_to_server(string ip_tb_string, int port_number, string username)
        {
            m_clientSocket.Connect(ip_tb_string, port_number, username);
            client_connect_btn.Enabled = false;
            client_sendBtn.Enabled = true;

            client_ip_tb.ReadOnly = true;
            client_port_tb.ReadOnly = true;
            client_name_tb.ReadOnly = true;
            client_selectFile_btn.Enabled = true;
        }

        private void choose_file_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            // Hiển thị hộp thoại và nhận kết quả.
            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK) //Kiểm tra kết quả.
            {
                string fileName = ofd.FileName;

                long length = new System.IO.FileInfo(fileName).Length;

                if (length > 8 * 1048576) // Kiểm tra xem file up có lớn hơn 8MB không
                {
                    MessageBox.Show("Kích thước file lớn hơn 8MB, hãy gửi file có kích thước nhỏ hơn.");
                    return;
                }

                m_clientSocket.SendFile(fileName);
                client_conversation_Lb.Items.Add("ME : SENT A FILE");
            }
        }

        private void client_files_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void client_conversation_Lb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
