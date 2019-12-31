package crc64850e31a5a51c0ce8;


public class AppHandler
	extends android.os.Handler
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_handleMessage:(Landroid/os/Message;)V:GetHandleMessage_Landroid_os_Message_Handler\n" +
			"";
		mono.android.Runtime.register ("Com.Bocaihua.APP.AppHandler, com.bocaihua.app.smartAssistant", AppHandler.class, __md_methods);
	}


	public AppHandler ()
	{
		super ();
		if (getClass () == AppHandler.class)
			mono.android.TypeManager.Activate ("Com.Bocaihua.APP.AppHandler, com.bocaihua.app.smartAssistant", "", this, new java.lang.Object[] {  });
	}


	public AppHandler (android.os.Looper p0)
	{
		super (p0);
		if (getClass () == AppHandler.class)
			mono.android.TypeManager.Activate ("Com.Bocaihua.APP.AppHandler, com.bocaihua.app.smartAssistant", "Android.OS.Looper, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public void handleMessage (android.os.Message p0)
	{
		n_handleMessage (p0);
	}

	private native void n_handleMessage (android.os.Message p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
