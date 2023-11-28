using System;
using System.Net.Sockets;
using System.Timers;
using SuperSimpleTcp;
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
        public SuperSimpleTcp.SimpleTcpClient Client;

        public TcpConnection()
        {
            Client.Events.Connected += Connected;
            Client.Events.Disconnected += Disconnected;
            Client.Events.DataReceived += DataReceived;
        }

        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Disconnected(object sender, ConnectionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Connected(object sender, ConnectionEventArgs e)
        {
            throw new NotImplementedException();
        }


        public void Connect(string ip, int port)
        {
            Client = new SuperSimpleTcp.SimpleTcpClient(ip, port);

            Client.Connect();
        }
    }
}