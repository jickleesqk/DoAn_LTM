using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.IO;

namespace Server.Network
{
	class ServerSocket
	{
		private Socket m_socket;
		private IPEndPoint m_server_ipep;
		private List<Socket> m_arraySockets = new List<Socket>();
		public int m_number_of_connections = 0;
		private byte[] m_raw_data = new byte[1024 * 5000];

		public ServerSocket()
		{
			m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

		public void Bind(int port)
		{
			string ip = GetLocalIP();
			m_server_ipep = new IPEndPoint(IPAddress.Parse(ip), port);
			m_socket.Bind(m_server_ipep);
		}

		public void Listen(int number)
		{
			m_socket.Listen(number);
		}

		private string GetLocalIP()
		{
			IPHostEntry myHost;
			myHost = Dns.GetHostEntry(Dns.GetHostName());
			foreach (IPAddress ip in myHost.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
					return ip.ToString();
			}
			return "127.0.0.1";
		}

		public IPEndPoint get_ipEndPoint()
		{
			return m_server_ipep;
		}

		public void Accept()
		{
			//Làm server bắt đầu nghe những liên kết đang kết nối đến
			m_socket.BeginAccept(new AsyncCallback( AcceptedCallBack ), null);
		}

		private void AcceptedCallBack(IAsyncResult ar)
		{
			//Được xuất bản
			//Bắt đầu nhận data từ client mới
			//Nghe kết nối mới
			Socket new_client = m_socket.EndAccept(ar);

			m_arraySockets.Add(new_client);

			Console.WriteLine("\nConnected to client");

			new_client.BeginReceive(m_raw_data, 0, m_raw_data.Length, SocketFlags.None, new AsyncCallback(ReceivedCallBack), new_client);

			Accept();
		}

		private void ReceivedCallBack(IAsyncResult ar)
		{
			Socket new_client = ar.AsyncState as Socket;

			try
			{
				int receiveLength = new_client.EndReceive(ar);

				if(receiveLength > 0)
				{
                    //Format packet: type - username.length - username
                    byte[] clientData = new byte[receiveLength];
					Array.Copy(m_raw_data, clientData, receiveLength);

					//Lấy mỗi thuộc tính gắn vào bên trong mảng
					//Ngoài ra còn lấy kích thước byte của các thuộc tính
					int leap = 0;

					int type = BitConverter.ToInt32(clientData, leap);
					leap += BitConverter.GetBytes(type).Length;

					int usernameLength = BitConverter.ToInt32(clientData, leap);
					leap += BitConverter.GetBytes(usernameLength).Length;

                    string user_name = Encoding.ASCII.GetString(clientData, leap, usernameLength);
                    leap += Encoding.ASCII.GetBytes(user_name).Length;
                    //Nhận TYPE - USERNAME.LENGTH - USER_NAME

                    Console.WriteLine("\nClient info : " + new_client.RemoteEndPoint.ToString());
                    Console.WriteLine("Total length : " + clientData.Length + " bytes");
                    Console.WriteLine("Username: " + user_name);
                    if (type == 1)
                    {
                        //Format: type - username.length - username - data
                        string user_message = Encoding.Unicode.GetString(clientData, leap, receiveLength - leap);

                        //Message
                        Console.WriteLine("User_message : " + user_message);
                    }
                    else
                    {
                        //Format: type - username.length - username - filename.length - filename - filedata
                        int filename_length = BitConverter.ToInt32(clientData, leap);
                        leap += BitConverter.GetBytes(filename_length).Length;

                        string filename = Encoding.ASCII.GetString(clientData, leap, filename_length);
                        leap += Encoding.ASCII.GetBytes(filename).Length;

                        Console.WriteLine("File name: " + filename);

                        //Viết tệp tin vào đường dẫn sau trong ổ cứng
                        BinaryWriter bWrite = new BinaryWriter(File.Open(@"C:\Users\Jick\Downloads\Wallpaper\" + filename, FileMode.Append));
                        bWrite.Write(clientData, leap, receiveLength - leap);
                        bWrite.Close();
                    }

                    //Gửi gói tin đến tất cả user 

                    //Gửi đến tất cả thuộc tính trong danh sách
                    sendToList(clientData, new_client);

					//Bắt đầu việc nhận trở lại
					new_client.BeginReceive(m_raw_data, 0, m_raw_data.Length, SocketFlags.None, new AsyncCallback(ReceivedCallBack), new_client);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		private void SentCallBack(IAsyncResult ar)
		{
			try
			{
				Socket client = (Socket)ar.AsyncState;

				int byteSent = client.EndSend(ar);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

		}

		public void sendToList(byte [] data, Socket previousClient)
		{
            //Gửi đến tất cả client trong list chat 
            try
            {
                foreach (Socket eachClient in m_arraySockets)
                {
                    if (eachClient.Connected != false)
                    {
                        if (eachClient != previousClient)
                            eachClient.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SentCallBack), eachClient);
                    }
                }

                Console.WriteLine("status: OK\n\n");
            }
            catch
            {
                Console.WriteLine("status: FAILED\n\n");
            }
			
		}
	}
}
