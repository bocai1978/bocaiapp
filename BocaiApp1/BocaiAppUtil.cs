using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Java.Net;
using Java.Security;
using Java.Util;
using static Android.Content.PM.PackageManager;

namespace Com.Bocaihua.APP
{
    class BocaiHuaAppUtils
    {
#if DEBUG
        public const string CHECK_UPDATE_URL = "http://www.bocaihua.cn/BocaiHuaApp/upgradedebug.txt";
#else
        public const string CHECK_UPDATE_URL = "http://www.bocaihua.cn/BocaiHuaApp/upgrade.txt";
#endif
        public const string APPSERVICE_URL = "http://www.bocaihua.cn/BocaiHuaApp/Service.aspx";

        /**
             * <功能描述> 启动应用程序
             * 
             * @return void [返回类型说明]
             */
        public static bool StartUpApplication(Context mContext, string pkg)
        {
            PackageManager packageManager = mContext.PackageManager;
            PackageInfo packageInfo = null;
            try
            {
                // 获取指定包名的应用程序的PackageInfo实例
                packageInfo = packageManager.GetPackageInfo(pkg, 0);
            }
            catch (NameNotFoundException e)
            {
                // 未找到指定包名的应用程序
                e.PrintStackTrace();
                // 提示没有GPS Test Plus应用
                Log.Error(Application.Context.PackageName, e, "未找到指定的程序:" + pkg);
                //Toast.MakeText(mContext,"未找到指定的程序", ToastLength.Short).Show();
                return false;
            }
            if (packageInfo != null)
            {
                // 已安装应用
                Intent resolveIntent = new Intent(Intent.ActionMain, null);
                resolveIntent.AddCategory(Intent.CategoryLauncher);
                resolveIntent.SetPackage(packageInfo.PackageName );
                IList<ResolveInfo> apps = packageManager.QueryIntentActivities( resolveIntent, 0);
                ResolveInfo ri = null;
                try
                {
                    ri = apps[0];
                }
                catch ( Java.Lang.Exception e)
                {
                    Log.Error(Application.Context.PackageName, e,"未找到指定的程序:" + pkg);
                    return false;
                }
                if (ri != null)
                {
                    // 获取应用程序对应的启动Activity类名
                    string className = ri.ActivityInfo.Name;
                    // 启动应用程序对应的Activity
                    Intent intent = new Intent(Intent.ActionMain);
                    intent.AddCategory(Intent.CategoryLauncher);
                    ComponentName componentName = new ComponentName(pkg, className);
                    intent.SetComponent(componentName);
                    mContext.StartActivity(intent);
                    return true;
                }
            }
            return false;
        }
        //保存文件的路径
        private const string CACHE_IMAGE_DIR = "aray/cache/devices";
        //保存的文件 采用隐藏文件的形式进行保存
        private const string DEVICES_FILE_NAME = ".DEVICES";

        /**
         * 获取设备唯一标识符
         *
         * @param context
         * @return
         */
        public static string getDeviceId(Context context)
        {
            //读取保存的在sd卡中的唯一标识符
            string deviceId = readDeviceID(context);
            //用于生成最终的唯一标识符
            StringBuffer s = new StringBuffer();
            //判断是否已经生成过,
            if (deviceId != null && !"".Equals(deviceId))
            {
                return deviceId;
            }
            try
            {
                //获取IMES(也就是常说的DeviceId)
                deviceId = getIMIEStatus(context);
                s.Append(deviceId);
            }
            catch (Java.Lang.Exception e)
            {
                e.PrintStackTrace();
                Log.Error(context.PackageName, e, "ReadDeviceID ERROR.");
                //return null;
            }
            catch (System.Exception e)
            {
                Log.Error(context.PackageName, "ReadDeviceID ERROR.\r\n" + e.ToString());
                //return null;
            }

            try
            {
                //获取设备的MACAddress地址 去掉中间相隔的冒号
                deviceId = getLocalMac(context).Replace(":", "");
                s.Append(deviceId);
            }
            catch (Java.Lang.Exception e)
            {
                e.PrintStackTrace();
                Log.Error(context.PackageName, e, "ReadDeviceID ERROR.");
                //return null;
            }
            catch (System.Exception e)
            {
                Log.Error(context.PackageName, "ReadDeviceID ERROR.\r\n" + e.ToString());
                //return null;
            }
            
            //如果以上搜没有获取相应的则自己生成相应的UUID作为相应设备唯一标识符
            if (s == null || s.Length() <= 0)
            {
                UUID uuid = UUID.RandomUUID();
                deviceId = uuid.ToString().Replace("-", "");
                s.Append(deviceId);
            }
            //为了统一格式对设备的唯一标识进行md5加密 最终生成32位字符串
            string md5 = getMD5(s.ToString(), false);
            if (s.Length() > 0)
            {
                //持久化操作, 进行保存到SD卡中
                saveDeviceID(md5, context);
            }
            return md5;
        }


        /**
         * 读取固定的文件中的内容,这里就是读取sd卡中保存的设备唯一标识符
         *
         * @param context
         * @return
         */
        public static string readDeviceID(Context context)
        {
            string sFile = getDevicesDir(context);
            StringBuffer buffer = new StringBuffer();
            try
            {
                return System.IO.File.ReadAllText(sFile);
            }
            catch (Java.Lang.Exception e)
            {
                e.PrintStackTrace();
                Log.Error(context.PackageName, e, "ReadDeviceID ERROR.");
                return null;
            }
            catch (System.Exception e)
            {
                Log.Error(context.PackageName, "ReadDeviceID ERROR.\r\n" + e.ToString());
                return null;
            }
        }

        /**
         * 获取设备的DeviceId(IMES) 这里需要相应的权限<br/>
         * 需要 READ_PHONE_STATE 权限
         *
         * @param context
         * @return
         */
        private static string getIMIEStatus(Context context)
        {
            string imei;
            TelephonyManager tm = (TelephonyManager)context.GetSystemService(Context.TelephonyService);
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                imei = tm.Imei;
            }
            else
            {
                imei = tm.DeviceId;
            }
            return imei;
        }


        /**
         * 获取设备MAC 地址 由于 6.0 以后 WifiManager 得到的 MacAddress得到都是 相同的没有意义的内容
         * 所以采用以下方法获取Mac地址
         * @param context
         * @return
         */
        private static string getLocalMac(Context context)
        {
            //        WifiManager wifi = (WifiManager) context
            //                .getSystemService(Context.WIFI_SERVICE);
            //        WifiInfo info = wifi.getConnectionInfo();
            //        return info.getMacAddress();
            NetworkInterface networkInterface = null;
            try
            {
                networkInterface = NetworkInterface.GetByName("eth1");
                if (networkInterface == null)
                {
                    networkInterface = NetworkInterface.GetByName("wlan0");
                }
                if (networkInterface == null)
                {
                    return "";
                }
                byte[] addr = networkInterface.GetHardwareAddress();

                return BitConverter.ToString(addr).Replace("-", ":");
            }
            catch (Java.Lang.Exception e)
            {
                e.PrintStackTrace();
                Log.Error(context.PackageName, e, "ReadDeviceID ERROR.");
                return "";
            }
            catch (System.Exception e)
            {
                Log.Error(context.PackageName, "ReadDeviceID ERROR.\r\n" + e.ToString());
                return "";
            }

        }

        /**
         * 保存 内容到 SD卡中,  这里保存的就是 设备唯一标识符
         * @param str
         * @param context
         */
        public static void saveDeviceID(string str, Context context)
        {
            string sFile = getDevicesDir(context);
            try
            {
                System.IO.File.WriteAllText(sFile, str);
            }
            catch (Java.Lang.Exception e)
            {
                e.PrintStackTrace();
                Log.Error(context.PackageName, e, "ReadDeviceID ERROR.");
            }
            catch (System.Exception e)
            {
                Log.Error(context.PackageName, "ReadDeviceID ERROR.\r\n" + e.ToString());
            }
        }

        /**
         * 对挺特定的 内容进行 md5 加密
         * @param message  加密明文
         * @param upperCase  加密以后的字符串是是大写还是小写  true 大写  false 小写
         * @return
         */
        public static string getMD5(string message, bool upperCase)
        {
            string md5str = "";
            try
            {
                MessageDigest md = MessageDigest.GetInstance("MD5");

                byte[] input = System.Text.Encoding.UTF8.GetBytes(message);

                byte[] buff = md.Digest(input);

                md5str = bytesToHex(buff, upperCase);

            }
            catch (Java.Lang.Exception e)
            {
                e.PrintStackTrace();
                Log.Error(Application.Context.PackageName, e, "ReadDeviceID ERROR.");
            }
            catch (System.Exception e)
            {
                Log.Error(Application.Context.PackageName, "ReadDeviceID ERROR.\r\n" + e.ToString());
            }
            return md5str;
        }


        public static string bytesToHex(byte[] bytes, bool upperCase)
        {
            StringBuffer md5str = new StringBuffer();
            int digital;
            for (int i = 0; i < bytes.Length; i++)
            {
                digital = bytes[i];

                if (digital < 0)
                {
                    digital += 256;
                }
                if (digital < 16)
                {
                    md5str.Append("0");
                }
                md5str.Append(Integer.ToHexString(digital));
            }
            if (upperCase)
            {
                return md5str.ToString().ToUpper();
            }
            return md5str.ToString().ToLower();
        }

        /**
         * 统一处理设备唯一标识 保存的文件的地址
         * @param context
         * @return
         */
        private static string getDevicesDir(Context context)
        {
            string mCropFile = "";
            string cropDir;
            if (Android.OS.Environment.ExternalStorageState.Equals(Android.OS.Environment.MediaMounted))
            {
                cropDir = Application.Context.GetExternalFilesDir(CACHE_IMAGE_DIR).Path;
            }
            else
            {
                cropDir = context.GetExternalFilesDir(CACHE_IMAGE_DIR).Path;
            }
            if (!System.IO.Directory.Exists(cropDir))
            {
                System.IO.Directory.CreateDirectory(cropDir);
            }
            mCropFile = cropDir + Java.IO.File.Separator + DEVICES_FILE_NAME; // 用当前时间给取得的图片命名
            return mCropFile;
        }
        public static string HttpRequestGetString(string pUrl, 
            string pMethod = "GET",
            params System.Collections.Generic.KeyValuePair<string, string>[] pRequestList)
        {
            
            URL url = new URL(pUrl);
            HttpURLConnection conn = (HttpURLConnection)url.OpenConnection();
            conn.ConnectTimeout = 5000;
            conn.RequestMethod = pMethod;
            string sRequestParameter = "";
            if(pRequestList!= null&& pRequestList.Length >0)
            {
                foreach(System.Collections.Generic.KeyValuePair<string, string> kvPair in pRequestList)
                {
                    //conn.AddRequestProperty(kvPair.Key, kvPair.Value); 
                    sRequestParameter += kvPair.Key + "=" + kvPair.Value + "&";
                }
                sRequestParameter.Remove(sRequestParameter.Length - 1);
                if(pMethod.ToLower() == "POST")
                {
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(sRequestParameter);
                    conn.OutputStream.Write(bytes);
                }
            }

            if (Java.Net.HttpStatus.Ok == conn.ResponseCode)
            {
                
                Log.Info(Application.Context.PackageName, "HTTP请求成功:" + pUrl);
                System.IO.Stream inSteam = conn.InputStream;
                int read = -1;
                ByteArrayOutputStream baos = new ByteArrayOutputStream();
                while ((read = inSteam.ReadByte()) != -1)
                {
                    baos.Write(read);
                }
                inSteam.Close();
                string backcontent = System.Text.Encoding.UTF8.GetString(baos.ToByteArray());
                Log.Info(Application.Context.PackageName, backcontent);
                return backcontent;
            }
            else
            {
                Log.Info(Application.Context.PackageName, "HTTP请求失败");
            }
            return null;
        }
    }
}