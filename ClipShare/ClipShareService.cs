using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClipShare
{
    public class ClipShareService
    {
        private enum MessageType : byte
        {
            Search = 0, Text = 1, Close = 255
        }

        private const int SERVER_PORT = 49001;
        private readonly Encoding ENCODING = Encoding.UTF8;

        private IPAddress _remoteIP;

        public ClipShareService()
        {

        }

        public IPAddress FindAndSetRemoteIP()
        {
            using (UdpClient client = new UdpClient(SERVER_PORT + 1))
            {
                byte[] searchData = new byte[] { (byte)MessageType.Search, 12, 7 };
                int sentBytesNumber = client.Send(searchData, searchData.Length, new IPEndPoint(IPAddress.Broadcast, SERVER_PORT));

                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref endPoint);

                IPAddress address = endPoint.Address;

                _remoteIP = address;
            }

            return _remoteIP;
        }

        public void SendText(string text)
        {
            if (_remoteIP == null)
            {
                throw new NullReferenceException("Remote IP not set!");
            }

            int textOffset = 3;
            byte[] textData = ENCODING.GetBytes(text);
            byte[] sendData = new byte[textData.Length + textOffset];
            sendData[0] = (byte)MessageType.Text;
            sendData[1] = (byte)(textData.Length / byte.MaxValue);
            sendData[2] = (byte)(textData.Length % byte.MaxValue);
            Array.Copy(textData, 0, sendData, textOffset, textData.Length);

            using (UdpClient client = new UdpClient())
            {
                client.Send(sendData, sendData.Length, _remoteIP.ToString(), SERVER_PORT);
            }
        }

        public void Close()
        {
            if (_remoteIP == null)
            {
                throw new NullReferenceException("Remote IP not set!");
            }

            byte[] sendData = new byte[1];
            sendData[0] = (byte)MessageType.Close;

            using (UdpClient client = new UdpClient())
            {
                client.Send(sendData, sendData.Length, _remoteIP.ToString(), SERVER_PORT);
            }
        }
    }
}
