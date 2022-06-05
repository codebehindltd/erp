using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InnBoardSDC.SDCTool
{
    
    public class udpHelp
    {
        /// <summary>
        /// Network service class for UDP transmission
        /// </summary>
        private UdpClient udpcSend = new UdpClient(0);
        /// <summary>
        /// Network service class for UDP reception
        /// </summary>
        private UdpClient udpcRecv = null;

        /// <summary>
        /// Thread: Continuously monitor UDP packets
        /// </summary>
        Thread thrRecv = null;

        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);
        private IPEndPoint remoteEP = null;
        //private IPEndPoint localEP = null;

        const uint IOC_IN = 0x80000000;
        static int IOC_VENDOR = 0x18000000;
        int SIO_UDP_CONNRESET = (int)(IOC_IN | IOC_VENDOR | 12);

        public class UdpState
        {
            public UdpClient udpClient = null;
            public IPEndPoint ipEndPoint = null;
            public const int BufferSize = 2048;
            public byte[] buffer = new byte[BufferSize];
            public int counter = 0;
        }

        private UdpState udpReceiveState = null;
        public delegate int SomeFunWay(int iRet, string strRt);

        public SomeFunWay recvCB;

        public int sendMsg(string msg, int iPort, SomeFunWay sendCallBack)
        {
            try
            {
                recvCB = sendCallBack;
                udpcSend = new UdpClient(0);
                udpcSend.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
                byte[] sendbytes = Encoding.UTF8.GetBytes(msg);
                remoteEP = new IPEndPoint(IPAddress.Parse("255.255.255.255"), iPort); // IP address and port number sent to
                int iRet = udpcSend.Available;
                udpcSend.Send(sendbytes, sendbytes.Length, remoteEP);
                udpReceiveState = new UdpState();
                udpReceiveState.ipEndPoint = remoteEP;
                udpReceiveState.udpClient = udpcSend;

                ReceiveMessages(udpReceiveState);
                //udpcSend.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }

        public void ReceiveMessages(UdpState udpReceiveState)
        {
            lock (this)
            {
                udpcSend.BeginReceive(new AsyncCallback(ReceiveCallback), udpReceiveState);
                receiveDone.WaitOne();
                Thread.Sleep(100);
            }
        }

        public void ReceiveCallback(IAsyncResult iar)
        {
            UdpState udpState = iar.AsyncState as UdpState;
            if (!iar.IsCompleted)
            {
                return;
            }
            try
            {
                Byte[] receiveBytes = udpState.udpClient.EndReceive(iar, ref udpReceiveState.ipEndPoint);
                string receiveString = Encoding.UTF8.GetString(receiveBytes);
                //Console.WriteLine("Received: {0}", receiveString);
                recvCB(0, receiveString);
                udpState.udpClient.Close();
                receiveDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public int startListen(int iPort, SomeFunWay sendCallBack)
        {
            recvCB = sendCallBack;
            IPEndPoint localIpep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), iPort); // Local IP and listening port number
            udpcRecv = new UdpClient(localIpep);
            thrRecv = new Thread(ReceiveMessage);
            thrRecv.Start();
            return 0;
        }

        public int stopListen()
        {
            thrRecv.Abort(); // This thread must be closed first, otherwise it will be abnormal
            udpcRecv.Close();
            return 0;
        }

        private string GetIpAddress()
        {
            string hostName = Dns.GetHostName();   //Get local name
            IPHostEntry localhost = Dns.GetHostEntry(hostName);    //The method has expired, you can get an IPv4 address
            //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //Get IPv6 address
            IPAddress localaddr = localhost.AddressList[0];

            return localaddr.ToString();
        }

        private void ReceiveMessage(object obj)
        {
            IPEndPoint remoteIpep = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    byte[] bytRecv = udpcRecv.Receive(ref remoteIpep);
                    string message = Encoding.Unicode.GetString(bytRecv, 0, bytRecv.Length);
                    recvCB(1, "recv: " + message);
                }
                catch (Exception ex)
                {
                    recvCB(1, ex.Message);
                    break;
                }
            }
        }

        public class paramSend
        {
            public string msg;
            public int port;
        }
    }
}
