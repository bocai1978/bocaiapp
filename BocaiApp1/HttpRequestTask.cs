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

namespace Com.Bocaihua.APP
{
    public class HttpRequestTask : AsyncTask
    {
        public enum ResponseTypeEnum { String, Bytes };
        public ResponseTypeEnum ResponseType;
        private ExecuteFinishEventArgs eventArgs = new ExecuteFinishEventArgs();
        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] args)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            if (args.Length <= 0)
            {
                eventArgs.IfSuccess = false;
                eventArgs.ErrorMessage = "参数错误.";
                eventArgs.ErrorDetail = "参数错误.缺少URL信息";
                return null;
            }
            string url = args[0].ToString();
            if (args.Length >= 2)
            {
                if (args[1].ToString().ToLower() == "bytes")
                {
                    this.ResponseType = ResponseTypeEnum.Bytes;
                }
            }
            try
            {
                if (this.ResponseType == ResponseTypeEnum.String)
                {
                    string result = client.DownloadString(url);
                    return result;
                }
                else
                {
                    byte[] bytes = client.DownloadData(url);
                    return bytes;
                }
            }
            catch (System.Exception e)
            {
                eventArgs.IfSuccess = false;
                eventArgs.ErrorMessage = "通讯错误.";
                eventArgs.ErrorDetail = e.ToString();
                return null;
            }
        }
        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            ExecuteFinishEventArgs e = new ExecuteFinishEventArgs();
            e.Result = result.ToString();
            this.ExecuteFinish(e);
        }
        public event ExecuteFinishEvent ExecuteFinish;
        public delegate void ExecuteFinishEvent(ExecuteFinishEventArgs e);
        public class ExecuteFinishEventArgs : System.EventArgs
        {
            public object Result = "";
            public bool IfSuccess = true;
            public string ErrorMessage = "";
            public string ErrorDetail = "";
        }
    }
}