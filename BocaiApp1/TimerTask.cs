using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Java.Lang;
using Android;
using Android.Util;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Com.Bocaihua.APP
{
    public class TimerTask: AsyncTask
    {
        public int WaitTime;
        private TimerFinishEventArgs eventArgs = new TimerFinishEventArgs();
        private int NowIndex = 0;
        private int SleepMilliSeconds = 500;
        private bool IsRunning = false;
        private Java.Lang.Object[] Args;
        private DateTime finishTime = new DateTime();
        public void Reset()
        {
            finishTime = DateTime.Now.AddMilliseconds(this.WaitTime);
            IsRunning = true;
        }
        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] args)
        {
            ///参数0. 等候时间     int 
            ///参数1. 是否马上生效 boolean
            ///参数2. 每次检查时间间隔  int 默认100ms


            eventArgs.IfSuccess = true;
            Args = args;
            if (args.Length < 2 )
            {
                eventArgs.IfSuccess = false;
                eventArgs.ErrorMessage = "参数错误.";
                eventArgs.ErrorDetail = "参数错误.";
                return null;
            }
            this.WaitTime = (int)args[0];
            if( (bool)args[1])
            {
                this.IsRunning = true;
            }
            if (args.Length >= 2)
            {
                this.SleepMilliSeconds= (int)args[1];
            }
            while (true)
            {
                try
                {
                    if ( IsRunning && DateTime.Now > this.finishTime)
                    {
                        this.PublishProgress(this.NowIndex);
                        IsRunning = false;
                    }
                    else
                    {

                    }
                    if(this.IsRunning)
                    {
                        System.Threading.Thread.Sleep(this.SleepMilliSeconds);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep( 2000 );
                    }
                }
                catch (Throwable ex)
                {
                    Log.Error( MainActivity.CurrentInstance.PackageName, ex, "Error on TimerTask");
                }
            }
        }
        protected override void OnProgressUpdate(params Java.Lang.Object[] values)
        {
            base.OnProgressUpdate(values);
            if( this.ProcessUpdate != null)
            {
                ProcessUpdateEventArgs e = new ProcessUpdateEventArgs();
                e.Value = (int)values[0];
                this.ProcessUpdate(e);
            }
        }
        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            //e.Result = result.ToString();
            if( this.ExecuteFinish != null)
            {
                this.ExecuteFinish(eventArgs);
            }
        }
        public event ExecuteFinishEvent ExecuteFinish;
        public delegate void ExecuteFinishEvent(TimerFinishEventArgs e);
        public event ProcessUpdateEvent ProcessUpdate;
        public delegate void ProcessUpdateEvent(ProcessUpdateEventArgs  e);
        public class ProcessUpdateEventArgs : System.EventArgs
        {
            public int Value = 0;
        }

        public class TimerFinishEventArgs : System.EventArgs
        {
            public object Result = "";
            public bool IfSuccess = true;
            public string ErrorMessage = "";
            public string ErrorDetail = "";
        }
    }
}
