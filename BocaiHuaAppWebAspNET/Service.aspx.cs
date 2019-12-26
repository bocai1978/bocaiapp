using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BocaiHuaAppWebAspNET
{
    public partial class Service : System.Web.UI.Page
    {
        private const string LogFolder = @"c:\bocaihuaapp\logon\";
        private static object LogLocker = new object();
        protected void Page_Load(object sender, EventArgs e)
        {
            string sAction = "";
            if (this.Request.QueryString["action"] != null)
            {
                sAction = this.Request.QueryString["action"].ToString().ToLower();
            }
            else if (this.Request.Form["action"] != null)
            {
                sAction = this.Request.Form["action"].ToString().ToLower();
            }

            this.Response.Clear();
            string sResult = "{Action:\"" + sAction + "\"";
            if(sAction.Equals("login"))
            {
                string sDeviceID = "";
                if (this.Request.Form["DeviceID"] != null)
                {
                    sDeviceID = this.Request.Form["DeviceID"].ToString().ToLower();

                }else if (String.IsNullOrWhiteSpace(sDeviceID))
                {
                    sDeviceID = this.Request.QueryString["DeviceID"].ToString().ToLower();
                }
                if(sDeviceID.Length > 0)
                {
                    lock (LogLocker)
                    {
                        if (!System.IO.Directory.Exists(LogFolder))
                        {
                            System.IO.Directory.CreateDirectory(LogFolder);
                        }

                        System.IO.File.AppendAllText(LogFolder + DateTime.Now.ToString("yyyy-MM-dd") + ".log", DateTime.Now.ToString() + "\t" + sDeviceID +"\r\n");
                    }
                    sResult += ",Result =\"OK\"";
                }
                else
                {
                    sResult += ",Result =\"DeviceID is null\"";
                }
            }
            sResult += "}";
            this.Response.Write(sResult);
            this.Response.Flush();
            this.Response.End();
        }
    }
}