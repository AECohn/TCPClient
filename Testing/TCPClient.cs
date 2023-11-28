﻿using System;
using System.Net.Sockets;
using System.Timers;
using Timer = System.Timers.Timer;

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
        private Timer _timer;

        private ushort _port;

        private Byte[] _responseData = new Byte[65534];

        public event EventHandler<SendArgs> DataReceived, ConnectionStatus, Error;

        private SendArgs _responseArgs, _connectionArgs, _errorArgs;
        
        public void ConnectionPollStart(int milliseconds)
        {
            _timer = new Timer();
            _timer.Interval = milliseconds;
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }
        
        public void ConnectionPollStop()
        {
            _timer.Stop();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!IsConnected())
            {
                /*_disconnect();
                _connect();*/
                _connectionArgs = new SendArgs()
                {
                    Data = "Disconnected"
                };
            }
        }
        
        public bool IsConnected()
        {
            try
            {
                if (_tcpClient != null && _tcpClient.Client != null && _tcpClient.Client.Connected)
                {
                    if (_tcpClient.Client.Poll(0, SelectMode.SelectRead))
                    {
                        return !(_tcpClient.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Connects to the remote device via hostname and port fields
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        public void Connect(string hostname, ushort port)
        {
            _hostname = hostname;
            _port = port;
            _connect();
          
        }
        
        
        private void _connect()
        {
            try
            {
                if (_tcpClient != null)
                {
                    _disconnect();
                }

                _tcpClient = new TcpClient();
                _tcpClient.ConnectAsync(_hostname, _port).Wait(1000);
                _tcpStream = _tcpClient.GetStream();
                _tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
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
                _disconnect();
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
                _disconnect();
                _connect();
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
                _disconnect();
            }
        }

        private void EndReading(IAsyncResult readResult) //restarts itself when a message is received
        {
            try
            {
                _numberOfBytesRead = _tcpStream.EndRead(readResult);
                _responseArgs = new SendArgs()
                {
                    Data = System.Text.Encoding.GetEncoding("iso-8859-1").GetString(_responseData, 0, _numberOfBytesRead)
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
                _disconnect();
            }
        }

        public void Disconnect()
        {
            _hostname = null;
            _port = 0;
            _disconnect();
        }
        /// <summary>
        /// Disconnects from the remote device
        /// </summary>
        private void _disconnect()
        {
            try
            {
                _tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, false);
                _tcpClient.Close();
                _tcpStream.Close();
                _tcpClient.Dispose();
                _tcpStream.Dispose();
                
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