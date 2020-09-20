package crc64af5cc6b280ec52ba;


public class HomepageView
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
		mono.android.Runtime.register ("PolitiQuality.Droid.View.HomepageView, PolitiQualityAlpha.Android", HomepageView.class, __md_methods);
	}


	public HomepageView ()
	{
		super ();
		if (getClass () == HomepageView.class)
			mono.android.TypeManager.Activate ("PolitiQuality.Droid.View.HomepageView, PolitiQualityAlpha.Android", "", this, new java.lang.Object[] {  });
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
