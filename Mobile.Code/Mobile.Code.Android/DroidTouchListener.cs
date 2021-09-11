using Android.Views;

namespace Mobile.Code.Droid
{
    public class DroidTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        public bool OnTouch(View v, MotionEvent e)
        {
            v.Parent?.RequestDisallowInterceptTouchEvent(true);
            if ((e.Action & MotionEventActions.Up) != 0 && (e.ActionMasked & MotionEventActions.Up) != 0)
            {
                v.Parent?.RequestDisallowInterceptTouchEvent(false);
            }
            return false;
        }
    }
}