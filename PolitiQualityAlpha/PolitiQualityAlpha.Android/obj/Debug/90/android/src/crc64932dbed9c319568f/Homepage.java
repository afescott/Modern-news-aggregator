package crc64932dbed9c319568f;


public class Homepage
	extends mvvmcross.platforms.android.views.MvxSplashScreenActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("PolitiQuality.Droid.Homepage, PolitiQualityAlpha.Android", Homepage.class, __md_methods);
	}


	public Homepage ()
	{
		super ();
		if (getClass () == Homepage.class)
			mono.android.TypeManager.Activate ("PolitiQuality.Droid.Homepage, PolitiQualityAlpha.Android", "", this, new java.lang.Object[] {  });
	}

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
