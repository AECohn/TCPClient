//From Aviv's Simpl#Pro template

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
        public string Data;
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
        public event EventHandler<SendArgs> DataReceived, ConnectionStatus;
        private double _queueSpeed;
        private SendArgs _responseArgs, _connectionArgs;

       

        public void EnableQueue(string queueRate)
        {
            _queueSpeed = double.Parse((queueRate));
            Message_Sender.Enabled = true;

            if (_queueSpeed > 0)
            {
                Message_Sender.Interval = _queueSpeed;
            }
            else
            {
                ErrorLog.Error("Queue Speed must be greater than 0");
            }
        }

        public void DisableQueue()
        {
            _queueSpeed = 0;
            Message_Sender.Enabled = false;

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
                    CrestronConsole.PrintLine($"Exception in Write");
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

                _connectionArgs = new SendArgs();
                _connectionArgs.Data = "Connected";
                ConnectionStatus?.Invoke(this, _connectionArgs);
            }
            catch
            {
                CrestronConsole.PrintLine($"Exception in Connect");
                Disconnect();
            }
        }

        public void Write(string message)
        {
            Byte[] sendData = System.Text.Encoding.ASCII.GetBytes(message);
            if (Message_Sender.Enabled)
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
                    CrestronConsole.PrintLine($"Exception in Write");
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
                CrestronConsole.PrintLine($"{e} Exception at BeginRead");
            }
        }

        private void EndReading(IAsyncResult readResult)
        {
            try
            {
                _numberOfBytesRead = _tcpStream.EndRead(readResult);
                _responseArgs = new SendArgs()
                {
                    Data = System.Text.Encoding.ASCII.GetString(ResponseData, 0, _numberOfBytesRead)
                };

                DataReceived?.Invoke(this, _responseArgs);

                if (_numberOfBytesRead != 0)
                {
                    BeginRead();
                }
            }
            catch
            {
                CrestronConsole.PrintLine($"Exception at EndRead");
                Disconnect();
            }
        }


        public void Disconnect()
        {
            try
            {
                _tcpClient.Close();
                _connectionArgs = new SendArgs();
                _connectionArgs.Data = "Could not Connect";
                ConnectionStatus?.Invoke(this, _connectionArgs);
            }
            catch
            {
                CrestronConsole.PrintLine("Error Disconnecting");
            }
        }
    }
}