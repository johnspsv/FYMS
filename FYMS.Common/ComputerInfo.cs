using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Instrumentation;
using System.Management;
using System.Net;
using System.IO;

namespace FYMS.Common
{
    public class ComputerInfo
    {
        public static string CpuID; //1.cpu序列号
        public static string MacAddress; //2.mac序列号
        public static string DiskID; //3.硬盘id
        public static string IpAddress; //4.ip地址
        public static string LoginUserName; //5.登录用户名
        public static string ComputerName; //6.计算机名
        public static string SystemType; //7.系统类型
        public static string TotalPhysicalMemory; //8.内存量 单位：M

        static ComputerInfo()
        {
            CpuID = GetCpuID();
            MacAddress = GetMacAddress();
            DiskID = GetDiskID();
            IpAddress = GetIPAddress();
            LoginUserName = GetUserName();
            SystemType = GetSystemType();
            TotalPhysicalMemory = GetTotalPhysicalMemory();
            ComputerName = GetComputerName();
        }

        //1.获取CPU序列号代码 
        static string GetCpuID()
        {
            try
            {

                string cpuInfo = "";//cpu序列号 

                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //2.获取网卡硬件地址 
        static string GetMacAddress()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //3.获取硬盘ID 
        static string GetDiskID()
        {
            try
            {
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return HDid;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //4.获取IP地址 
        static string GetIPAddress()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        //st=mo["IpAddress"].ToString(); 
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        /// 5.操作系统的登录用户名 
        static string GetUserName()
        {
            try
            {
                string un = "";

                un = Environment.UserName;
                return un;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //6.获取计算机名
        static string GetComputerName()
        {
            try
            {
                return System.Environment.MachineName;

            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        ///7 PC类型 
        static string GetSystemType()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    st = mo["SystemType"].ToString();

                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        ///8.物理内存        
        static string GetTotalPhysicalMemory()
        {
            try
            {

                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    st = mo["TotalPhysicalMemory"].ToString();

                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
    }



    public class Router
    {
        Encoding gb2312 = Encoding.GetEncoding(936);//路由器的web管理系统默认编码为gb2312
        /// <summary>
        /// 使用HttpWebRequest对象发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding">编码</param>
        /// <param name="cache">凭证</param>
        /// <returns></returns>
        private static string SendRequest(string url, Encoding encoding, CredentialCache cache)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            if (cache != null)
            {
                request.PreAuthenticate = true;
                request.Credentials = cache;
            }
            string html = null;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader srd = new StreamReader(response.GetResponseStream(), encoding);
                html = srd.ReadToEnd();
                srd.Close();
                response.Close();
            }
            catch (Exception ex) { html = "FALSE" + ex.Message; }
            return html;
        }
        /// <summary>
        /// 获取路由MAC和外网IP地址
        /// </summary>
        /// <param name="RouterIP">路由IP地址，就是网关地址了，默认192.168.1.1</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Passowrd">密码</param>
        /// <returns></returns>
        private string LoadMACWanIP(string RouterIP, string UserName, string Passowrd)
        {
            CredentialCache cache = new CredentialCache();
            string url = "http://" + RouterIP + "/userRpm/StatusRpm.htm";
            cache.Add(new Uri(url), "Basic", new NetworkCredential(UserName, Passowrd));
            return SendRequest(url, gb2312, cache);
        }
    }
}
