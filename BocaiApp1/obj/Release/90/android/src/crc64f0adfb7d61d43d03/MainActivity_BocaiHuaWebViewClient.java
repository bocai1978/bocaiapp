package crc64f0adfb7d61d43d03;


public class MainActivity_BocaiHuaWebViewClient
	extends android.webkit.WebViewClient
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_shouldOverrideUrlLoading:(Landroid/webkit/WebView;Landroid/webkit/WebResourceRequest;)Z:GetShouldOverrideUrlLoading_Landroid_webkit_WebView_Landroid_webkit_WebResourceRequest_Handler\n" +
			"";
		mono.android.Runtime.register ("BocaiApp1.MainActivity+BocaiHuaWebViewClient, BocaiApp1", MainActivity_BocaiHuaWebViewClient.class, __md_methods);
	}


	public MainActivity_BocaiHuaWebViewClient ()
	{
		super ();
		if (getClass () == MainActivity_BocaiHuaWebViewClient.class)
			mono.android.TypeManager.Activate ("BocaiApp1.MainActivity+BocaiHuaWebViewClient, BocaiApp1", "", this, new java.lang.Object[] {  });
	}


	public boolean shouldOverrideUrlLoading (android.webkit.WebView p0, android.webkit.WebResourceRequest p1)
	{
		return n_shouldOverrideUrlLoading (p0, p1);
	}

	private native boolean n_shouldOverrideUrlLoading (android.webkit.WebView p0, android.webkit.WebResourceRequest p1);

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
