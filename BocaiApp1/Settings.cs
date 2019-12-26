using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Java.Util;

namespace Com.Bocaihua.APP
{
    public class Config
    {
        public string DeviceID = "";
        //大中小 123
        public int ButtonSize = 1;
        //在打开浏览器模式，或者文本模式时,自动开启相机功能
        public bool AutoOpenCamera = false;
        public Color TextViewBGColor = Color.White;
        public Color TextViewForeColor = Color.Black;
        public bool ShowSeekBar = true;
        //大中小 123
        public int TextViewFontSize = 1;
        public int TextViewAlpha = 255;
        public int WebViewAlpha = 255;
        public string WebViewURI = "http://www.bocaihua.cn/appservice/onlinedoc/stj/";
        public Color WebViewBGColor = Color.White;
        //下次打开保持上次的窗体大小， false 则全屏
        public bool KeepWindowSize = false;
        public System.Drawing.Rectangle WindowRectangle = new System.Drawing.Rectangle(0,0,-1, -1);
    }
    public class Settings
    {
        public static Settings Instance
        {
            get
            {
                if(mInstance == null)
                {
                    mInstance = new Settings();
                }
                return mInstance;
            }
        }
        private static Settings mInstance;
        private string m_ConfigFile = "";
        private System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Config));
        public Config Config = new Config();
        public Settings()
        {
            if (Settings.mInstance == null)
            {
                Settings.mInstance = this;
            }
            string rootPath = "";
            if (Android.OS.Environment.ExternalStorageState == Android.OS.Environment.MediaMounted)
            {// 优先保存到SD卡中
                rootPath = Application.Context.GetExternalFilesDir("config").AbsolutePath;
            }
            else
            {// 如果SD卡不存在，就保存到本应用的目录下
                rootPath = Application.Context.FilesDir.AbsolutePath +
                    File.Separator + Application.Context.PackageName +
                    File.Separator + "config"; ;
            }
            File file = new File(rootPath);
            if (!file.Exists())
            {
                file.Mkdirs();
            }
            file.Dispose();
            m_ConfigFile = rootPath + File.Separator + Application.Context.PackageName + ".cfg";
            if (System.IO.File.Exists(m_ConfigFile))
            {
                this.Reload();
            }
            else
            {
                this.Save();
            }
        }
        public void Reload()
        {
            using (System.IO.FileStream fs = System.IO.File.OpenRead(m_ConfigFile))
            {
                this.Config = xmlSerializer.Deserialize(fs) as Config;
            }
        }
        public void Save()
        {
            using (System.IO.FileStream fs = System.IO.File.Create(m_ConfigFile))
            {
                xmlSerializer.Serialize(fs, this.Config);
            }
        }
    }
}