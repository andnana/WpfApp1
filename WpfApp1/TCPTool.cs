using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace WpfApp1
{
    internal class TCPTool
    {
        #region 文件流传输 port：5001

        private static string IP = "114.116.47.165";

        //  private static string IP = "192.168.1.115";
        private static int port = 5001;

        private static TcpClient client;
        private static NetworkStream ns;
        private static byte[] buffer = new byte[1024];

        /// <summary>
        /// 向服务端发送图片
        /// </summary>
        /// <param name="imgURl">图片路径</param>
        public static void sendToServer(string imgURl, int device_num)
        {
            if (File.Exists(imgURl))
            {  //创建一个文件流打开图片
                FileStream fs = File.Open(imgURl, FileMode.Open);
                //声明一个byte数组接受图片byte信息
                byte[] fileBytes = new byte[fs.Length];
                using (fs)
                {
                    //将图片byte信息读入byte数组中
                    fs.Read(fileBytes, 0, fileBytes.Length);
                    fs.Close();
                }
                try
                {
                    IPAddress address = IPAddress.Parse(IP);
                    //创建TcpClient对象实现与服务器的连接
                    TcpClient client = new TcpClient();
                    //连接服务器
                    client.Connect(address, port);
                    using (client)
                    {
                        //连接完服务器后便在客户端和服务端之间产生一个流的通道
                        NetworkStream ns = client.GetStream();
                        //通过此通道将图片数据写入网络流，传向服务器端接收
                        ns.Write(fileBytes, 0, fileBytes.Length);

                        //从网络流中读取文件名
                        ns.Read(buffer, 0, buffer.Length);
                        MainWindow.real_PlayPOJOs[device_num].messagePOJO.imgpath = Tool.streamToString(buffer);
                        Console.WriteLine(Tool.streamToString(buffer));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "上传失败");
                    // Console.WriteLine(ex.Message);
                }
            }
        }

        #endregion 文件流传输 port：5001

        #region 信息传输 port：5000

        private static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static byte[] mesbuffer = new byte[1024];
        private static IPAddress iPAdress = new IPAddress(new byte[] { 114, 116, 47, 165 });
        private static int mesport = 5000;
        private static IPEndPoint iPEndPoint = new IPEndPoint(iPAdress, mesport);
        private static bool bo = true;

        public static void CreateClient(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                using (socket)
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(iPEndPoint);
                    //发送
                    for (int i = 0; i < MainWindow.real_PlayPOJOs.Count; i++)
                    {
                        MainWindow.real_PlayPOJOs[i].messagePOJO.deviceNum = MainWindow.real_PlayPOJOs[i].deviceNum;
                        socket.Send(Encoding.Default.GetBytes(MainWindow.real_PlayPOJOs[i].messagePOJO.plus()));
                        Thread.Sleep(200);
                    }
                    //接取
                    //Console.WriteLine("服务器:" + Encoding.Default.GetString(mesbuffer, 0, socket.Receive(mesbuffer)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /*
         *      发送和接取
               static void doMessage()
                {
                    try
                    {
                        //发送
                        socket.Send(Encoding.Default.GetBytes(MessagePOJO.plus()));

                        //接取
                        byte[] buffer = new byte[1024];
                        int length = socket.Receive(buffer);
                        string msg = Encoding.Default.GetString(buffer, 0, length);
                        Console.WriteLine("服务器:" + msg);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
        */

        #endregion 信息传输 port：5000
    }
}