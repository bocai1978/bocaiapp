using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Com.Bocaihua.APP
{
    public class MessageBox
    {
        public enum MessageBoxResult { OK,Cancel,Yes,No};
        public enum MessageBoxButtons
        {
            OKOnly,
            OKCancel,
            YeaNoCancel
        }
        private static string TextOK = "确定";
        private static string TextCancel = "取消";
        private static string TextYes = "是";
        private static string TextNo = "否";
        public delegate void DislogCallBack(MessageBoxResult result);
        public static AlertDialog ShowDialog(Context pContext, string pTitle, string pMessage, MessageBoxButtons buttons, DislogCallBack asyncCallback, params string[] pButtonText)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(pContext);
            builder.SetTitle(pTitle);
            builder.SetMessage(pMessage);
            if (buttons == MessageBoxButtons.OKOnly)
            {
                if (pButtonText.Length > 0)
                {
                    builder.SetPositiveButton(pButtonText[0], delegate (object obj, DialogClickEventArgs e)
                    {
                        ((AlertDialog)obj).Dismiss();
                        asyncCallback( MessageBoxResult.OK );
                    });
                }
                else
                {
                    builder.SetPositiveButton(TextOK, delegate (object obj, DialogClickEventArgs e)
                    {
                        ((AlertDialog)obj).Dismiss();
                        asyncCallback(MessageBoxResult.OK);
                    });
                }
            }

            if (buttons == MessageBoxButtons.OKCancel)
            {
                if (pButtonText.Length > 0)
                {
                    builder.SetPositiveButton(pButtonText[0], delegate (object obj, DialogClickEventArgs e)
                    {
                        ((AlertDialog)obj).Dismiss();
                        asyncCallback(MessageBoxResult.OK);
                    });
                }
                else
                {
                    builder.SetPositiveButton(TextOK, delegate (object obj, DialogClickEventArgs e)
                    {
                        ((AlertDialog)obj).Dismiss();
                        asyncCallback(MessageBoxResult.OK);
                    });
                }
                if (pButtonText.Length > 1)
                {
                    builder.SetNegativeButton(pButtonText[1], delegate (object obj, DialogClickEventArgs e)
                    {
                        ((AlertDialog)obj).Dismiss();
                        asyncCallback(MessageBoxResult.Cancel);
                    });
                }
                else
                {
                    builder.SetNegativeButton(TextCancel, delegate (object obj, DialogClickEventArgs e)
                    {
                        ((AlertDialog)obj).Dismiss();
                        asyncCallback(MessageBoxResult.Cancel);
                    });
                }
            }
            if (buttons == MessageBoxButtons.YeaNoCancel)
            {
                if (pButtonText.Length > 0)
                {
                    builder.SetPositiveButton(pButtonText[0], delegate (object obj, DialogClickEventArgs e)
                    {
                        ((AlertDialog)obj).Dismiss();
                        asyncCallback(MessageBoxResult.Yes);
                    });
                }
                else
                {
                    builder.SetPositiveButton(TextYes, delegate (object obj, DialogClickEventArgs e)
                    {
                        ((AlertDialog)obj).Dismiss();
                        asyncCallback(MessageBoxResult.Yes);
                    });
                }
                if (pButtonText.Length > 1)
                {
                    builder.SetNeutralButton(pButtonText[1], delegate (object obj, DialogClickEventArgs e)
                    {
                        ((AlertDialog)obj).Dismiss();
                        asyncCallback(MessageBoxResult.No);
                    });
                }
                else
                {
                    builder.SetNeutralButton(TextNo, delegate (object obj, DialogClickEventArgs e)
                    {
                        ((AlertDialog)obj).Dismiss();
                        asyncCallback(MessageBoxResult.No);
                    });
                }
                if (pButtonText.Length > 2)
                {
                    builder.SetNegativeButton(pButtonText[2], delegate (object obj, DialogClickEventArgs e)
                    {
                        ((AlertDialog)obj).Dismiss();
                        asyncCallback(MessageBoxResult.Cancel);
                    });
                }
                else
                {
                    builder.SetNegativeButton(TextCancel, delegate (object obj, DialogClickEventArgs e)
                    {
                        ((AlertDialog)obj).Dismiss();
                        asyncCallback(MessageBoxResult.Cancel);
                    });
                }
            }

            AlertDialog alertDialog= builder.Create();
            alertDialog.Show();
            return alertDialog;
        }
    }
}