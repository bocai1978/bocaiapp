package crc64850e31a5a51c0ce8;


public class UpdateManager_MyHandler
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
		mono.android.Runtime.register ("Com.Bocaihua.APP.UpdateManager+MyHandler, com.bocaihua.app.smartAssistant", UpdateManager_MyHandler.class, __md_methods);
	}


	public UpdateManager_MyHandler ()
	{
		super ();
		if (getClass () == UpdateManager_MyHandler.class)
			mono.android.TypeManager.Activate ("Com.Bocaihua.APP.UpdateManager+MyHandler, com.bocaihua.app.smartAssistant", "", this, new java.lang.Object[] {  });
	}


	public UpdateManager_MyHandler (android.os.Looper p0)
	{
		super (p0);
		if (getClass () == UpdateManager_MyHandler.class)
			mono.android.TypeManager.Activate ("Com.Bocaihua.APP.UpdateManager+MyHandler, com.bocaihua.app.smartAssistant", "Android.OS.Looper, Mono.Android", this, new java.lang.Object[] { p0 });
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
