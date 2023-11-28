using System;
using System.Text;
using SuperSimpleTcp;

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
        public int Test;
        public SimpleTcpClient Client;
        
        
        
        
        public void Connect(string address, int port)
        {
          Client = new SimpleTcpClient(address, port);
          Client.Connect();
          Client.Events.Connected += Connected;
          Client.Events.Disconnected += Disconnected;
          Client.Events.DataReceived += DataReceived;
            
        }
        
        public void Disconnect()
        {
            Client.Disconnect();
            Client.Events.Disconnected -= Disconnected;
            Client.Events.DataReceived -= DataReceived;
            Client.Events.Connected -= Connected;
        }
        
        public void Send(string data)
        {
            Client.Send(Encoding.GetEncoding("Latin1").GetBytes(data));
        }
        
        public TcpConnection()
        {
            
        }

        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
           Test = e.Data.Count;
        }

        private void Disconnected(object sender, ConnectionEventArgs e)
        {
            //
        }

        private void Connected(object sender, ConnectionEventArgs e)
        {
           //
        }
    }
}