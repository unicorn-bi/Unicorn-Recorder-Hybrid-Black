using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace UnicornRecorderUDPTrigger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Socket socket = null;
            IPEndPoint endPoint = null;
            bool senderThreadRunning = false;

            try
            {
                Console.WriteLine("IP (e.g. 127.0.0.1):\n--------------------");
                string ipString = Console.ReadLine();
                IPAddress ip = IPAddress.Parse(ipString);

                Console.WriteLine("Port (e.g. 1000):\n--------------------");
                string portString = Console.ReadLine();
                int port = int.Parse(portString);

                //Initialize socket
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.EnableBroadcast = true;
                endPoint = new IPEndPoint(ip, port);
                socket.Connect(endPoint);

                Console.WriteLine("Sending triggers.\nPress ENTER key to terminate the application.");

                senderThreadRunning = true;
                int i = 0;
                new Thread(() => 
                {
                    while (senderThreadRunning)
                    {
                        //Send trigger
                        string trigString = ((i % 10)+1).ToString();
                        i++;
                        byte[] sendBytes = Encoding.ASCII.GetBytes(trigString);
                        socket.SendTo(sendBytes, endPoint);
                        Console.WriteLine("Sent trigger with value {0}", trigString);
                        Thread.Sleep(1000);
                    }
                }).Start();

                Console.ReadLine();
                senderThreadRunning = false;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Failed to send triggers.\n{0}\n{1}\n", ex.Message, ex.StackTrace);
            }
        }
    }
}