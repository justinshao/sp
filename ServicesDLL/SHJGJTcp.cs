using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Common;
using Common.Core;

namespace ServicesDLL
{
    public class SHJGJTcp
    { 
        /// <summary>
        /// 日志记录对象
        /// </summary>
        static Logger logger = LogManager.GetLogger("连接上海交管局");
      
        public static bool TcpSendData(byte[] bts, ref string resultdata)
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(SystemInfo.SHJGJIP);
                IPEndPoint point = new IPEndPoint(ip, int.Parse(SystemInfo.SHJGJPort));
                socket.SendTimeout = 20 * 1000;
                socket.ReceiveTimeout = 20 * 1000;
                socket.Connect(point);
                socket.Send(bts);
                byte[] buffer = new byte[1024 * 1024 * 3];//实际接收到的有效字节数
                int len = socket.Receive(buffer);
                resultdata = Encoding.UTF8.GetString(buffer, 5, len-5);
                socket.Close();
                return true;
            }
            catch(Exception ex)
            {
                logger.Fatal("连接交管局服务器失败" + ex.Message + "\r\n");
                return false;
            }
        }
    }
}
