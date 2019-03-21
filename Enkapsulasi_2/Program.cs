﻿using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApp2
{
    /*
     * Dalam CS-GO data yang dikirim adalah :
     * - ID player
     * - ID ModelCharacter
     * - Koordinat player
     * - Koordinat bullet
     * - ID Senjata yang digunakan
     * - Status dead/alive
     */

    [Serializable] struct Koor { public int X, Y; }

    [Serializable] struct ID { public string player, ModelCharacter, Senjata; }

    [Serializable]
    struct Data
    {
        public ID Id;
        public string Status_dead_alive;
        public Koor XYbullet, XYplayer;

        public void Show()
        {
            Console.WriteLine(Id.player + "\n" + Id.ModelCharacter + "\n" + Id.Senjata + "\n" + Status_dead_alive);
            Console.WriteLine(XYplayer.X + "," + XYplayer.Y);
            Console.WriteLine(XYbullet.X + "," + XYbullet.Y);
        }
    };

    /*Berguna untuk men-Serialize data yang akan dikirim, sehingga memoryStream dari objek dapat diambil
     dan diubah menjadi array of byte. Untuk men-Serialize, menggunakan BinaryFormatter.*/
    public class byteData
    {
        //menserialize objek, agar data dapat dikirim
        public static byte[] serializable(object O)
        {
            var memoryStream = new MemoryStream();
            (new BinaryFormatter()).Serialize(memoryStream, O);
            byte[] alpha = memoryStream.ToArray();
            return alpha;
        }

        //membalikkan objek yang terserialize
        public static object Deserializable(byte[] alpha, int dataLength)
        {
            using (var memoryStream = new MemoryStream(alpha, 0, dataLength))
            {
                return (Data)(new BinaryFormatter()).Deserialize(memoryStream);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string status = Console.ReadLine();

            if (status == "client") Client.Start();
            else Server.Start();
        }
    }

    class Server
    {
        const int PORT_NO = 8888;
       // const string SERVER_IP = "127.0.0.1";

        public static void Start()
        {
            Console.Clear();
            Console.WriteLine("Server");

            //---listen at the specified IP and port no.---
           // IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(IPAddress.Any, PORT_NO);
            Console.WriteLine("Listening...");
            listener.Start();

            //---incoming client connected---
            TcpClient client = listener.AcceptTcpClient();

            //---get the incoming data through a network stream--

            //---read incoming stream---
            using (NetworkStream nwStream = client.GetStream())
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int dataLength = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                //---convert the data received into a string---
                Data dataEnemy = (Data)byteData.Deserializable(buffer, dataLength);
                if(dataLength == null)Console.WriteLine("NULL");
                dataEnemy.Show();

                byte[] bytesToSend = byteData.serializable(dataEnemy);

                //---send the text---
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            }
            //---write back the text to the client---

           // nwStream.Write(buffer, 0, bytesRead);
            client.Close();
            listener.Stop();
            Console.ReadLine();
        }
    }

    class Client
    {
        const int PORT_NO = 8888;
        const string CLIENT_IP = "192.168.43.84";

        public static void Start()
        {
            Console.Clear();
            Console.WriteLine("Client");

            Data player = new Data();
            player.Id.player = "crowisover2";
            player.Id.ModelCharacter = "grunt1";
            player.Id.Senjata = "AK-56";
            player.XYplayer.X = 1;
            player.XYplayer.Y = 2;
            player.XYbullet.X = 1;
            player.XYbullet.Y = 2;
            player.Status_dead_alive = "dead";

            //---create a TCPClient object at the IP and port no.---
            TcpClient client = new TcpClient(CLIENT_IP, PORT_NO);
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToSend = byteData.serializable(player);

            //---send the text---
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            //---menerima data
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int dataLength = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

            //---convert the data received into a string---
            Data dataEnemy = (Data)byteData.Deserializable(buffer, dataLength);
            if (dataLength == null) Console.WriteLine("NULL");
            dataEnemy.Show();

            Console.ReadLine();
            client.Close();
        }
    }
    
}
