using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;
using System.Windows.Forms;
using System.Text;

namespace WpfApp1
{
    internal class TcpServer
    {
        public Socket ServerSocket { get; set; }
        public Dictionary<string, Socket> Sockets { get; set; } = new Dictionary<string, Socket>();
        public byte[] SendBuffer { get; set; }
        public byte[] ReceiveBuffer { get; set; }

        /// <summary>
        /// 创建构造函数
        /// </summary>
        /// <param name="sendBufferLength">发送帧长度限制</param>
        /// <param name="receiveBufferLength">接收帧长度限制</param>
        public TcpServer(int sendBufferLength, int receiveBufferLength)
        {
            SendBuffer = new byte[sendBufferLength];
            ReceiveBuffer = new byte[receiveBufferLength];
        }

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        public string GetIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting local IP: " + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Start(string ip, string port)
        {
            MessageBox.Show(ip + ":" + port);
            ServerSocket = CreateSocket(ip, port);
            CreateThread();
        }

        /// <summary>
        /// 服务端向指定客户端发送消息
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="data"></param>
        public void Send(string clientInfo, byte[] data)
        {
            Socket socket = Sockets[clientInfo];
            socket.Send(data);
        }

        /// <summary>
        /// 创建服务端Socket
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private Socket CreateSocket(string ip, string port)
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress address = IPAddress.Parse(ip);
                IPEndPoint endPoint = new IPEndPoint(address, int.Parse(port));
                socket.Bind(endPoint);
                socket.Listen(50);
                return socket;
            }
            catch (System.Net.Sockets.SocketException e)
            {
                Console.WriteLine("socketException2");
                Console.WriteLine(e);
                Console.WriteLine("ErrorCode");
                Console.WriteLine(e.ErrorCode);
                throw;
            }


        }

        /// <summary>
        /// 创建监听客户端的线程并启动
        /// </summary>
        private void CreateThread()
        {
            Thread watchThread = new Thread(WatchConnection);
            watchThread.IsBackground = true;
            watchThread.Start();
        }

        /// <summary>
        /// 监听客户端请求
        /// </summary>
        private void WatchConnection()
        {
            while (true)
            {
                Socket clientSocket = null;
                try
                {
                    clientSocket = ServerSocket.Accept();
                    IPEndPoint endPoint = (IPEndPoint)clientSocket.RemoteEndPoint;
                    string ip = endPoint.Address.ToString();
                    string port = endPoint.Port.ToString();
                    Sockets.Add(ip + ":" + port, clientSocket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
                ParameterizedThreadStart pts = new ParameterizedThreadStart(ClientReceiver);
                Thread clientThread = new Thread(pts);
                clientThread.IsBackground = true;
                clientThread.Start(clientSocket);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="socket"></param>
        private void ClientReceiver(object socket)
        {
            Socket clientSocket = socket as Socket;
            int ReceiveBufferSize = 12;
            while (true)
            {
                byte[] buffer = new byte[ReceiveBufferSize];
                try
                {
                    if (clientSocket != null)
                    {
                        if (clientSocket.Receive(buffer) > 0)
                        {
                            // 打印接到的数据
                            Console.WriteLine(Encoding.Default.GetString(buffer));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
            }
        }
    }
}