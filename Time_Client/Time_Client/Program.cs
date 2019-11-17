using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static int n; 
        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static void Main(string[] args)
        {
            Console.WriteLine("Номер клиетна:");
            n = int.Parse(Console.ReadLine());
            Console.Title = "Клиент:"+n;
            LoopConnect();
            SendLoop();
            Console.ReadKey();
        }

        private static void SendLoop()
        {
            Random rand = new Random();
            while (true)
            {
                Thread.Sleep(2700);
                try
                {
                    double d = rand.Next(20, 30) * 0.99;
                    byte[] buffer = Encoding.UTF8.GetBytes(d+" Клиент:"+n);
                    clientSocket.Send(buffer);

                    byte[] receivedBuf = new byte[1234];
                    int rec = clientSocket.Receive(receivedBuf);
                    byte[] data = new byte[rec];
                    Array.Copy(receivedBuf, data, rec);
                    Message("Получено: " + Encoding.UTF8.GetString(data));
                }
                catch (SocketException)
                {
                    Message("Сервер отключился");
                    break;
                }
            }
        }

        private static void LoopConnect()
        {
            int attempts = 0;
            while (!clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    clientSocket.Connect(IPAddress.Loopback, 8888);
                }
                catch (SocketException)
                {
                    Console.Clear();
                    Message("Попытка соединения: " + attempts.ToString());
                }
            }
            Console.Clear();
            Message("Подключено");
        }
        
        private static void Message(string str)
        {
            Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff ") + str);
        }
    }
}
