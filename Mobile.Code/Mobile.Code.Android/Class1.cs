using Android.Graphics;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(Mobile.Code.Droid.ScreenshotManager))]
namespace Mobile.Code.Droid
{
    public class ScreenshotManager : IScreenshotManager
    {


        public async System.Threading.Tasks.Task<byte[]> CaptureAsync()
        {
            var activity = Xamarin.Forms.Forms.Context as MainActivity;
            if (activity == null)
            {
                return null;
            }

            var view = activity.Window.DecorView;

            view.DrawingCacheEnabled = true;

            Bitmap bitmap = view.GetDrawingCache(true);

            byte[] bitmapData;

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }

            return bitmapData;
        }
    }
}