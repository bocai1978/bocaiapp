using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Java.Net;

namespace Com.Bocaihua.APP
{
    public class UpdateManager
    {
        // 应用程序Context 
        private Context mContext;
        // 是否是最新的应用,默认为false 
        private bool isNew = false;
        private bool intercept = false;
        // 下载安装包的网络路径 
        private string apkUrl;
        // 保存APK的文件夹 
        private static string savePath;
        private static string saveFileName;
        // 下载线程 
        private System.Threading.Thread downLoadThread;
        private int progress;// 当前进度 
        // 进度条与通知UI刷新的handler和msg常量 
        private ProgressBar mProgress;
        private const int DOWN_UPDATE = 1;
        private const int DOWN_OVER = 2;
        private const int DOWN_ERROR = 3;
        public UpdateManager(Context context,string sUri)
        {
            this.mHandler = new MyHandler(this);
            mContext = context;
            if (Android.OS.Environment.ExternalStorageState == Android.OS.Environment.MediaMounted)
            {// 优先保存到SD卡中

                savePath = context.GetExternalFilesDir("update").AbsolutePath;
            }
            else
            {// 如果SD卡不存在，就保存到本应用的目录下
                savePath = context.FilesDir.AbsolutePath +
                    File.Separator + context.PackageName +
                    File.Separator + "update";
            }

            saveFileName = savePath + File.Separator + context.ApplicationContext.PackageName + ".apk";
            apkUrl = sUri;
        }

        /** 
         * 检查是否更新的内容 
         */
        public void checkUpdateInfo()
        {
            //这里的isNew本来是要从服务器获取的，我在这里先假设他需要更新 
            if (isNew)
            {

                return;
            }
            else
            {
                showUpdateDialog();
            }
        }

        /** 
         * 显示更新程序对话框，供主程序调用 
         */
        private void showUpdateDialog()
        {
            MessageBox.ShowDialog(mContext, "版本更新", "新版本已开放,请下载.",
                MessageBox.MessageBoxButtons.OKCancel,
                delegate (MessageBox.MessageBoxResult result)
                {
                    switch (result)
                    {
                        case MessageBox.MessageBoxResult.OK:
                            showDownloadDialog();
                            break;
                        case MessageBox.MessageBoxResult.Cancel:
                            break;
                    }
                },
                "下载",
                "以后再说");
        }

        /** 
         * 显示下载进度的对话框 
         */
        private void showDownloadDialog()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(mContext);
            builder.SetTitle("软件版本更新");
            LayoutInflater inflater = LayoutInflater.From(mContext);
            View v = inflater.Inflate(Resource.Layout.update_progress, null);
            mProgress = (ProgressBar)v.FindViewById(Resource.Id.progressBarUpdate);

            builder.SetView(v);
            builder.SetNegativeButton("取消", delegate (object obj, DialogClickEventArgs e)
            {
                intercept = true;
            });
            adDownload = builder.Show();
            
            downloadApk();
        }
        AlertDialog adDownload;
        //AlertDialog adCheckUpdate;

        /** 
         * 从服务器下载APK安装包 
         */
        private void downloadApk()
        {
            downLoadThread = new System.Threading.Thread(mdownApkRunnable);
            downLoadThread.Start();
        }

        private void mdownApkRunnable()
        {
            URL url;
            try
            {
                url = new URL(apkUrl);
                HttpURLConnection conn = (HttpURLConnection)url.OpenConnection();
                conn.Connect();
                int length = conn.ContentLength;
                System.IO.Stream ins = conn.InputStream;
                File file = new File(savePath);
                if (!file.Exists())
                {
                    file.Mkdir();
                }
                File apkFile = new File(saveFileName);
                FileOutputStream fos = new FileOutputStream(apkFile);
                int count = 0;
                byte[] buf = new byte[1024];
                while (!intercept)
                {
                    int numread = ins.Read(buf);
                    count += numread;
                    progress = (int)(((float)count / length) * 100);

                    // 下载进度 
                    mHandler.SendEmptyMessage(DOWN_UPDATE);
                    if (numread <= 0)
                    {
                        // 下载完成通知安装 
                        mHandler.SendEmptyMessage(DOWN_OVER);
                        break;
                    }
                    fos.Write(buf, 0, numread);
                }
                fos.Close();
                ins.Close();

            }
            catch (Throwable e)
            {
                mHandler.SendEmptyMessage(DOWN_ERROR);
                Log.Error(Application.Context.PackageName, e, "UpdateManager Error");
            }
        }
        /** 
         * 安装APK内容 
         */
        private void installAPK()
        {
            if (!System.IO.File.Exists(saveFileName))
            {
                return;
            }
            Intent intent = new Intent(Intent.ActionView);
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.N)
            {
                intent.SetDataAndType(Android.Net.Uri.Parse("content://" + saveFileName), "application/vnd.android.package-archive");
            }
            else
            {
                intent.SetDataAndType(Android.Net.Uri.Parse("file://" + saveFileName), "application/vnd.android.package-archive");
            }
            mContext.StartActivity(intent);

        }
        public void HandleMessage(Android.OS.Message msg)
        {
            switch (msg.What)
            {
                case DOWN_UPDATE:
                    this.mProgress.Progress = this.progress;
                    break;
                case DOWN_OVER:
                    this.adDownload.Dismiss();
                    this.installAPK();
                    break;
                case DOWN_ERROR:
                    this.adDownload.Dismiss();
                    MessageBox.ShowDialog(mContext, "下载失败", "下载过程中发生错误.",
                        MessageBox.MessageBoxButtons.OKOnly,
                        delegate (MessageBox.MessageBoxResult result)
                        {
                        },
                        "好吧");
                    break;

                default:
                    break;
            }
        }
        private class MyHandler : Handler
        {
            public MyHandler(UpdateManager pUpdateManager)
            {
                mUpdateManager = pUpdateManager;
            }
            UpdateManager mUpdateManager;
            public override void HandleMessage(Android.OS.Message msg)
            {
                mUpdateManager.HandleMessage(msg);
            }

        }

        private MyHandler mHandler;
    }
}
