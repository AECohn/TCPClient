using System;
using System.Net.Sockets;

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

        private Byte[] _responseData = new Byte[65534];

        public event EventHandler<SendArgs> DataReceived, ConnectionStatus, Error;

        private SendArgs _responseArgs, _connectionArgs, _errorArgs;

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

                _connectionArgs = new SendArgs()
                {
                    Data = "Connected"
                };

                ConnectionStatus?.Invoke(this, _connectionArgs);
            }
            catch
            {
                _errorArgs = new SendArgs()
                {
                    Data = "Exception in Connect"
                };
                Error?.Invoke(this, _errorArgs);
                Disconnect();
            }
        }

        /// <summary>
        /// Sends the message passed into the function to the remote device
        /// </summary>
        /// <param name="message"></param>
        public void Write(string message)
        {
            Byte[] sendData = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(message);

            try
            {
                _tcpStream.Write(sendData, 0, sendData.Length);
            }
            catch
            {
                _errorArgs = new SendArgs()
                {
                    Data = "Exception in Write"
                };
                Error?.Invoke(this, _errorArgs);
                Disconnect();
                Connect(_hostname, _port);
            }
        }


        private void BeginRead() //Recursively begins waiting for a message to arrive from the remote device
        {
            try
            {
                _tcpStream.BeginRead(_responseData, 0, _responseData.Length, EndReading, _tcpStream);
            }
            catch
            {
                _errorArgs = new SendArgs()
                {
                    Data = "Exception in BeginRead"
                };
                Error?.Invoke(this, _errorArgs);
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
                    Data = System.Text.Encoding.ASCII.GetString(_responseData, 0, _numberOfBytesRead)
                };

                DataReceived?.Invoke(this, _responseArgs);

                if (_numberOfBytesRead != 0)
                {
                    BeginRead();
                }
            }
            catch
            {
                _errorArgs = new SendArgs()
                {
                    Data = "Exception in EndRead"
                };
                Error?.Invoke(this, _errorArgs);
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
                _connectionArgs = new SendArgs()
                {
                    Data = "Could not Connect"
                };

                ConnectionStatus?.Invoke(this, _connectionArgs);
            }
            catch
            {
                _errorArgs = new SendArgs()
                {
                    Data = "Error Disconnecting"
                };
                Error?.Invoke(this, _errorArgs);
            }
        }
    }
}