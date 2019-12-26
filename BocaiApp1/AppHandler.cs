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
    public class AppHandler:Handler
    {
        public AppHandler()
        {
        }
        public delegate void MessageHandleEventHandler(Android.OS.Message msg);
        public event MessageHandleEventHandler MessageHandleEvent;

        public override void HandleMessage(Android.OS.Message msg)
        {
            MessageHandleEvent(msg);
        }
        public const int CHECK_NEW_VERSION = 0;
    }
}