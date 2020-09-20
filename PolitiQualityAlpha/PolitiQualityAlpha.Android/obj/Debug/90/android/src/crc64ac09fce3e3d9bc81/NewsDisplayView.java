package crc64ac09fce3e3d9bc81;


public class NewsDisplayView
	extends crc6466d8e86b1ec8bfa8.MvxActivity_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("PolitiQualityAlpha.Droid.View.NewsDisplayView, PolitiQualityAlpha.Android", NewsDisplayView.class, __md_methods);
	}


	public NewsDisplayView ()
	{
		super ();
		if (getClass () == NewsDisplayView.class)
			mono.android.TypeManager.Activate ("PolitiQualityAlpha.Droid.View.NewsDisplayView, PolitiQualityAlpha.Android", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
