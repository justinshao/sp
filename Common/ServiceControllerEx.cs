using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Management;

namespace Common
{
    public class ServiceControllerEx : ServiceController
    {
        public ServiceControllerEx()
            : base()
        { }
        public ServiceControllerEx(string name)
            : base(name)
        { }
        public ServiceControllerEx(string name, string machineName)
            : base(name, machineName)
        { }

        public string Description
        {
            get
            {
                //construct the management path
                string path = "Win32_Service.Name='" + this.ServiceName + "'";
                ManagementPath p = new ManagementPath(path);
                //construct the management object
                ManagementObject ManagementObj = new ManagementObject(p);
                if (ManagementObj["Description"] != null)
                {
                    return ManagementObj["Description"].ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// This property returns the Service startup type,
        /// it can be one of these values
        /// Automatic , Manual and Disabled
        /// and accept the same values
        /// </summary>

        public string StartupType
        {
            get
            {
                if (this.ServiceName != null)
                {
                    //construct the management path
                    string path = "Win32_Service.Name='" + this.ServiceName + "'";
                    ManagementPath p = new ManagementPath(path);
                    //construct the management object
                    ManagementObject ManagementObj = new ManagementObject(p);
                    return ManagementObj["StartMode"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != "Automatic" && value != "Manual" && value != "Disabled")
                    throw new Exception("The valid values are Automatic, Manual or Disabled");

                if (this.ServiceName != null)
                {
                    //construct the management path
                    string path = "Win32_Service.Name='" + this.ServiceName + "'";
                    ManagementPath p = new ManagementPath(path);
                    //construct the management object
                    ManagementObject ManagementObj = new ManagementObject(p);
                    //we will use the invokeMethod method of the ManagementObject class
                    object[] parameters = new object[1];
                    parameters[0] = value;
                    ManagementObj.InvokeMethod("ChangeStartMode", parameters);
                }
            }
        }

    }
}
