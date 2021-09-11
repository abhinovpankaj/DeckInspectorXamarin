using Android.Graphics;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(Mobile.Code.Droid.ResizeImage))]
namespace Mobile.Code.Droid
{
    public class ResizeImage : IImageService
    {
        public byte[] ResizeTheImage(byte[] imageData, float width, float height)
        {
            imageData = CropImage(imageData, 20);
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            // originalImage = getResizedBitmap(originalImage);
            //  Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);
            int imageWidth = originalImage.Width;
            int imageHeight = originalImage.Height;
            int newHeight = (imageHeight * 800) / imageWidth;

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, 2000, newHeight, false);


            using MemoryStream ms = new MemoryStream();
            resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);

            return ms.ToArray();
        }
        public Bitmap getResizedBitmap(Bitmap bm)
        {
            int width = bm.Width;
            int height = bm.Height;

            //int narrowSize = Math.Min(width, height);
            //int differ = (int)Math.Abs((bm.Height - bm.Width) / 4.0f);

            //width = (width == narrowSize) ? 0 : differ;
            //height = (width == 0) ? differ : 0;
            Matrix matrix = new Matrix();
            matrix.PostScale(0.5f, 0.5f);
            // Bitmap croppedBitmap = Bitmap.createBitmap(bitmapOriginal, 100, 100, 100, 100, matrix, true);
            Bitmap resizedBitmap = Bitmap.CreateBitmap(bm, bm.Width, bm.Height, bm.Width, bm.Height, matrix, true);
            bm.Recycle();
            return resizedBitmap;
        }
        private byte[] CropImage(byte[] screenshotBytes, int top)
        {
            Android.Graphics.Bitmap bitmap = Android.Graphics.BitmapFactory.DecodeByteArray(
              screenshotBytes, 0, screenshotBytes.Length);

            // int viewStartY = (int)(top * 2.8f);
            // int viewHeight = (int)(bitmap.Height - (top * 2.8f));
            //// var navBarXY = 20;
            // int viewHeightMinusNavBar = viewHeight - 20;

            int height = bitmap.Height - 120;
            height = height - 120;
            height = height > 0 ? height : bitmap.Height;
            Android.Graphics.Bitmap crop = Android.Graphics.Bitmap.CreateBitmap(bitmap, 0, 120, bitmap.Width, height);

            //Android.Graphics.Bitmap crop = Android.Graphics.Bitmap.CreateBitmap(bitmap,
            //    0, 120,
            //    bitmap.Width, bitmap.Height-140
            //    );

            bitmap.Dispose();

            using MemoryStream stream = new MemoryStream();
            crop.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, stream);
            return stream.ToArray();
        }
    }
    //    public  Bitmap resizeBitmapImageForFitSquare(Bitmap image, int maxResolution)
    //    {

    //        if (maxResolution <= 0)
    //            return image;

    //        int width = image.Width;
    //        int height = image.Height;
    //        float ratio = (width >= height) ? (float)maxResolution / width : (float)maxResolution / height;

    //        int finalWidth = (int)((float)width * ratio);
    //        int finalHeight = (int)((float)height * ratio);

    //        image = Bitmap.CreateScaledBitmap(image, finalWidth, finalHeight, true);

    //        if (image.getWidth() == image.getHeight())
    //            return image;
    //        else
    //        {
    //            //fit height and width
    //            int left = 0;
    //            int top = 0;

    //            if (image.getWidth() != maxResolution)
    //                left = (maxResolution - image.getWidth()) / 2;

    //            if (image.getHeight() != maxResolution)
    //                top = (maxResolution - image.getHeight()) / 2;

    //            Bitmap bitmap = Bitmap.createBitmap(maxResolution, maxResolution, Bitmap.Config.ARGB_8888);
    //            Canvas canvas = new Canvas(bitmap);
    //            canvas.drawBitmap(image, left, top, null);
    //            canvas.save();
    //            canvas.restore();

    //            return bitmap;
    //        }
    //    }

    //}
}