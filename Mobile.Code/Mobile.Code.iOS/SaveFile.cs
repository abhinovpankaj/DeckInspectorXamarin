using CoreGraphics;
using Foundation;
using Mobile.Code.iOS;
using QuickLook;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using UIKit;
[assembly: Xamarin.Forms.Dependency(typeof(SaveFile))]
namespace Mobile.Code.iOS
{
    class SaveFile : ISaveFile
    {
        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            //
            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
        public async Task<string> SaveFiles(string filename, byte[] bytes)
        {

            UIImage originalImage = ImageFromByteArray(bytes);

            var bytesImagen = originalImage.AsJPEG().ToArray();
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var filePath = Path.Combine(documentsPath, filename);
            File.WriteAllBytes(filePath, bytes);
            // OpenPDF(filePath);
            return await Task.FromResult(filePath);

        }

        public void OpenPDF(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);

            QLPreviewController previewController = new QLPreviewController();
            previewController.DataSource = new PDFPreviewControllerDataSource(fi.FullName, fi.Name);

            UINavigationController controller = FindNavigationController();
            if (controller != null)
                controller.PresentViewController(previewController, true, null);
        }

        private UINavigationController FindNavigationController()
        {
            foreach (var window in UIApplication.SharedApplication.Windows)
            {
                if (window.RootViewController.NavigationController != null)
                    return window.RootViewController.NavigationController;
                else
                {
                    UINavigationController val = CheckSubs(window.RootViewController.ChildViewControllers);
                    if (val != null)
                        return val;
                }
            }

            return null;
        }

        private UINavigationController CheckSubs(UIViewController[] controllers)
        {
            foreach (var controller in controllers)
            {
                if (controller.NavigationController != null)
                    return controller.NavigationController;
                else
                {
                    UINavigationController val = CheckSubs(controller.ChildViewControllers);
                    if (val != null)
                        return val;
                }
            }
            return null;
        }

        public async Task<string> SaveFilesForCameraApi(string filename, byte[] bytes)
        {
            UIImage originalImage = ImageFromByteArray(bytes);


            UIDevice device = UIDevice.CurrentDevice;
            UIDeviceOrientation orientation = device.Orientation;

            var photo = new UIImage();



            switch (orientation)
            {
                case UIDeviceOrientation.PortraitUpsideDown:

                    photo = originalImage;
                    //  photo = new UIImage(originalImage.CGImage, 1.0f, UIImageOrientation.Up);
                    // UpdatePreviewLayer(previewLayerConnection,
                    // AVCaptureVideoOrientation.Portrait);
                    break;
                case UIDeviceOrientation.LandscapeLeft:
                    photo = new UIImage(originalImage.CGImage, photo.CurrentScale, UIImageOrientation.Up);
                    // UpdatePreviewLayer(previewLayerConnection,
                    // AVCaptureVideoOrientation.Portrait);
                    break;
                case UIDeviceOrientation.LandscapeRight:
                    //    photo = new UIImage(image.CGImage, 1.0f, UIImageOrientation.Down);

                    var temp12 = new UIImage(originalImage.CGImage, photo.CurrentScale, UIImageOrientation.Down);

                    photo = ScaleAndRotateImage(temp12, UIImageOrientation.Down);
                    break;

                //UpdatePreviewLayer(previewLayerConnection,
                // AVCaptureVideoOrientation.Portrait);
                //  break;
                case UIDeviceOrientation.Portrait:
                    //UpdatePreviewLayer(previewLayerConnection,
                    //   AVCaptureVideoOrientation.Portrait);
                    // if(image.Orientation==UIImageOrientation.Right)
                    // var temp = new UIImage(originalImage.CGImage, 1.0f, originalImage.Orientation);
                    photo = originalImage;
                    //photo = ScaleAndRotateImage(temp, originalImage.Orientation);
                    break;

                //  break;
                default:
                    photo = originalImage;
                    //photo = new UIImage(originalImage.CGImage, photo.CurrentScale, UIImageOrientation.Down);
                    // photo = originalImage;
                    break;

                    //   UpdatePreviewLayer(previewLayerConnection,
                    //   AVCaptureVideoOrientation.Portrait);
                    //var temp1 = new UIImage(image.CGImage, 1.0f, image.Orientation);

                    // NSData retrunObj2 = ScaleAndRotateImage(temp1, image.Orientation).AsJPEG();
                    // return retrunObj2;
            }


            //  UIImage originalImage = ImageFromByteArray(bytes);
            // UIDevice device = UIDevice.CurrentDevice;
            // UIDeviceOrientation orientation = device.Orientation;


            // var 
            // var photo=originalImage;
            var bytesImagen = photo.AsJPEG().ToArray();
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var filePath = Path.Combine(documentsPath, filename);
            File.WriteAllBytes(filePath, bytesImagen);
            // OpenPDF(filePath);
            return await Task.FromResult(filePath);
        }
        UIImage ScaleAndRotateImage(UIImage imageIn, UIImageOrientation orIn)
        {
            int kMaxResolution = 2048;

            CGImage imgRef = imageIn.CGImage;
            float width = imgRef.Width;
            float height = imgRef.Height;
            CGAffineTransform transform = CGAffineTransform.MakeIdentity();
            RectangleF bounds = new RectangleF(0, 0, width, height);

            if (width > kMaxResolution || height > kMaxResolution)
            {
                float ratio = width / height;

                if (ratio > 1)
                {
                    bounds.Width = kMaxResolution;
                    bounds.Height = bounds.Width / ratio;
                }
                else
                {
                    bounds.Height = kMaxResolution;
                    bounds.Width = bounds.Height * ratio;
                }
            }

            float scaleRatio = bounds.Width / width;
            SizeF imageSize = new SizeF(width, height);
            UIImageOrientation orient = orIn;
            float boundHeight;

            switch (orient)
            {
                case UIImageOrientation.Up:                                        //EXIF = 1
                    transform = CGAffineTransform.MakeIdentity();
                    break;

                case UIImageOrientation.UpMirrored:                                //EXIF = 2
                    transform = CGAffineTransform.MakeTranslation(imageSize.Width, 0f);
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    break;

                case UIImageOrientation.Down:                                      //EXIF = 3
                    transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI);
                    break;

                case UIImageOrientation.DownMirrored:                              //EXIF = 4
                    transform = CGAffineTransform.MakeTranslation(0f, imageSize.Height);
                    transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
                    break;

                case UIImageOrientation.LeftMirrored:                              //EXIF = 5
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(imageSize.Height, imageSize.Width);
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.Left:                                      //EXIF = 6
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Width);
                    transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.RightMirrored:                             //EXIF = 7
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.Right:                                     //EXIF = 8
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0.0f);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
                    break;

                default:
                    throw new Exception("Invalid image orientation");
                    //break;
            }

            UIGraphics.BeginImageContext(bounds.Size);

            CGContext context = UIGraphics.GetCurrentContext();

            if (orient == UIImageOrientation.Right || orient == UIImageOrientation.Left)
            {
                context.ScaleCTM(-scaleRatio, scaleRatio);
                context.TranslateCTM(-height, 0);
            }
            else
            {
                context.ScaleCTM(scaleRatio, -scaleRatio);
                context.TranslateCTM(0, -height);
            }

            context.ConcatCTM(transform);
            context.DrawImage(new RectangleF(0, 0, width, height), imgRef);

            UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return imageCopy;
        }
        public class PDFItem : QLPreviewItem
        {
            string title;
            string uri;

            public PDFItem(string title, string uri)
            {
                this.title = title;
                this.uri = uri;
            }

            public override string ItemTitle
            {
                get { return title; }
            }

            public override NSUrl ItemUrl
            {
                get { return NSUrl.FromFilename(uri); }
            }
        }

        public class PDFPreviewControllerDataSource : QLPreviewControllerDataSource
        {
            string url = "";
            string filename = "";

            public PDFPreviewControllerDataSource(string url, string filename)
            {
                this.url = url;
                this.filename = filename;
            }

            public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
            {
                return new PDFItem(filename, url);
            }

            public override nint PreviewItemCount(QLPreviewController controller)
            {
                return 1;
            }


        }
    }
}