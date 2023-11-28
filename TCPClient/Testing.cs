using System;
using System.Threading;
using NUnit.Framework;


namespace TCPClient
{
    [TestFixture]
    public class Testing
    {
        TcpConnection _tcpConnection = new TcpConnection();
        [Test]
        public void ConnectionTest()
        {
            _tcpConnection.Connect("127.0.0.1", 81);
            Thread.Sleep(100000);
            Assert.True(_tcpConnection.Client.IsConnected);
        }
        
        [Test]
        public void DisconnectionTest()
        {
            _tcpConnection.Connect("127.0.0.1", 81);
            Thread.Sleep(5000);
            _tcpConnection.Disconnect();
            Assert.False(_tcpConnection.Client.IsConnected);
            
        }
        [Test]
        public void SendTest()
        {
            _tcpConnection.Connect("127.0.0.1", 81);
            for (int i = 0; i < 100; i++)
            {
                _tcpConnection.Send(i.ToString()+'\n');
            }

            Assert.True(_tcpConnection.Client.IsConnected);
        }
        
        [Test]
        public void ReceiveTest()
        {
            _tcpConnection.Connect("127.0.0.1", 81);
            _tcpConnection.Send("Hello\n");
            Thread.Sleep(10000);
            Assert.That(_tcpConnection.Test, Is.EqualTo(8));
        }

        [Test]
        public void RapidConnectTest()
        {
            for (int i = 0; i < 20; i++)
            {
                _tcpConnection.Connect("127.0.0.1", 81);
                _tcpConnection.Disconnect();
            }
        }
        
        
}
}