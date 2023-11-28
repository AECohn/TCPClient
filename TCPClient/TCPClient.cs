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
        public string EncodingType = "Latin1";
        public SimpleTcpClient Client;
        public event EventHandler<SendArgs> ReceivedData;
        public event EventHandler<SendArgs> ConnectionStatus;
        
        
        
        
        public void Connect(string address, int port)
        {
          Client = new SimpleTcpClient(address, port);
          Client.Connect();
          Client.Keepalive.EnableTcpKeepAlives = true;
          Client.Keepalive.TcpKeepAliveTime = 5;
          Client.Keepalive.TcpKeepAliveInterval = 5;
          Client.Events.Connected += Connected;
          Client.Events.Disconnected += Disconnected;
          Client.Events.DataReceived += DataReceived;
            
        }
        
        public void Disconnect()
        {
            Client.Disconnect();
            Client.Keepalive.EnableTcpKeepAlives = false;
            Client.Events.Disconnected -= Disconnected;
            Client.Events.DataReceived -= DataReceived;
            Client.Events.Connected -= Connected;
        }
        
        public void Send(string data)
        {
            Client.Send(Encoding.GetEncoding(EncodingType).GetBytes(data));
        }

        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
           Test = e.Data.Count;
           ReceivedData?.Invoke(this, new SendArgs{Data = Encoding.GetEncoding(EncodingType).GetString(e.Data.Array ?? throw new InvalidOperationException())});
        }

        private void Disconnected(object sender, ConnectionEventArgs e)
        {
            ConnectionStatus?.Invoke(this, new SendArgs{Data = "Disconnected"});
        }

        private void Connected(object sender, ConnectionEventArgs e)
        {
            ConnectionStatus?.Invoke(this, new SendArgs{Data = "Connected"});
        }
    }
}