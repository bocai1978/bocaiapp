using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace BocaiHuaAppWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BocaiHuaAppController : ControllerBase
    {
        private const string LogFolder = @"c:\bocaihuaapp\logon\";
        private object LogLocker = new object();
        private readonly ILogger<BocaiHuaAppController> _logger;

        public BocaiHuaAppController(ILogger<BocaiHuaAppController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public dynamic Get()
        {
            return Post();
        }
        [HttpPost]
        public dynamic Post()
        {
            string sAction = this.Request.Query["action"].ToString().ToLower();
            if (sAction.Equals("checkversion"))
            {
                return new
                {
                    VersionName = "1.0.2",
                    VersionCode = 2,
                    Memo = "测试版",
                    URL = "http://www.bocaihua.cn/AppService/BocaihuaApp_1.0.2.apk"
                };
            }
            else if (sAction.Equals("login"))
            {
                string sDeviceID = this.Request.Query["DeviceID"].ToString().ToLower();
                lock(LogLocker)
                {
                    if (!System.IO.Directory.Exists(LogFolder))
                    {
                        System.IO.Directory.CreateDirectory(LogFolder);
                    }

                    System.IO.File.WriteAllText(LogFolder + DateTime.Now.ToString("yyyy-MM-dd") + ".log", DateTime.Now.ToString() + "\r" + sDeviceID);
                }
                this._logger.LogInformation("Device Logon:" + sDeviceID);
                return new { Result = "OK" };
            }
            return null;
        }
    }
}