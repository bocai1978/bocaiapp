package crc64850e31a5a51c0ce8;


public class LogcatHelper_LogDumper
	extends java.lang.Thread
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_run:()V:GetRunHandler\n" +
			"";
		mono.android.Runtime.register ("Com.Bocaihua.APP.LogcatHelper+LogDumper, com.bocaihua.app.smartAssistant", LogcatHelper_LogDumper.class, __md_methods);
	}


	public LogcatHelper_LogDumper ()
	{
		super ();
		if (getClass () == LogcatHelper_LogDumper.class)
			mono.android.TypeManager.Activate ("Com.Bocaihua.APP.LogcatHelper+LogDumper, com.bocaihua.app.smartAssistant", "", this, new java.lang.Object[] {  });
	}


	public LogcatHelper_LogDumper (java.lang.Runnable p0)
	{
		super (p0);
		if (getClass () == LogcatHelper_LogDumper.class)
			mono.android.TypeManager.Activate ("Com.Bocaihua.APP.LogcatHelper+LogDumper, com.bocaihua.app.smartAssistant", "Java.Lang.IRunnable, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	public LogcatHelper_LogDumper (java.lang.String p0, java.lang.String p1, java.lang.String p2)
	{
		super ();
		if (getClass () == LogcatHelper_LogDumper.class)
			mono.android.TypeManager.Activate ("Com.Bocaihua.APP.LogcatHelper+LogDumper, com.bocaihua.app.smartAssistant", "System.String, mscorlib:System.String, mscorlib:System.String, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void run ()
	{
		n_run ();
	}

	private native void n_run ();

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
