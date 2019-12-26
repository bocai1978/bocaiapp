package crc64775064edecbadd42;


public class FloatLayout
	extends android.widget.RelativeLayout
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnDragListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDrag:(Landroid/view/View;Landroid/view/DragEvent;)Z:GetOnDrag_Landroid_view_View_Landroid_view_DragEvent_Handler:Android.Views.View/IOnDragListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Com.Bocaihua.APP.FloatLayout, BocaiApp1", FloatLayout.class, __md_methods);
	}


	public FloatLayout (android.content.Context p0)
	{
		super (p0);
		if (getClass () == FloatLayout.class)
			mono.android.TypeManager.Activate ("Com.Bocaihua.APP.FloatLayout, BocaiApp1", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public FloatLayout (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == FloatLayout.class)
			mono.android.TypeManager.Activate ("Com.Bocaihua.APP.FloatLayout, BocaiApp1", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public FloatLayout (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == FloatLayout.class)
			mono.android.TypeManager.Activate ("Com.Bocaihua.APP.FloatLayout, BocaiApp1", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public FloatLayout (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == FloatLayout.class)
			mono.android.TypeManager.Activate ("Com.Bocaihua.APP.FloatLayout, BocaiApp1", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public boolean onDrag (android.view.View p0, android.view.DragEvent p1)
	{
		return n_onDrag (p0, p1);
	}

	private native boolean n_onDrag (android.view.View p0, android.view.DragEvent p1);

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
