using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client;
using System.IO;

namespace Client.Network
{
   public class ClientSocket
    {
        private Socket          m_clientSocket;
        private byte[]          m_raw_data = new byte[1024*5000];
        private MainForm        m_clientForm;
        private string          m_username;

        public ClientSocket(MainForm clientForm)
        {
            m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_clientForm = clientForm;
        }

        public void Connect(string ip, int port, string username)
        {
            m_username = username;
            m_clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), ConnectedCallback, null);
        }

        private void ConnectedCallback(IAsyncResult ar)
        {
            if (m_clientSocket.Connected)
            {
                m_raw_data = new byte[1024];

                //byte [] usernameByte = Encoding.Unicode.GetBytes(m_username);
                //m_clientSocket.Send(usernameByte);

                //Nếu code không bị lỗi => chạy giao diện MainForm
                m_clientSocket.BeginReceive(m_raw_data, 0, m_raw_data.Length, SocketFlags.None, new AsyncCallback(ReceivedCallback), null);
            }
        }

        public void SendData(string client_message)
        {
            //1 - mesage
            //sending Encoding message
            //Type == Message: type - username.length - username - data

            byte[] dataByte = new byte[client_message.Length];

            dataByte = Encoding.Unicode.GetBytes(client_message);

            byte[] type = BitConverter.GetBytes(1);
            byte[] username = Encoding.ASCII.GetBytes(m_username);
            byte[] usernameLength = BitConverter.GetBytes(username.Length);
            byte[] received_data = new byte[type.Length + usernameLength.Length + username.Length + dataByte.Length];

            type.CopyTo(received_data, 0);
            usernameLength.CopyTo(received_data, type.Length);
            username.CopyTo(received_data, type.Length + usernameLength.Length);
            dataByte.CopyTo(received_data, type.Length + usernameLength.Length + username.Length);

            m_clientSocket.Send(received_data);
        }

        public void SendFile(string full_file_name)
        {
            //2 - files
            //Type == File: type - username.length - username - filename.length - filename - filedata 
            int packet_type = 2;
            string filePath = "";
            string fileName = full_file_name;

            fileName = fileName.Replace("\\", "/");

            while (fileName.IndexOf("/") > -1)
            {
                filePath += fileName.Substring(0, fileName.IndexOf("/") + 1);
                fileName = fileName.Substring(fileName.IndexOf("/") + 1);
            }

            byte[] type = BitConverter.GetBytes(packet_type);
            byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);
            //Read the file for data in bytes
            byte[] fileData = File.ReadAllBytes(filePath + fileName);
            byte[] username = Encoding.ASCII.GetBytes(m_username);
            byte[] usernameLength = BitConverter.GetBytes(username.Length);
            byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
            byte[] sendingData = new byte[type.Length + usernameLength.Length + username.Length + fileNameLen.Length + fileNameByte.Length + fileData.Length];

            type.CopyTo(sendingData, 0);
            usernameLength.CopyTo(sendingData, type.Length);
            username.CopyTo(sendingData, type.Length + usernameLength.Length);
            fileNameLen.CopyTo(sendingData, type.Length + usernameLength.Length + username.Length);
            fileNameByte.CopyTo(sendingData, type.Length + usernameLength.Length + username.Length + fileNameLen.Length);
            fileData.CopyTo(sendingData, type.Length + usernameLength.Length + username.Length + fileNameLen.Length + fileNameByte.Length);

            m_clientSocket.Send(sendingData);
        }

        private void ReceivedCallback(IAsyncResult ar)
        {
            int receiveLength = m_clientSocket.EndReceive(ar);

            try
            {
                //Process received_data
                /**********/
                string result_output = "";

                //Format packet: type - username.length - username
                byte[] clientData = new byte[receiveLength];
                Array.Copy(m_raw_data, clientData, receiveLength);

                //Get each of the element appened inside array
                //Not only get the element but also the size of bytes of the element
                int leap = 0;

                int type = BitConverter.ToInt32(clientData, leap);
                leap += BitConverter.GetBytes(type).Length;

                int usernameLength = BitConverter.ToInt32(clientData, leap);
                leap += BitConverter.GetBytes(usernameLength).Length;
                MessageBox.Show(usernameLength.ToString());

                string user_name = Encoding.ASCII.GetString(clientData, leap, usernameLength);
                leap += Encoding.ASCII.GetBytes(user_name).Length;
                //Get the TYPE - USERNAME.LENGTH - USER_NAME

                if (type == 1)
                {
                    //Format: type - username.length - username - data
                    string user_message = Encoding.Unicode.GetString(clientData, leap, receiveLength - leap);

                    result_output = user_name + ": " + user_message;
                }
                else
                {
                    //Format: type - username.length - username - filename.length - filename - filedata
                    int filename_length = BitConverter.ToInt32(clientData, leap);
                    leap += BitConverter.GetBytes(filename_length).Length;

                    string filename = Encoding.ASCII.GetString(clientData, leap, filename_length);
                    leap += Encoding.ASCII.GetBytes(filename).Length;

                    string filePath = @"C:\Users\Jick\Downloads\Wallpaper\";

                    //Write the file down to harddisk
                    FileStream fileStream = new FileStream(filePath + filename, FileMode.OpenOrCreate);

                    fileStream.Write(clientData, leap, receiveLength - leap);
                    fileStream.Close();
                    
                    //Client được nhận file sẽ thấy cửa sổ thông báo hỏi rằng client có muốn mở file được nhận hay không
                    DialogResult result = MessageBox.Show(user_name + "sent a file. Do you want to open it? ", "", MessageBoxButtons.YesNo);

                    if(result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(@"C:\Users\Jick\Downloads\Wallpaper\" + filename);
                    }

                    result_output = user_name + ": SENT A FILE";
                }

                m_clientForm.fetch_message(result_output);

                m_clientSocket.BeginReceive(m_raw_data, 0, m_raw_data.Length, SocketFlags.None, new AsyncCallback(ReceivedCallback), null);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        
        }
    }
}
