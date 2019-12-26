using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
namespace Com.Bocaihua.APP
{
    /**
     * log日志统计保存
     * 
     * @author way
     * 
     */

    public class LogcatHelper
    {

        private static LogcatHelper INSTANCE = null;
        private static String PATH_LOGCAT;
        private LogDumper mLogDumper = null;
        private string mLevel = "E";
        //private int mPId;
        private string mTAG;
        /**
         * 
         * 初始化目录
         * 
         * */
        public void Init(Context context)
        {
            if (Android.OS.Environment.ExternalStorageState == Android.OS.Environment.MediaMounted)
            {// 优先保存到SD卡中

                PATH_LOGCAT = context.GetExternalFilesDir("log").AbsolutePath;
            }
            else
            {// 如果SD卡不存在，就保存到本应用的目录下
                PATH_LOGCAT = context.FilesDir.AbsolutePath +
                    File.Separator + context.PackageName +
                    File.Separator + "log"; ;
            }
            File file = new File(PATH_LOGCAT);
            if (!file.Exists())
            {
                file.Mkdirs();
            }
        }
        public static LogcatHelper CreateInstance(Context context)
        {
            return CreateInstance(context, "E");
        }

        public static LogcatHelper CreateInstance(Context context, string level)
        {
            if (INSTANCE == null)
            {
                INSTANCE = new LogcatHelper(context,level);
            }
            return INSTANCE;
        }

        private LogcatHelper(Context context,string pLevel)
        {
            Init(context);
            //mPId = Process.my.MyPid();
            this.mTAG = context.PackageName;
            this.mLevel = pLevel;
            if (mLogDumper == null)
                mLogDumper = new LogDumper(this.mTAG, this.mLevel, PATH_LOGCAT);
            mLogDumper.Start();
        }
        public void Stop()
        {
            if (mLogDumper != null)
            {
                mLogDumper.StopLogs();
                mLogDumper = null;
            }
        }

        private class LogDumper : Java.Lang.Thread
        {

            private Java.Lang.Process logcatProc;
            private BufferedReader mReader = null;
            private bool mRunning = true;
            String cmds = null;
            private String mTag;
            private FileOutputStream fos = null;

            public LogDumper(string tag, string level,string dir)
            {
                this.mTag = tag;
                try
                {
                    fos = new FileOutputStream(new File(dir, "Log-"
                            + DateTime.Now.ToFileTime().ToString() + ".log"));
                }
                catch (FileNotFoundException e)
                {
                    e.PrintStackTrace();
                }

                /**
                 * 
                 * 日志等级：*:v , *:d ,*:i , *:w , *:e , *:f , *:s
                 * 
                 * 显示当前mPID程序的 E和W等级的日志.
                 * 
                 * */

                // cmds = "logcat *:e *:w | grep \"(" + mPID + ")\"";
                // cmds = "logcat  | grep \"(" + mPID + ")\"";//打印所有日志信息
                // cmds = "logcat -s way";//打印标签过滤信息
                // cmds = "logcat *:e *:i | grep \"(" + mPID + ")\"";
                cmds = "logcat " + this.mTag + ":" + level + " *:F  "; //| grep \"(" + mPID + ")\"";
            }

            public void StopLogs()
            {
                mRunning = false;
            }

            public override void Run()
            {
                try
                {

                    logcatProc = Java.Lang.Runtime.GetRuntime().Exec(cmds);
                    mReader = new BufferedReader(new InputStreamReader(
                            logcatProc.InputStream), 1024);
                    String line = null;
                    while (mRunning && (line = mReader.ReadLine()) != null)
                    {
                        if (!mRunning)
                        {
                            break;
                        }
                        if (line.Length == 0)
                        {
                            continue;
                        }
                        if (fos != null ) //&& line.Contains(mPID))
                        {
                            fos.Write(System.Text.Encoding.UTF8.GetBytes( line + "\r\n"));
                        }
                    }

                }
                catch (IOException e)
                {
                    e.PrintStackTrace();
                }
                finally
                {
                    if (logcatProc != null)
                    {
                        logcatProc.Destroy();
                        logcatProc = null;
                    }
                    if (mReader != null)
                    {
                        try
                        {
                            mReader.Close();
                            mReader = null;
                        }
                        catch (IOException e)
                        {
                            e.PrintStackTrace();
                        }
                    }
                    if (fos != null)
                    {
                        try
                        {
                            fos.Close();
                        }
                        catch (IOException e)
                        {
                            e.PrintStackTrace();
                        }
                        fos = null;
                    }
                }
            }
        }
    }
}