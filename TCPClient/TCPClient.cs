﻿//From Aviv's Simpl#Pro template

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Timers;
using Crestron.SimplSharp;
using Timer = System.Timers.Timer;

namespace TCPClient
{
    public class SendArgs : EventArgs
    {
        public SimplSharpString Response;
    }

    public class TcpConnection
    {
        private int _numberOfBytesRead;
        private TcpClient _tcpClient;
        private NetworkStream _tcpStream;
        private string _hostname;
        private ushort _port;
        private Timer Message_Sender = new Timer();
        private Byte[] ResponseData = new Byte[65534];
        private Queue<Byte[]> writeQueue;
        public event EventHandler<SendArgs> DataReceived;
        private bool _queueMessages;
        public double QueueSpeed;

        public bool QueueMessages
        {
            get { return _queueMessages; }

            set
            {
                if (QueueSpeed > 0)
                {
                    Message_Sender.Interval = QueueSpeed;
                }
                else
                {
                    CrestronConsole.PrintLine(("Queue Speed must be greater than 0"));
                }

                _queueMessages = value;
                Message_Sender.Enabled = _queueMessages;
            }
        }


        public TcpConnection()
        {
            writeQueue = new Queue<byte[]>();
            Message_Sender.Elapsed += Message_SenderOnElapsed;
            Message_Sender.AutoReset = true;
        }


        private void Message_SenderOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (writeQueue.Count > 0)
            {
                try
                {
                    byte[] send = writeQueue.Dequeue();
                    _tcpStream.Write(send, 0, send.Length);
                }
                catch
                {
                    Console.WriteLine($"Exception in Write");
                    Disconnect();
                    Connect(_hostname, _port);
                }
            }
        }


        public void Connect(string hostname, ushort port)
        {
            try
            {
                _tcpClient = new TcpClient();
                _hostname = hostname;
                _port = port;
                _tcpClient.ConnectAsync(hostname, port).Wait(1000);
                _tcpStream = _tcpClient.GetStream();
                BeginRead();

            }
            catch
            {
                Console.WriteLine($"Exception in Connect");
                Disconnect();
            }
        }

        public void Write(string message)
        {
            Byte[] sendData = System.Text.Encoding.ASCII.GetBytes(message);
            if (_queueMessages)
            {
                writeQueue.Enqueue(sendData);
            }
            else
            {
                try
                {
                    _tcpStream.Write(sendData, 0, sendData.Length);
                }
                catch
                {
                    Console.WriteLine($"Exception in Write");
                    Disconnect();
                    Connect(_hostname, _port);
                }
            }
        }

        private void BeginRead()
        {
            try
            {
                _tcpStream.BeginRead(ResponseData, 0, ResponseData.Length, EndReading, _tcpStream);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e} Exception at BeginRead");
            }
        }

        private void EndReading(IAsyncResult readResult)
        {
            try
            {
                _numberOfBytesRead = _tcpStream.EndRead(readResult);
                SendArgs responseArgs = new SendArgs()
                {
                    Response = System.Text.Encoding.ASCII.GetString(ResponseData, 0, _numberOfBytesRead)
                };

                DataReceived?.Invoke(this, responseArgs);

                if (_numberOfBytesRead != 0)
                {
                    BeginRead();
                }
            }
            catch
            {
                Console.WriteLine($"Exception at EndRead");
            }
        }


        public void Disconnect()
        {
            try
            {
                _tcpClient.Close();
            }
            catch
            {
                Console.WriteLine("Error Disconnecting");
            }
        }
    }
}