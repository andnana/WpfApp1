using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class LoginInfo
    {
        private string ip;

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string port;

        public string Port
        {
            get { return port; }
            set { port = value; }
        }
        private string deviceNum;

        public string DeviceNum
        {
            get { return deviceNum; }
            set { deviceNum = value; }
        }

    }
}
