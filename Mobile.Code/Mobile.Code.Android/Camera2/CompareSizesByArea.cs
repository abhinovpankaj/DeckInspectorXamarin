using Java.Lang;
using Java.Util;
using Object = Java.Lang.Object;
using Size = Android.Util.Size;

namespace Mobile.Code.Droid.Camera2
{
    public class CompareSizesByArea : Object, IComparator
    {
        public int Compare(Object lhs, Object rhs)
        {
            var lhsSize = (Size)lhs;
            var rhsSize = (Size)rhs;
            // We cast here to ensure the multiplications won't overflow
            return Long.Signum((long)lhsSize.Width * lhsSize.Height - (long)rhsSize.Width * rhsSize.Height);
        }
    }
}