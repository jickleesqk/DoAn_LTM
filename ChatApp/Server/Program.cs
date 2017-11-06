using Server.Network;
using System;


namespace Server
{
    class Program
    {
        public static ServerSocket server_socket = new ServerSocket();
        static void Main(string[] args)
        {
            server_socket.Bind(1234);
            server_socket.Listen(5);
            server_socket.Accept();

            string server_info = server_socket.get_ipEndPoint().ToString();
            Console.Write("Server's information: " + server_info);

            while (true)
            {

            }
        }

    }
}
