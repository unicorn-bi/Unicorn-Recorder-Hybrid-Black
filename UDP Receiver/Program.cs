using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace UnicornRecorderUDPReceiver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Unicorn Recorder UDP Receiver Example");
            Console.WriteLine("----------------------------");
            Console.WriteLine();
            try
            {
                //define an IP endpoint
                Console.Write("Destination port: ");
                int port = Convert.ToInt32(Console.ReadLine());
                IPAddress ip = IPAddress.Any;
                IPEndPoint endPoint = new IPEndPoint(ip, port);
                Console.WriteLine("Listening on port '{0}'...", port);

                //initialize upd socket
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.Bind(endPoint);
                byte[] receiveBufferByte = new byte[1024];
                float[] receiveBufferFloat = new float[receiveBufferByte.Length / sizeof(float)];

                //acquisition loop
                while (true)
                {
                    int numberOfBytesReceived = socket.Receive(receiveBufferByte);
                    if (numberOfBytesReceived > 0)
                    {
                        Console.Write(Encoding.ASCII.GetString(receiveBufferByte));
                        Array.Clear(receiveBufferByte, 0, receiveBufferByte.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine("Press ENTER to terminate the application.");
                Console.ReadLine();
            }
        }
    }
}
