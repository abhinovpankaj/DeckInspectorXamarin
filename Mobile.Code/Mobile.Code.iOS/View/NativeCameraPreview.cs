using AVFoundation;
using CoreGraphics;
using Foundation;
using Mobile.Code.Camera2Forms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using UIKit;


namespace Mobile.Code.iOS.View
{
    public class NativeCameraPreview : UIView
    {
        AVCaptureVideoPreviewLayer previewLayer;
        CameraOptions cameraOptions;

        public event EventHandler<EventArgs> Tapped;

        public AVCaptureSession CaptureSession { get; private set; }

        public bool IsPreviewing { get; set; }

        public NativeCameraPreview(CameraOptions options)
        {
            cameraOptions = options;
            IsPreviewing = false;
            Initialize();
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            previewLayer.Frame = rect;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            OnTapped();
        }

        protected virtual void OnTapped()
        {
            var eventHandler = Tapped;
            if (eventHandler != null)
            {
                eventHandler(this, new EventArgs());
            }
        }
        AVCaptureDeviceInput captureDeviceInput;
        AVCaptureStillImageOutput stillImageOutput;
        void Initialize()
        {
            try
            {
                CaptureSession = new AVCaptureSession();
                previewLayer = new AVCaptureVideoPreviewLayer(CaptureSession)
                {
                    Frame = Bounds,
                    VideoGravity = AVLayerVideoGravity.ResizeAspectFill
                };

                var videoDevices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
                var cameraPosition = (cameraOptions == CameraOptions.Front) ? AVCaptureDevicePosition.Front : AVCaptureDevicePosition.Back;
                var device = videoDevices.FirstOrDefault(d => d.Position == cameraPosition);
                ConfigureCameraForDevice(device);

                if (device == null)
                {
                    return;
                }
                var dictionary = new NSMutableDictionary();
                dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
                stillImageOutput = new AVCaptureStillImageOutput()
                {
                    OutputSettings = new NSDictionary()
                };
                NSError error;
                var input = new AVCaptureDeviceInput(device, out error);

                CaptureSession.AddOutput(stillImageOutput);
                CaptureSession.AddInput(input);
                Layer.AddSublayer(previewLayer);
                CaptureSession.StartRunning();
                IsPreviewing = true;
            }
            catch (Exception message)
            {
                Debug.WriteLine(message.ToString());
            }
        }
        /*  public async Task<NSData> CapturePhoto()
          {
              var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
              var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
              var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
              return jpegImageAsNsData;
          }*/

        /*    public override void LayoutSubviews()
            {
                base.LayoutSubviews();

                UIDevice device = UIDevice.CurrentDevice;
                UIDeviceOrientation orientation = device.Orientation;
                AVCaptureConnection previewLayerConnection = this.previewLayer.Connection;
                if (previewLayerConnection.SupportsVideoOrientation)
                {
                    switch (orientation)
                    {
                        case UIDeviceOrientation.Portrait:
                            UpdatePreviewLayer(previewLayerConnection,
                                AVCaptureVideoOrientation.Portrait);
                            break;
                        case UIDeviceOrientation.LandscapeRight:
                            UpdatePreviewLayer(previewLayerConnection,
                                AVCaptureVideoOrientation.Portrait);
                            break;
                        case UIDeviceOrientation.LandscapeLeft:
                            UpdatePreviewLayer(previewLayerConnection,
                                AVCaptureVideoOrientation.Portrait);
                            break;
                        case UIDeviceOrientation.PortraitUpsideDown:
                            UpdatePreviewLayer(previewLayerConnection,
                                AVCaptureVideoOrientation.Portrait);
                            break;
                        default:
                            UpdatePreviewLayer(previewLayerConnection,
                                AVCaptureVideoOrientation.Portrait);
                            break;
                    }
                }
            }

            private void UpdatePreviewLayer(AVCaptureConnection layer,
                AVCaptureVideoOrientation orientation)
            {
                layer.VideoOrientation = orientation;
                previewLayer.Frame = this.Bounds;
            }*/

        /* public  UIImage ScaleAndRotateImage( UIImage image)
         {
             int kMaxResolution = 2048; // Or whatever

             CGImage imgRef = image.CGImage;
             float width = imgRef.Width;
             float height = imgRef.Height;
             CGAffineTransform transform = CGAffineTransform.MakeIdentity();
             RectangleF bounds = new RectangleF(0, 0, width, height);

             if (width > kMaxResolution || height > kMaxResolution)
             {
                 float ratio = width / height;

                 if (ratio > 1)
                 {
                     bounds.Size = new SizeF(kMaxResolution, bounds.Size.Width / ratio);
                 }
                 else
                 {
                     bounds.Size = new SizeF(bounds.Size.Height * ratio, kMaxResolution);
                 }
             }

             float scaleRatio = bounds.Size.Width / width;
             SizeF imageSize = new SizeF(imgRef.Width, imgRef.Height);
             UIImageOrientation orient = image.Orientation;
             float boundHeight;

             switch (orient)
             {
                 case UIImageOrientation.Up:                                        //EXIF = 1
                     transform = CGAffineTransform.MakeIdentity();
                     break;
                 // TODO: Add other Orientations
                 case UIImageOrientation.Right:                                     //EXIF = 8
                     boundHeight = bounds.Size.Height;
                     bounds.Size = new SizeF(boundHeight, bounds.Size.Width);
                     transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0);
                     transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
                     break;
                 default:
                     throw new Exception("Invalid image orientation");

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
         }*/
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
                    break;
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

        public async Task<NSData> CapturePhoto()
        {
            var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
            var jpegData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
            var image = UIImage.LoadFromData(jpegData);

            CGImage imgRed = image.CGImage;
            UIDevice device = UIDevice.CurrentDevice;
            UIDeviceOrientation orientation = device.Orientation;
            AVCaptureConnection previewLayerConnection = this.previewLayer.Connection;
            var photo = new UIImage(jpegData);
            if (previewLayerConnection.SupportsVideoOrientation)

            {


                switch (orientation)
                {
                    case UIDeviceOrientation.PortraitUpsideDown:
                        photo = new UIImage(image.CGImage, 1.0f, UIImageOrientation.Up);
                        // UpdatePreviewLayer(previewLayerConnection,
                        // AVCaptureVideoOrientation.Portrait);
                        break;
                    case UIDeviceOrientation.LandscapeLeft:
                        photo = new UIImage(image.CGImage, photo.CurrentScale, UIImageOrientation.Up);
                        // UpdatePreviewLayer(previewLayerConnection,
                        // AVCaptureVideoOrientation.Portrait);
                        break;
                    case UIDeviceOrientation.LandscapeRight:
                        //    photo = new UIImage(image.CGImage, 1.0f, UIImageOrientation.Down);

                        var temp12 = new UIImage(image.CGImage, photo.CurrentScale, UIImageOrientation.Down);

                        NSData retrunObj22 = ScaleAndRotateImage(temp12, UIImageOrientation.Down).AsJPEG();
                        return retrunObj22;
                    //UpdatePreviewLayer(previewLayerConnection,
                    // AVCaptureVideoOrientation.Portrait);
                    //  break;
                    case UIDeviceOrientation.Portrait:
                        //UpdatePreviewLayer(previewLayerConnection,
                        //   AVCaptureVideoOrientation.Portrait);
                        // if(image.Orientation==UIImageOrientation.Right)
                        var temp = new UIImage(image.CGImage, 1.0f, image.Orientation);

                        NSData retrunObj1 = ScaleAndRotateImage(temp, image.Orientation).AsJPEG();
                        return retrunObj1;

                    //  break;
                    default:

                        //   UpdatePreviewLayer(previewLayerConnection,
                        //   AVCaptureVideoOrientation.Portrait);
                        var temp1 = new UIImage(image.CGImage, 1.0f, image.Orientation);

                        NSData retrunObj2 = ScaleAndRotateImage(temp1, image.Orientation).AsJPEG();
                        return retrunObj2;
                }
            }
            LayoutSubviews();
            NSData retrunObj = photo.AsJPEG();
            return retrunObj;

        }
        public void ConfigureCameraForDevice(AVCaptureDevice device)
        {
            try
            {
                var error = new NSError();
                if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
                {
                    device.LockForConfiguration(out error);
                    device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
                    device.UnlockForConfiguration();
                }
                else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
                {
                    device.LockForConfiguration(out error);
                    device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
                    device.UnlockForConfiguration();
                }
                else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
                {
                    device.LockForConfiguration(out error);
                    device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
                    device.UnlockForConfiguration();
                }
            }
            catch (Exception message)
            {
                Debug.WriteLine(message.ToString());
            }
        }
    }
}