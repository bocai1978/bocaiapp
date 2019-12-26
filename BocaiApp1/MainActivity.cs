using System;
using Android;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Webkit;
using Java.Lang;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.Locations;
using Android.Util;
using Java.IO;
using Android.Graphics.Drawables;
using Java.Net;
using Android.Content.Res;

namespace Com.Bocaihua.APP
{
    [Activity( ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.SmallestScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Navigation , Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    //android:configChanges="orientation|screenSize|smallestScreenSize|keyboard|keyboardHidden|navigation"
    public class MainActivity : AppCompatActivity, View.IOnTouchListener
    {
        #region 全局变量定义
        public static string TAG = "";
        public string[] PermistionList = {
            Android.Manifest.Permission.ReadExternalStorage,
            Android.Manifest.Permission.WriteExternalStorage
        };
        LayoutInflater inflater;
        /// <summary>        
        /// 定义浮动窗口布局对象
        /// </summary>        
        LinearLayout mFloatLayout;
        /// <summary>        
        /// 定义全屏窗口布局对象
        /// </summary>        
        RelativeLayout mFullWindowLayout;
        /// <summary>        
        /// 配置窗口布局对象
        /// </summary>        
        View mSettingsWindowLayout;

        /// <summary>        
        /// 悬浮框Layout对象参数        
        /// </summary>        
        WindowManagerLayoutParams wmParamsFloatButton;
        WindowManagerLayoutParams wmParamsView;
        WindowManagerLayoutParams wmSettingsParams;
        /// <summary>        
        /// 创建浮动窗口设置布局参数的对象        
        /// </summary>        
        IWindowManager mWindowManager;
        /// <summary>        
        /// 悬浮框关闭按钮对象        
        /// </summary>        
        ImageView buttonHome;
        ImageView buttonWebView;
        ImageView buttonSettings;
        ImageView buttonQuit;
        ImageView buttonDocView;
        /// <summary>
        /// 全屏窗体
        /// </summary>
        TextView textView1;
        WebView webView1;
        SeekBar seekBar1;
        LinearLayout linearLayoutButton;
        ImageView buttonWindowsBottom;
        ImageView buttonWindowsTop;
        ImageView buttonWindowsLeft;
        ImageView buttonWindowsRight;
        ImageView buttonTurnLast;
        ImageView buttonTurnNext;
        /// <summary>
        /// 配置窗体
        /// </summary>
        TextView textViewFloatWindowGrant;
        TextView textViewSDWriteGrant;
        TextView textViewGPSGrant;
        TextView textViewCameraGrant;
        TextView textViewHardwareGrant;
        TextView textViewVersion;
        TextView textViewUpdateVersion;
        Button buttonCheckUpdate;
        SeekBar seekBarWebViewAlpha;
        SeekBar seekBarTextViewAlpha;
        EditText textViewURL;
        ImageView btnButtonSize_s;
        ImageView btnButtonSize_m;
        ImageView btnButtonSize_l;
        Button buttonSaveSettings;
        Button buttonQuitApp;
        TextView btnTextColorTheme_1;
        TextView btnTextColorTheme_2;
        TextView btnTextColorTheme_3;
        TextView btnTextColorTheme_UserDefine;
        TextView txtReaderBGColor;
        TextView txtReaderTextColor;
        TextView btnTextSize_s;
        TextView btnTextSize_m;
        TextView btnTextSize_l;
        CheckBox chkAutoOpenCameraWhenOpenView;
        private float _viewX;
        private float _viewY;
        private int fullwindow_x = -1;
        private int fullwindow_width = -1;
        private int fullwindow_height = -1;
        private int iDocFileIndex = 0;
        #endregion
        #region 创建悬浮框
        /// <summary>        
        /// 创建悬浮框        
        /// </summary>        
        private void createFloatView()
        {
            hideButtonTask.ProcessUpdate += HideButtonTask_ProcessUpdate;
            hideButtonTask.Execute(5 * 1000,false);

            if(Settings.Instance.Config.ButtonSize == 2)
            {
                this.currentButtonWidth = this.Resources.GetDimensionPixelSize(Resource.Dimension.button_width_m);
                this.currentButtonHeight = this.Resources.GetDimensionPixelSize(Resource.Dimension.button_height_m);
            }
            else if (Settings.Instance.Config.ButtonSize == 3)
            {
                this.currentButtonWidth = this.Resources.GetDimensionPixelSize(Resource.Dimension.button_width_l);
                this.currentButtonHeight = this.Resources.GetDimensionPixelSize(Resource.Dimension.button_height_l);
            }
            else
            {
                this.currentButtonWidth = this.Resources.GetDimensionPixelSize(Resource.Dimension.button_width_s);
                this.currentButtonHeight = this.Resources.GetDimensionPixelSize(Resource.Dimension.button_height_s);
            }

            //创建WindowManager接口实现对象
            mWindowManager = this.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            wmParamsFloatButton = new WindowManagerLayoutParams();
            //设置窗体类型
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                wmParamsFloatButton.Type = WindowManagerTypes.ApplicationOverlay;
            }
            else
            {
                wmParamsFloatButton.Type = WindowManagerTypes.Phone;
            }
            //设置图片格式，效果为背景透明              
            wmParamsFloatButton.Format = Android.Graphics.Format.Translucent;
            //设置浮动窗口不可聚焦（实现操作除浮动窗口外的其他可见窗口的操作）              
            wmParamsFloatButton.Flags = WindowManagerFlags.NotFocusable;
            //调整悬浮窗显示的停靠位置为左侧置顶             
            wmParamsFloatButton.Gravity = GravityFlags.Left | GravityFlags.Top; //.Center;
            // 以屏幕左上角为原点，设置x、y初始值，相对于gravity              
            wmParamsFloatButton.X = 0;
            wmParamsFloatButton.Y = 200;
            //设置悬浮窗口长宽数据              
            wmParamsFloatButton.Width = this.currentButtonWidth;
            wmParamsFloatButton.Height = this.currentButtonHeight;
            //获取浮动窗口视图所在布局         
            inflater = LayoutInflater.From(this.ApplicationContext);
            mFloatLayout = (LinearLayout)inflater.Inflate(Resource.Layout.float_window_s, null);
            this.InitFloatButtonWindow();
            //添加mFloatLayout              
            mWindowManager.AddView(this.mFloatLayout, this.wmParamsFloatButton);
            //全屏view
            wmParamsView = new WindowManagerLayoutParams();
            wmParamsView.CopyFrom(wmParamsFloatButton);
            if (Settings.Instance.Config.KeepWindowSize)
            {
                wmParamsView.X = Settings.Instance.Config.WindowRectangle.X;
                wmParamsView.Y = Settings.Instance.Config.WindowRectangle.Y;
                wmParamsView.Width = Settings.Instance.Config.WindowRectangle.Width;
                wmParamsView.Height = Settings.Instance.Config.WindowRectangle.Height;
            }
            else
            {
                wmParamsView.X = 0;
                wmParamsView.Y = 0;
                wmParamsView.Width = ViewGroup.LayoutParams.MatchParent;
                wmParamsView.Height = ViewGroup.LayoutParams.MatchParent;
            }

            this.mFullWindowLayout = (RelativeLayout)inflater.Inflate(Resource.Layout.view_full_window, null);
            this.textView1 = this.mFullWindowLayout.FindViewById<TextView>(Resource.Id.textView1);
            this.textView1.SetBackgroundColor(Settings.Instance.Config.TextViewBGColor);
            this.textView1.SetTextColor(Settings.Instance.Config.TextViewForeColor);
            if (Settings.Instance.Config.TextViewFontSize == 2)
                this.textView1.SetTextSize(ComplexUnitType.Sp, this.Resources.GetDimensionPixelSize(Resource.Dimension.abc_text_size_medium_material));
            else if (Settings.Instance.Config.TextViewFontSize == 3)
                this.textView1.SetTextSize(ComplexUnitType.Sp, this.Resources.GetDimensionPixelSize(Resource.Dimension.abc_text_size_large_material));
            else
                this.textView1.SetTextSize(ComplexUnitType.Sp, this.Resources.GetDimensionPixelSize(Resource.Dimension.abc_text_size_small_material));
            this.textView1.Alpha = Settings.Instance.Config.TextViewAlpha;
            this.loadDocFile(0);

            this.webView1 = this.mFullWindowLayout.FindViewById<WebView>(Resource.Id.webView1);
            this.webView1.Alpha = Settings.Instance.Config.WebViewAlpha;
            this.webView1.SetBackgroundColor(Settings.Instance.Config.WebViewBGColor);
            this.webView1.Settings.JavaScriptEnabled = true;
            //设置webview支持js
            this.webView1.SetBackgroundColor(Color.Transparent);
            this.webView1.LoadUrl(Settings.Instance.Config.WebViewURI);
            Paint paint = new Paint();
            this.webView1.SetLayerType(LayerType.Software, paint);
            //系统默认会通过手机浏览器打开网页，为了能够直接通过WebView显示网页，则必须设置
            this.webView1.SetWebChromeClient(new WebChromeClient());

            this.seekBar1 = this.mFullWindowLayout.FindViewById<SeekBar>(Resource.Id.seekBar1);
            this.seekBar1.Progress = Settings.Instance.Config.TextViewAlpha;
            this.seekBar1.ProgressChanged += SeekBar1_ProgressChanged;
            if(Settings.Instance.Config.ShowSeekBar == false)
            {
                this.seekBar1.Visibility = ViewStates.Invisible;
            }
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                this.seekBar1.SetProgress(180, true);
            }
            else
            {
                this.seekBar1.Progress = 180;
            }
            this.buttonWindowsBottom = this.mFullWindowLayout.FindViewById<ImageView>(Resource.Id.buttonWindowsBottom);
            this.buttonWindowsBottom.Touch += ButtonWindowsBottom_Touch;
            this.buttonWindowsBottom.ImageAlpha = 150;
            this.buttonWindowsTop = this.mFullWindowLayout.FindViewById<ImageView>(Resource.Id.buttonWindowsTop);
            this.buttonWindowsTop.Touch += ButtonWindowsBottom_Touch;
            this.buttonWindowsTop.ImageAlpha = 150;
            this.buttonWindowsLeft = this.mFullWindowLayout.FindViewById<ImageView>(Resource.Id.buttonWindowsLeft);
            this.buttonWindowsLeft.Touch += ButtonWindowsBottom_Touch;
            this.buttonWindowsLeft.ImageAlpha = 150;
            this.buttonWindowsRight= this.mFullWindowLayout.FindViewById<ImageView>(Resource.Id.buttonWindowsRight);
            this.buttonWindowsRight.Touch += ButtonWindowsBottom_Touch;
            this.buttonWindowsRight.ImageAlpha = 150;
            this.buttonTurnLast = this.mFullWindowLayout.FindViewById<ImageView>(Resource.Id.buttonTurnLast);
            this.buttonTurnLast.Click += ButtonTurnLast_Click;
            this.buttonTurnLast.ImageAlpha = 150;
            this.buttonTurnNext= this.mFullWindowLayout.FindViewById<ImageView>(Resource.Id.buttonTurnNext);
            this.buttonTurnNext.Click += ButtonTurnNext_Click;
            this.buttonTurnNext.ImageAlpha = 150;

            this.showFullButtons(false);
            this.showFullWindow(false);

            this.wmSettingsParams = new WindowManagerLayoutParams();
            this.wmSettingsParams.CopyFrom(this.wmParamsFloatButton);
            this.wmSettingsParams.Width = ViewGroup.LayoutParams.MatchParent;
            this.wmSettingsParams.Height = ViewGroup.LayoutParams.MatchParent;
            this.wmSettingsParams.Gravity = GravityFlags.Top | GravityFlags.Left;
            this.wmSettingsParams.Format = Android.Graphics.Format.Opaque;

            this.mSettingsWindowLayout = (LinearLayout)inflater.Inflate(Resource.Layout.settings_window, null);

            mFloatLayout.Measure(View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
                View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
        }

        private void BtnTextColorTheme_Click(object sender, EventArgs e)
        {
            TextView btn = sender as TextView;
            switch (btn.Id)
            {
                case Resource.Id.btnTextColorTheme_1:
                    Settings.Instance.Config.TextViewBGColor = ((ColorDrawable)(this.btnTextColorTheme_1.Background)).Color;
                    Settings.Instance.Config.TextViewForeColor = new Color(this.btnTextColorTheme_1.CurrentTextColor);
                    break;
                case Resource.Id.btnTextColorTheme_2:
                    Settings.Instance.Config.TextViewBGColor = ((ColorDrawable)(this.btnTextColorTheme_2.Background)).Color;
                    Settings.Instance.Config.TextViewForeColor = new Color(this.btnTextColorTheme_2.CurrentTextColor);
                    break;
                case Resource.Id.btnTextColorTheme_3:
                    Settings.Instance.Config.TextViewBGColor = ((ColorDrawable)(this.btnTextColorTheme_3.Background)).Color;
                    Settings.Instance.Config.TextViewForeColor = new Color(this.btnTextColorTheme_3.CurrentTextColor);
                    break;
            }
            RefreshTextColorFromSettings();
        }

        private void BtnTextSize_Click(object sender, EventArgs e)
        {
            TextView btn = sender as TextView;
            switch (btn.Id)
            {
                case Resource.Id.btnTextSize_s:
                    Settings.Instance.Config.TextViewFontSize = 1;
                    break;
                case Resource.Id.btnTextSize_m:
                    Settings.Instance.Config.TextViewFontSize = 2;
                    break;
                case Resource.Id.btnTextSize_l:
                    Settings.Instance.Config.TextViewFontSize = 3;
                    break;
            }

            RefreshTextSizeFromSettings();
        }
        private void RefreshTextSizeFromSettings()
        {
            switch (Settings.Instance.Config.TextViewFontSize)
            {
                case 1:
                    this.btnTextSize_s.SetBackgroundColor(Color.Black);
                    this.btnTextSize_m.SetBackgroundColor(Color.LightGray);
                    this.btnTextSize_l.SetBackgroundColor(Color.LightGray);
                    this.textView1.SetTextSize(ComplexUnitType.Px, this.btnTextSize_s.TextSize);
                    this.btnTextColorTheme_UserDefine.SetTextSize(ComplexUnitType.Px, this.btnTextSize_s.TextSize);
                    break;
                case 2:
                    this.btnTextSize_s.SetBackgroundColor(Color.LightGray);
                    this.btnTextSize_m.SetBackgroundColor(Color.Black);
                    this.btnTextSize_l.SetBackgroundColor(Color.LightGray);
                    this.textView1.SetTextSize(ComplexUnitType.Px, this.btnTextSize_m.TextSize);
                    this.btnTextColorTheme_UserDefine.SetTextSize(ComplexUnitType.Px, this.btnTextSize_m.TextSize);
                    break;
                case 3:
                    this.btnTextSize_s.SetBackgroundColor(Color.LightGray);
                    this.btnTextSize_m.SetBackgroundColor(Color.LightGray);
                    this.btnTextSize_l.SetBackgroundColor(Color.Black);
                    this.textView1.SetTextSize(ComplexUnitType.Px, this.btnTextSize_l.TextSize);
                    this.btnTextColorTheme_UserDefine.SetTextSize( ComplexUnitType.Px,this.btnTextSize_l.TextSize);
                    break;
            }
            this.textView1.Alpha = Settings.Instance.Config.TextViewAlpha;
        }

        private void RefreshTextColorFromSettings()
        {
            this.btnTextColorTheme_UserDefine.SetTextColor(Settings.Instance.Config.TextViewForeColor);
            this.btnTextColorTheme_UserDefine.SetBackgroundColor(Settings.Instance.Config.TextViewBGColor);

            this.textView1.SetTextColor(Settings.Instance.Config.TextViewForeColor);
            this.textView1.SetBackgroundColor(Settings.Instance.Config.TextViewBGColor);

            this.txtReaderBGColor.Text = BitConverter.ToString(BitConverter.GetBytes(Settings.Instance.Config.TextViewBGColor.ToArgb())).Replace("-", "").Substring(2);
            this.txtReaderTextColor.Text = BitConverter.ToString(BitConverter.GetBytes(Settings.Instance.Config.TextViewForeColor.ToArgb())).Replace("-", "").Substring(2);
        }

        private void BtnButtonSize_Click(object sender, EventArgs e)
        {
            ImageView btn = sender as ImageView;
            switch(btn.Id)
            {
                case Resource.Id.btnButtonSize_s:
                    this.btnButtonSize_s.ImageAlpha = 255;
                    this.btnButtonSize_m.ImageAlpha = 150;
                    this.btnButtonSize_l.ImageAlpha = 150;
                    Settings.Instance.Config.ButtonSize = 1;
                    break;
                case Resource.Id.btnButtonSize_m:
                    this.btnButtonSize_s.ImageAlpha = 150;
                    this.btnButtonSize_m.ImageAlpha = 255;
                    this.btnButtonSize_l.ImageAlpha = 150;
                    Settings.Instance.Config.ButtonSize = 2; 
                    break;
                case Resource.Id.btnButtonSize_l:
                    this.btnButtonSize_s.ImageAlpha = 150;
                    this.btnButtonSize_m.ImageAlpha = 150;
                    this.btnButtonSize_l.ImageAlpha = 255;
                    Settings.Instance.Config.ButtonSize = 3;
                    break;
            }
            RefreshButtonSizeBySettings();
        }
        private void RefreshButtonSizeBySettings()
        {
            switch (Settings.Instance.Config.ButtonSize)
            {
                case 1:
                    this.btnButtonSize_s.ImageAlpha = 255;
                    this.btnButtonSize_m.ImageAlpha = 150;
                    this.btnButtonSize_l.ImageAlpha = 150;
                    break;
                case 2:
                    this.btnButtonSize_s.ImageAlpha = 150;
                    this.btnButtonSize_m.ImageAlpha = 255;
                    this.btnButtonSize_l.ImageAlpha = 150;
                    break;
                case 3:
                    this.btnButtonSize_s.ImageAlpha = 150;
                    this.btnButtonSize_m.ImageAlpha = 150;
                    this.btnButtonSize_l.ImageAlpha = 255;
                    break;
            }
            ResizeFloatButton(Settings.Instance.Config.ButtonSize);
        }

        private void ButtonTurnNext_Click(object sender, EventArgs e)
        {
            if (this.webView1.Visibility == ViewStates.Visible)
            {
                this.webView1.GoForward();
            }
            if (this.textView1.Visibility == ViewStates.Visible)
            {
                this.iDocFileIndex++;
                if (this.iDocFileIndex > 108)
                {
                    this.iDocFileIndex = 0;
                }
                this.loadDocFile(this.iDocFileIndex);
            }
        }

        private void ButtonTurnLast_Click(object sender, EventArgs e)
        {
            if(this.webView1.Visibility== ViewStates.Visible)
            {
                this.webView1.GoBack();
            }
            if( this.textView1.Visibility == ViewStates.Visible)
            {
                this.iDocFileIndex--;
                if (this.iDocFileIndex < 0)
                {
                    this.iDocFileIndex = 108;
                }
                this.loadDocFile(this.iDocFileIndex);
            }
        }
        private void loadDocFile(int index)
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(Assets.Open("stj/" + index + ".txt")))
            {
                string str = sr.ReadToEnd();
                this.textView1.Text = str;
            }
        }
        private void HideButtonTask_ProcessUpdate(TimerTask.ProcessUpdateEventArgs e)
        {
            if(this.fullButtonsShowing)
            {
                showFullButtons(false);
            }
            else
            {
                //隐藏按钮
                this.buttonHome.ImageAlpha = 255;
                for (int i = 0; i < 100; i++)
                {
                    this.buttonHome.ImageAlpha = 255 - i * 1;
                    System.Threading.Thread.Sleep(10);
                }
                this.buttonHome.ImageAlpha = 150;
            }
        }

        
        private void InitFloatButtonWindow()
        {
            //浮动窗口按钮              
            buttonHome = mFloatLayout.FindViewById<ImageView>(Resource.Id.imageButtonHome);
            buttonHome.Click += ButtonHome_Click; ;
            buttonHome.SetOnTouchListener(this);

            buttonWebView = mFloatLayout.FindViewById<ImageView>(Resource.Id.ImageButtonChrome);
            buttonWebView.Click += buttonWebView_Click;
            buttonDocView = mFloatLayout.FindViewById<ImageView>(Resource.Id.imageButtonViewFile);
            buttonDocView.Click += ButtonDocView_Click; ;
            buttonSettings = mFloatLayout.FindViewById<ImageView>(Resource.Id.imageButtonSettings);
            buttonSettings.Click += ButtonSettings_Click;
            buttonQuit = mFloatLayout.FindViewById<ImageView>(Resource.Id.imageButtonClose);
            buttonQuit.Click += ButtonQuit_Click;
            linearLayoutButton = mFloatLayout.FindViewById<LinearLayout>(Resource.Id.linearLayoutButton);
        }
        public void ResizeFloatButton( int size )
        {
            mWindowManager.RemoveView(mFloatLayout);

            switch (size)
            {
                case 1:
                    this.currentButtonWidth = this.Resources.GetDimensionPixelSize(Resource.Dimension.button_width_s);
                    this.currentButtonHeight = this.Resources.GetDimensionPixelSize(Resource.Dimension.button_height_s);
                    mFloatLayout = (LinearLayout)inflater.Inflate(Resource.Layout.float_window_s, null);
                    break;
                case 2:
                    this.currentButtonWidth = this.Resources.GetDimensionPixelSize(Resource.Dimension.button_width_m);
                    this.currentButtonHeight = this.Resources.GetDimensionPixelSize(Resource.Dimension.button_height_m);
                    mFloatLayout = (LinearLayout)inflater.Inflate(Resource.Layout.float_window_m, null);
                    break;
                case 3:
                    this.currentButtonWidth = this.Resources.GetDimensionPixelSize(Resource.Dimension.button_width_l);
                    this.currentButtonHeight = this.Resources.GetDimensionPixelSize(Resource.Dimension.button_height_l);
                    mFloatLayout = (LinearLayout)inflater.Inflate(Resource.Layout.float_window_l, null);
                    break;
                default:
                    break;
            }

            mWindowManager.AddView(mFloatLayout, wmParamsFloatButton);
            this.InitFloatButtonWindow();
            this.showFullButtons(true);

        }
        private void ButtonCheckUpdate_Click(object sender, EventArgs e)
        {
            CheckUpdate();
        }
        private void CheckUpdate()
        {
            this.textViewUpdateVersion.Text = "正在检查更新...";
            Thread checkVersionThread = new Thread(BeginCheckNewVersion);
            checkVersionThread.Start();
        }

        private void BeginCheckNewVersion()
        {
            try
            {
                string sJson = BocaiHuaAppUtils.HttpRequestGetString(BocaiHuaAppUtils.CHECK_UPDATE_URL);
                if(sJson != null)
                { 
                    var jObj = System.Json.JsonObject.Parse(sJson);
                    Message msg = new Message();
                    msg.Data = new Bundle();
                    msg.Data.PutString("VersionName", jObj["VersionName"].ToString());
                    msg.Data.PutString("VersionCode", jObj["VersionCode"].ToString());
                    msg.Data.PutString("URL", jObj["URL"].ToString().Replace("\"",""));
                    msg.What = AppHandler.CHECK_NEW_VERSION;

                    this.myHandler.SendMessage(msg);
                }
                else
                {
                    Log.Info(TAG, "版本检查失败.");
                }
            }
            catch (Java.Lang.Exception e)
            {
                e.PrintStackTrace();
                Log.Error(Application.Context.PackageName, e, "ERROR");
            }
            catch (System.Exception e)
            {
                Log.Error(Application.Context.PackageName, e.ToString());
            }
            //return null;
        }


        private void ButtonDocView_Click(object sender, EventArgs e)
        {
            this.textView1.Visibility = ViewStates.Visible;
            textView1.Background.Alpha = 255;
            //textView1.Text = DateTime.Now.ToString();
            this.seekBar1.Visibility = ViewStates.Visible;
            this.webView1.Visibility = ViewStates.Invisible;
            this.textView1.Visibility = ViewStates.Visible;

            showFullButtons(false);
            showFullWindow(true);
        }

        private void SeekBar1_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            int alphaValue = e.Progress;
            if (e.Progress >= 255)
            {
                alphaValue = 255;
            }
            if (textView1 != null)
            {
                textView1.Background.Alpha = alphaValue;
            }
            if (webView1 != null)
            {
                if (webView1.Background != null)
                {
                    webView1.Background.Alpha = alphaValue;
                }
                else
                {
                    //webView1.SetBackgroundColor(Color.Argb(alphaValue, WebViewBackgroundColor.R, WebViewBackgroundColor.G, WebViewBackgroundColor.B));
                }
                webView1.Alpha = alphaValue;
            }
        }
        //private Color WebViewBackgroundColor = Color.ParseColor("#ffffffff");
        //private Color TextViewBackgroundColor = Color.ParseColor("#ff000000");
        class BocaiHuaWebViewClient : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
            {
                view.LoadUrl(request.Url.ToString());
                //使用WebView加载显示url
                return false;
            }

        }
        private void ButtonQuit_Click(object sender, EventArgs e)
        {
            showFullButtons(false);
            showFullWindow(false);
            showSettingsWindow(false);
        }
        private void ButtonSettings_Click(object sender, EventArgs e)
        {
            showFullButtons(false);
            showFullWindow(false);
            //BocaiAppUtil.StartUpApplication(this,this.PackageName);
            showSettingsWindow(true);
        }

        private void buttonWebView_Click(object sender, EventArgs e)
        {
            this.seekBar1.Visibility = ViewStates.Visible;
            this.webView1.Visibility = ViewStates.Visible;
            this.textView1.Visibility = ViewStates.Invisible;

            showFullButtons(false);
            showFullWindow(true);
        }
        bool fullWindowShowing = false;
        bool fullButtonsShowing = false;
        private void showFullWindow(bool ifShow)
        {
            if (ifShow)
            {
                if (!fullWindowShowing)
                {
                    mWindowManager.AddView(this.mFullWindowLayout, this.wmParamsView);
                    if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                    {
                        this.seekBar1.SetProgress(230, true);
                        this.seekBar1.SetProgress(240, true);
                    }
                    else
                    {
                        this.seekBar1.Progress = 230;
                        this.seekBar1.Progress = 240;
                    }

                    mWindowManager.RemoveView(this.mFloatLayout);
                    mWindowManager.AddView(this.mFloatLayout, this.wmParamsFloatButton);

                    fullWindowShowing = true;
                }
                if (Settings.Instance.Config.AutoOpenCamera)
                {
                    BocaiHuaAppUtils.StartUpApplication(this, "com.android.camera");
                }
                showSettingsWindow(false);
            }
            else
            {
                if (fullWindowShowing)
                {
                    mWindowManager.RemoveView(this.mFullWindowLayout);
                }
            }
            fullWindowShowing = ifShow;
        }
        public bool settingsWindowShowing = false;
        private void showSettingsWindow(bool ifShow)
        {
            if (ifShow)
            {
                if (!settingsWindowShowing  )
                
                {
                    this.InitSettings(this.mSettingsWindowLayout);
                    mWindowManager.RemoveView(this.mFloatLayout);
                    mWindowManager.AddView(this.mSettingsWindowLayout, this.wmSettingsParams);
                    mWindowManager.AddView(this.mFloatLayout, this.wmParamsFloatButton);

                }
                showFullWindow(false);

                //Intent mIntent = new Intent( this, typeof(SettingsActivity));
                //mIntent.AddCategory("android.intent.category.LAUNCHER");
                //mIntent.SetComponent(this.ComponentName);
                //StartActivity(mIntent);
            }
            else
            {
                if (settingsWindowShowing)
                {
                    mWindowManager.RemoveView(this.mSettingsWindowLayout);
                }
            }
            settingsWindowShowing = ifShow;
        }
        private void InitSettings(View parent)
        {
            this.textViewVersion = parent.FindViewById<TextView>(Resource.Id.textViewVersion);
            this.textViewUpdateVersion = parent.FindViewById<TextView>(Resource.Id.textViewUpdateVersion);
            this.buttonCheckUpdate = parent.FindViewById<Button>(Resource.Id.buttonUpdate);
            this.buttonCheckUpdate.Click += ButtonCheckUpdate_Click;
            this.textViewURL = parent.FindViewById<EditText>(Resource.Id.editTextURL);
            this.textViewURL.Text = Settings.Instance.Config.WebViewURI;
            this.buttonSaveSettings = parent.FindViewById<Button>(Resource.Id.buttonSaveSettings);
            this.buttonSaveSettings.Click += ButtonSaveSettings_Click;
            this.buttonQuitApp = parent.FindViewById<Button>(Resource.Id.buttonQuitApp);
            this.buttonQuitApp.Click += ButtonQuitApp_Click;
            this.btnButtonSize_s = parent.FindViewById<ImageView>(Resource.Id.btnButtonSize_s);
            this.btnButtonSize_m = parent.FindViewById<ImageView>(Resource.Id.btnButtonSize_m);
            this.btnButtonSize_l = parent.FindViewById<ImageView>(Resource.Id.btnButtonSize_l);
            this.btnButtonSize_s.Click += BtnButtonSize_Click;
            this.btnButtonSize_m.Click += BtnButtonSize_Click;
            this.btnButtonSize_l.Click += BtnButtonSize_Click;
            RefreshButtonSizeBySettings();
            this.btnTextColorTheme_1 = parent.FindViewById<TextView>(Resource.Id.btnTextColorTheme_1);
            this.btnTextColorTheme_2 = parent.FindViewById<TextView>(Resource.Id.btnTextColorTheme_2);
            this.btnTextColorTheme_3 = parent.FindViewById<TextView>(Resource.Id.btnTextColorTheme_3);
            this.btnTextColorTheme_UserDefine = parent.FindViewById<TextView>(Resource.Id.btnTextColorTheme_UserDefine);
            this.btnTextColorTheme_1.Click += BtnTextColorTheme_Click;
            this.btnTextColorTheme_2.Click += BtnTextColorTheme_Click;
            this.btnTextColorTheme_3.Click += BtnTextColorTheme_Click;
            this.txtReaderBGColor = parent.FindViewById<TextView>(Resource.Id.txtReaderBGColor);
            this.txtReaderTextColor = parent.FindViewById<TextView>(Resource.Id.txtReaderTextColor);
            this.btnTextSize_s = parent.FindViewById<TextView>(Resource.Id.btnTextSize_s);
            this.btnTextSize_m = parent.FindViewById<TextView>(Resource.Id.btnTextSize_m);
            this.btnTextSize_l = parent.FindViewById<TextView>(Resource.Id.btnTextSize_l);
            this.btnTextSize_s.Click += BtnTextSize_Click;
            this.btnTextSize_m.Click += BtnTextSize_Click;
            this.btnTextSize_l.Click += BtnTextSize_Click;
            this.textViewFloatWindowGrant = parent.FindViewById<TextView>(Resource.Id.textViewFloatWindowGrant);
            this.textViewSDWriteGrant = parent.FindViewById<TextView>(Resource.Id.textViewSDWriteGrant);
            this.textViewGPSGrant = parent.FindViewById<TextView>(Resource.Id.textViewGPSGrant);
            this.textViewHardwareGrant = parent.FindViewById<TextView>(Resource.Id.textViewHardwareGrant);
            this.textViewCameraGrant = parent.FindViewById<TextView>(Resource.Id.textViewCameraGrant);
            
            if (!Android.Provider.Settings.CanDrawOverlays(this))
            {
                this.textViewFloatWindowGrant.Text = "未开启";
                this.textViewFloatWindowGrant.SetTextColor(Color.OrangeRed);
            }
            else
            {
                this.textViewFloatWindowGrant.Text = "已开启";
                this.textViewFloatWindowGrant.SetTextColor(Color.DarkSeaGreen);
            }
            if (this.CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted &&
                this.CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != Permission.Granted
                )
            {
                this.textViewGPSGrant.Text = "未开启";
                this.textViewGPSGrant.SetTextColor(Color.OrangeRed);
            }
            else
            {
                this.textViewGPSGrant.Text = "已开启";
                this.textViewGPSGrant.SetTextColor(Color.DarkSeaGreen);
            }
            if (this.CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Permission.Granted
                )
            {
                this.textViewSDWriteGrant.Text = "未开启";
                this.textViewSDWriteGrant.SetTextColor(Color.OrangeRed);
            }
            else
            {
                this.textViewSDWriteGrant.Text = "已开启";
                this.textViewSDWriteGrant.SetTextColor(Color.DarkSeaGreen);
            }
            if (this.CheckSelfPermission(Manifest.Permission.Camera) != Permission.Granted )
            {
                this.textViewCameraGrant.Text = "未开启";
                this.textViewCameraGrant.SetTextColor(Color.OrangeRed);
            }
            else
            {
                this.textViewCameraGrant.Text = "已开启";
                this.textViewCameraGrant.SetTextColor(Color.DarkSeaGreen);
            }
            Android.Content.PM.PackageInfo pi = this.PackageManager.GetPackageInfo(this.PackageName, Android.Content.PM.PackageInfoFlags.Configurations);
            this.textViewVersion.Text = pi.VersionName + "[" + pi.VersionCode + "]";

            this.seekBarWebViewAlpha = parent.FindViewById<SeekBar>(Resource.Id.seekBarWebViewAlpha);
            this.seekBarWebViewAlpha.Progress = Settings.Instance.Config.WebViewAlpha;
            this.seekBarWebViewAlpha.ProgressChanged += SeekBarWebViewAlpha_ProgressChanged;
            this.seekBarTextViewAlpha = parent.FindViewById<SeekBar>(Resource.Id.seekBarTextViewAlpha);
            this.seekBarTextViewAlpha.Progress = Settings.Instance.Config.TextViewAlpha;
            this.seekBarTextViewAlpha.ProgressChanged += SeekBarTextViewAlpha_ProgressChanged;

            this.chkAutoOpenCameraWhenOpenView = parent.FindViewById<CheckBox>(Resource.Id.chkAutoOpenCameraWhenOpenView);
            this.chkAutoOpenCameraWhenOpenView.Checked = Settings.Instance.Config.AutoOpenCamera;
            this.chkAutoOpenCameraWhenOpenView.CheckedChange += ChkAutoOpenCameraWhenOpenView_CheckedChange;
            this.RefreshTextColorFromSettings();
            this.RefreshTextSizeFromSettings();
        }

        private void ChkAutoOpenCameraWhenOpenView_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Settings.Instance.Config.AutoOpenCamera = e.IsChecked;
        }

        private void SeekBarTextViewAlpha_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            Settings.Instance.Config.TextViewAlpha = e.Progress;
            this.textView1.Alpha = Settings.Instance.Config.TextViewAlpha;
        }

        private void SeekBarWebViewAlpha_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            Settings.Instance.Config.WebViewAlpha = e.Progress;
            this.webView1.Alpha = Settings.Instance.Config.WebViewAlpha;
        }

        private void ButtonQuitApp_Click(object sender, EventArgs e)
        {
            this.Finish();
            Java.Lang.JavaSystem.Exit(0);
        }

        private void ButtonSaveSettings_Click(object sender, EventArgs e)
        {
            Settings.Instance.Config.WebViewURI = this.textViewURL.Text;
            Settings.Instance.Save();
            this.showSettingsWindow(false);
        }
        TimerTask hideButtonTask = new TimerTask();
        
        private void showFullButtons(bool ifShow)
        {
            if (ifShow)
            {
                hideButtonTask.WaitTime = 20 * 1000;
                wmParamsFloatButton.Width = (this.currentButtonWidth +5) * 5;
                wmParamsFloatButton.Height = this.currentButtonHeight;
            }
            else
            {
                hideButtonTask.WaitTime = 5 * 1000;
                wmParamsFloatButton.Width = this.currentButtonWidth;
                wmParamsFloatButton.Height = this.currentButtonHeight;
            }
            hideButtonTask.Reset();
            this.fullButtonsShowing = ifShow;
            mWindowManager.UpdateViewLayout(mFloatLayout, wmParamsFloatButton);
        }

        public int currentButtonWidth = 50;
        public int currentButtonHeight = 50;
        private void ButtonHome_Click(object sender, EventArgs e)
        {
            try
            {
                //ImageView ivTest1 = this.mFloatLayout.FindViewById<ImageView>(Resource.Id.imageButtonHome);
                showFullButtons(!this.fullButtonsShowing);
                this.buttonHome.Layout(0, 0, this.currentButtonWidth * 2, this.currentButtonHeight * 2);
            }
            catch (Java.Lang.Exception ex)
            {
                Log.Error(TAG, ex, "Error on ButtonHome_Click");
            }
        }        
        #endregion
        #region 移除Android悬浮框        
        /// <summary>        
        /// 移除Android悬浮框        
        /// </summary>        
        private void CloseFloatWindow()
        {
            if (mFloatLayout != null)
            {
                //移除悬浮窗口                  
                mWindowManager.RemoveView(mFloatLayout);
            }
            this.Finish();
        }
        #endregion
        private static int REQUEST_CODE_ADDRESS = 100;
        public static MainActivity CurrentInstance;
        
        private void CheckPermissioinAll()
        {
            Android.Locations.LocationManager locationManager= (Android.Locations.LocationManager)this.GetSystemService(Context.LocationService);
            System.Collections.Generic.IList<string> providerList = locationManager.GetProviders(true);
            string provider;
            if (providerList.Contains(LocationManager.GpsProvider))
            {
                provider = LocationManager.GpsProvider;
            }
            else if (providerList.Contains(LocationManager.NetworkProvider))
            {
                provider = LocationManager.NetworkProvider;
            }
            else
            {
                Toast.MakeText(this, "No location provider to use", ToastLength.Short).Show();
                Log.Warn(TAG, "No location provider to use");
                return;
            }
            if( provider != null)
            {
                Location location = locationManager.GetLastKnownLocation(provider);
                string currentPosition = "latitude is " + location.Latitude + "\n"
                    + "longitude is " + location.Longitude;

                Toast.MakeText(this, "Location:" + currentPosition.ToString(), ToastLength.Long).Show();
                Log.Info(TAG, "Location:" + currentPosition.ToString());
            }
            string[] permissionArray = { 
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation,
                Manifest.Permission.ReadExternalStorage,
                Manifest.Permission.WriteExternalStorage,
                Manifest.Permission.SystemAlertWindow,
                Manifest.Permission.Camera,
                Manifest.Permission.ReadPhoneState
            };
                        
            System.Collections.Generic.List<string> permissionList = new System.Collections.Generic.List<string>();
            foreach(string permission in permissionArray)
            {
                if( this.CheckSelfPermission(permission) != Permission.Granted)
                {
                    permissionList.Add(permission);
                }
            }
            ActivityCompat.RequestPermissions(this, permissionList.ToArray(), REQUEST_CODE_ADDRESS);//申请授权
        }
        private LogcatHelper mLogcatHelper = null;
        /* 自动拍照
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == takeCode)
            {
                if (data != null)
                {
                    // 如果不指定图片保存地址，那么将会返回照片的缩略图，返回的照片大小是被压缩过的。
                    if (data.HasExtra("data"))
                    {
                        try
                        {
                            string filepath = "";
                            if (Android.OS.Environment.ExternalStorageState == Android.OS.Environment.MediaMounted)
                            {// 优先保存到SD卡中
                                filepath = this.GetExternalFilesDir("pic").AbsolutePath;
                            }
                            else
                            {// 如果SD卡不存在，就保存到本应用的目录下
                                filepath = this.FilesDir.AbsolutePath +
                                    File.Separator + this.PackageName +
                                    File.Separator + "pic"; ;
                            }
                            File file = new File(filepath);
                            if (!file.Exists())
                            {
                                file.Mkdirs();
                            }


                            Bitmap bitmap = (Bitmap)data.GetParcelableExtra("data");
                            System.IO.FileStream fos = new System.IO.FileStream(filepath + File.Separator + DateTime.Now.ToFileTime().ToString() + ".png", System.IO.FileMode.CreateNew);
                            bitmap.Compress(Bitmap.CompressFormat.Png, 90, fos);
                            fos.Flush();
                            fos.Close();
                            Log.Info( TAG, "已经保存");
                        }
                        catch (FileNotFoundException e)
                        {
                            e.PrintStackTrace();
                        }
                        catch (IOException e)
                        {
                            // TODO Auto-generated catch block
                            e.PrintStackTrace();
                        }
                        //imageView1.setImageBitmap(bitmap);
                        //BitmapFactory.de
                    }
                }
                else
                {
                    // 如果给照相机指定了图片存储位置，那么返回的data是null的，
                    //需要从图片路径加载图片。生成缩略图，使用这种办法可以防止返回的缩略图尺寸太小的关系
                    //int width = imageView1.getWidth();
                    //int height = imageView1.getHeight();

                    BitmapFactory.Options opts = new BitmapFactory.Options();

                    int imageWidth = opts.OutWidth;
                    int imageHeight = opts.OutHeight;

                    // 图像缩小比例
                    //int scale = Math.min(imageWidth / width, imageHeight / height);

                    // 把图像大小设置成imageview 大小
                    opts.InJustDecodeBounds = false;
                    opts.InSampleSize = 1;
                    opts.InPurgeable = true;

                    Bitmap bitmap = BitmapFactory.DecodeFile(outPutFile.Path, opts);

                    //imageView1.setImageBitmap(bitmap);
                }

            }

        }`
        */
        //private Android.Net.Uri outPutFile = null;//
        AppHandler myHandler = new AppHandler();
        private static bool OnFirstCreate = true;
        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            try
            {
                mWindowManager.RemoveView(this.mFloatLayout);
            }
            catch { }
            try
            {
                mWindowManager.RemoveView(this.mFullWindowLayout);
            }
            catch { }
            try
            {
                mWindowManager.RemoveView(this.mSettingsWindowLayout);
            }
            catch { }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            if (OnFirstCreate)
            {
                new Thread(delegate () {
                    try
                    {
                        //获取保存在sd中的 设备唯一标识符
                        if ( System.String.IsNullOrEmpty(Settings.Instance.Config.DeviceID))
                        {
                            Settings.Instance.Config.DeviceID = BocaiHuaAppUtils.getDeviceId(this);
                            Settings.Instance.Save();
                        }
                        //服务器登记
                        string sResult = BocaiHuaAppUtils.HttpRequestGetString(BocaiHuaAppUtils.APPSERVICE_URL, "POST",
                            new System.Collections.Generic.KeyValuePair<string, string>("Action", "Login"),
                            new System.Collections.Generic.KeyValuePair<string, string>("DeviceID", Settings.Instance.Config.DeviceID)
                            );
                    }
                    catch (Java.Lang.Exception e)
                    {
                        e.PrintStackTrace();
                        Log.Error(Application.Context.PackageName, e, "ERROR");
                    }
                    catch (System.Exception e)
                    {
                        Log.Error(Application.Context.PackageName, e.ToString());
                    }
                }).Start();
        }
        OnFirstCreate = false;
            try
            {
                TAG = this.PackageName;
                myHandler.MessageHandleEvent += MyHandler_MessageHandleEvent;
                mLogcatHelper = LogcatHelper.CreateInstance(this.ApplicationContext,"*");
                Log.Wtf(TAG, "BocaiApp Starting");
                //权限检查
                if(OnFirstCreate)
                {
                    this.CheckPermissioinAll();
                }

                MainActivity.CurrentInstance = this;
                
                this.SetContentView(Resource.Layout.settings_window);
                this.mSettingsWindowLayout = this.Window.DecorView;

                this.createFloatView();
                this.InitSettings(this.Window.DecorView);
                if (OnFirstCreate)
                {
                    this.CheckUpdate();

                    if (!Android.Provider.Settings.CanDrawOverlays(this))
                    {
                        Toast.MakeText(this, "未获得悬浮窗权限.", ToastLength.Short).Show();
                        Log.Info(TAG, "未获得悬浮窗权限.");
                    }
                    else
                    {
                        Intent home = new Intent(Intent.ActionMain);
                        home.SetFlags(ActivityFlags.ClearTop);
                        home.AddCategory("android.intent.category.HOME");
                        this.StartActivity(home);
                    }
                }
            }
            catch (Java.Lang.Exception e)
            {
                e.PrintStackTrace();
                Log.Error(Application.Context.PackageName, e, "ERROR");
            }
            catch (System.Exception e)
            {
                Log.Error(Application.Context.PackageName, e.ToString());
            }
        }

        private void MyHandler_MessageHandleEvent(Message msg)
        {
            switch(msg.What )
            {
                case AppHandler.CHECK_NEW_VERSION:
                    this.textViewUpdateVersion.Text = msg.Data.GetString("VersionName") + "[" + msg.Data.GetString("VersionCode") + "]";
                    UpdateManager um = new UpdateManager(this, msg.Data.GetString("URL"));
                    um.checkUpdateInfo();
                    break;
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            bool ifresult =Log.IsLoggable(TAG, LogPriority.Error);
            foreach (string permission in permissions)
            {
                if(permission == Manifest.Permission.WriteExternalStorage)
                {
                    //if(grantResults.GetEnumerator().)
                }
                //如果返回true表示用户点了禁止获取权限，但没有勾选不再提示。
                //返回false表示用户点了禁止获取权限，并勾选不再提示。
                //我们可以通过该方法判断是否要继续申请权限
                bool shouldShow = this.ShouldShowRequestPermissionRationale( permission);
            }

        }
        DateTime mouseDownTime = DateTime.Now;
        private int oldPosX = 0;
        private int oldPosY = 0;
        private WindowManagerLayoutParams _wmParamsViewOLD = new WindowManagerLayoutParams();
        private void ButtonWindowsBottom_Touch(object sender, View.TouchEventArgs e)
        {
            int viewID = (sender as View).Id;
            switch (e.Event.Action)
            {
                case MotionEventActions.Up:
                    if ((DateTime.Now - mouseDownTime).TotalMilliseconds < 200)
                    {
                        mWindowManager.UpdateViewLayout(this.mFullWindowLayout, _wmParamsViewOLD);
                    }
                    else
                    {
                    }
                    break;
                case MotionEventActions.Down:
                    if (this.fullwindow_x == -1)
                    {
                        this.fullwindow_x = 0;
                        this.fullwindow_width = this.mFullWindowLayout.Width;
                        this.fullwindow_height = this.mFullWindowLayout.Height;
                    }
                    _viewX = e.Event.GetX();
                    _viewY = e.Event.GetY();
                    _wmParamsViewOLD.CopyFrom( wmParamsView);
                    mouseDownTime = DateTime.Now;
                    break;
                case MotionEventActions.Move:
                    var top = (int)(e.Event.RawY - _viewY - GetStateBarHeight());
                    var left = (int)(e.Event.RawX - _viewX);
                    
                    switch( viewID)
                    {
                        case Resource.Id.buttonWindowsLeft:
                            wmParamsView.X = left - 10;
                            if( wmParamsView.X<20)
                            {
                                wmParamsView.X = 0;
                            }
                            if(_wmParamsViewOLD.Width < 0)
                            {
                                wmParamsView.Width = fullwindow_width + _wmParamsViewOLD.X - wmParamsView.X;
                            }
                            else
                            {
                                wmParamsView.Width = _wmParamsViewOLD.Width + _wmParamsViewOLD.X - wmParamsView.X;
                            }
                            break;
                        case Resource.Id.buttonWindowsRight:
                            if (Java.Lang.Math.Abs(left + 20 - fullwindow_width) < 20)
                            {
                                wmParamsView.Width = fullwindow_width - wmParamsView.X;
                            }
                            else 
                            {
                                wmParamsView.Width = left + 20 - wmParamsView.X;
                            }
                            break;
                        case Resource.Id.buttonWindowsTop:
                            wmParamsView.Y = top - 10;
                            if (wmParamsView.Y < 20)
                            {
                                wmParamsView.Y = 0;
                            }
                            if(_wmParamsViewOLD.Height< 0)
                            {
                                wmParamsView.Height = fullwindow_height + _wmParamsViewOLD.Y - wmParamsView.Y;
                            }
                            else
                            {
                                wmParamsView.Height = _wmParamsViewOLD.Height + _wmParamsViewOLD.Y - wmParamsView.Y;
                            }
                            break;
                        case Resource.Id.buttonWindowsBottom:
                            wmParamsView.Height = top + 20 - wmParamsView.Y;
                            if (Java.Lang.Math.Abs(wmParamsView.Height + wmParamsView.Y - fullwindow_height) < 20)
                            {
                                wmParamsView.Height = fullwindow_height - wmParamsView.Y;
                            }
                            break;
                    }
                    
                    Log.Debug(TAG ,"height:" + wmParamsView.Height);
                    mWindowManager.UpdateViewLayout(this.mFullWindowLayout, wmParamsView);
                    break;
            }
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            this.buttonHome.ImageAlpha = 255;
            switch (e.Action)
            {
                case MotionEventActions.Up:
                    if ((DateTime.Now - mouseDownTime).TotalMilliseconds < 200)
                    {
                        v.CallOnClick();
                        wmParamsFloatButton.X = oldPosX;
                        wmParamsFloatButton.Y = oldPosY;
                        mWindowManager.UpdateViewLayout(mFloatLayout, wmParamsFloatButton);
                    }
                    else
                    {
                    }
                    this.hideButtonTask.Reset();
                    break;

                case MotionEventActions.Down:
                    _viewX = e.GetX();
                    _viewY = e.GetY();
                    Log.Debug(TAG,"Down:" + _viewX + "," + _viewY);
                    oldPosX = wmParamsFloatButton.X;
                    oldPosY = wmParamsFloatButton.Y;
                    mouseDownTime = DateTime.Now;
                    break;
                case MotionEventActions.Move:
                    Log.Debug(TAG,"Move:" + e.RawX+ "," + e.RawY);
                    var left = (int)(e.RawX - _viewX);
                    //var right = (int)(left + v.Width);
                    var top = (int)(e.RawY - _viewY - GetStateBarHeight());
                    //var bottom = (int)(top + v.Height);
                    wmParamsFloatButton.X = left;
                    wmParamsFloatButton.Y = top;
                    mWindowManager.UpdateViewLayout(mFloatLayout, wmParamsFloatButton);
                    break;
            }
            return true;
        }
        int _StatusBarHeight = 0;
        private int GetStateBarHeight()
        {
            if( _StatusBarHeight == 0)
            {
                int resourceId = this.Resources.GetIdentifier("status_bar_height", "dimen", "android");
                if (resourceId > 0)
                {
                    _StatusBarHeight = this.Resources.GetDimensionPixelSize(resourceId);
                }
            }
            if(_StatusBarHeight == 0)
            {
                _StatusBarHeight = 20;
            }
            return _StatusBarHeight;
        }
    }
}