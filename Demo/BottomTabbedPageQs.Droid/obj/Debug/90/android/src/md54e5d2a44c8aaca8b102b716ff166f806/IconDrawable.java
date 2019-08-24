package md54e5d2a44c8aaca8b102b716ff166f806;


public class IconDrawable
	extends android.graphics.drawable.Drawable
	implements
		mono.android.IGCUserPeer,
		android.support.v4.graphics.drawable.TintAwareDrawable
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getIntrinsicHeight:()I:GetGetIntrinsicHeightHandler\n" +
			"n_getIntrinsicWidth:()I:GetGetIntrinsicWidthHandler\n" +
			"n_isStateful:()Z:GetIsStatefulHandler\n" +
			"n_getOpacity:()I:GetGetOpacityHandler\n" +
			"n_clearColorFilter:()V:GetClearColorFilterHandler\n" +
			"n_draw:(Landroid/graphics/Canvas;)V:GetDraw_Landroid_graphics_Canvas_Handler\n" +
			"n_setState:([I)Z:GetSetState_arrayIHandler\n" +
			"n_setAlpha:(I)V:GetSetAlpha_IHandler\n" +
			"n_setColorFilter:(Landroid/graphics/ColorFilter;)V:GetSetColorFilter_Landroid_graphics_ColorFilter_Handler\n" +
			"n_setTint:(I)V:GetSetTint_IHandler\n" +
			"n_setTintList:(Landroid/content/res/ColorStateList;)V:GetSetTintList_Landroid_content_res_ColorStateList_Handler\n" +
			"n_setTintMode:(Landroid/graphics/PorterDuff$Mode;)V:GetSetTintMode_Landroid_graphics_PorterDuff_Mode_Handler:Android.Support.V4.Graphics.Drawable.ITintAwareDrawableInvoker, Xamarin.Android.Support.Compat\n" +
			"";
		mono.android.Runtime.register ("Plugin.Iconize.IconDrawable, Plugin.Iconize", IconDrawable.class, __md_methods);
	}


	public IconDrawable ()
	{
		super ();
		if (getClass () == IconDrawable.class)
			mono.android.TypeManager.Activate ("Plugin.Iconize.IconDrawable, Plugin.Iconize", "", this, new java.lang.Object[] {  });
	}

	public IconDrawable (android.content.Context p0, java.lang.String p1)
	{
		super ();
		if (getClass () == IconDrawable.class)
			mono.android.TypeManager.Activate ("Plugin.Iconize.IconDrawable, Plugin.Iconize", "Android.Content.Context, Mono.Android:System.String, mscorlib", this, new java.lang.Object[] { p0, p1 });
	}


	public int getIntrinsicHeight ()
	{
		return n_getIntrinsicHeight ();
	}

	private native int n_getIntrinsicHeight ();


	public int getIntrinsicWidth ()
	{
		return n_getIntrinsicWidth ();
	}

	private native int n_getIntrinsicWidth ();


	public boolean isStateful ()
	{
		return n_isStateful ();
	}

	private native boolean n_isStateful ();


	public int getOpacity ()
	{
		return n_getOpacity ();
	}

	private native int n_getOpacity ();


	public void clearColorFilter ()
	{
		n_clearColorFilter ();
	}

	private native void n_clearColorFilter ();


	public void draw (android.graphics.Canvas p0)
	{
		n_draw (p0);
	}

	private native void n_draw (android.graphics.Canvas p0);


	public boolean setState (int[] p0)
	{
		return n_setState (p0);
	}

	private native boolean n_setState (int[] p0);


	public void setAlpha (int p0)
	{
		n_setAlpha (p0);
	}

	private native void n_setAlpha (int p0);


	public void setColorFilter (android.graphics.ColorFilter p0)
	{
		n_setColorFilter (p0);
	}

	private native void n_setColorFilter (android.graphics.ColorFilter p0);


	public void setTint (int p0)
	{
		n_setTint (p0);
	}

	private native void n_setTint (int p0);


	public void setTintList (android.content.res.ColorStateList p0)
	{
		n_setTintList (p0);
	}

	private native void n_setTintList (android.content.res.ColorStateList p0);


	public void setTintMode (android.graphics.PorterDuff.Mode p0)
	{
		n_setTintMode (p0);
	}

	private native void n_setTintMode (android.graphics.PorterDuff.Mode p0);

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
