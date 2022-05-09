//From Aviv's Simpl#Pro template

using System;
using System.Net.Sockets;
using Crestron.SimplSharp;

namespace TCPClient
{
    public class SendArgs : EventArgs
    {
        /// <summary>
        /// class containing the data that will be sent when an event triggers
        /// </summary>
        public string Data;
    }

    public class TcpConnection
    {
        /// <summary>
        /// Class containing TCP connection logic
        /// </summary>
        private int _numberOfBytesRead;

        private TcpClient _tcpClient;
        private NetworkStream _tcpStream;
        private string _hostname;

        private ushort _port;

        private Byte[] ResponseData = new Byte[65534];

        public event EventHandler<SendArgs> DataReceived, ConnectionStatus;

        private SendArgs _responseArgs, _connectionArgs;

        /// <summary>
        /// Connects to the remote device via hostname and port fields
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
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

        /// <summary>
        /// Sends the message passed into the function to the remote device
        /// </summary>
        /// <param name="message"></param>
        public void Write(string message)
        {
            Byte[] sendData = System.Text.Encoding.ASCII.GetBytes(message);

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


        private void BeginRead() //Recursively begins waiting for a message to arrive from the remote device
        {
            try
            {
                _tcpStream.BeginRead(ResponseData, 0, ResponseData.Length, EndReading, _tcpStream);
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine($"{e} Exception at BeginRead");
                Disconnect();
            }
        }

        private void EndReading(IAsyncResult readResult) //restarts itself when a message is received
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

        /// <summary>
        /// Disconnects from the remote device
        /// </summary>
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