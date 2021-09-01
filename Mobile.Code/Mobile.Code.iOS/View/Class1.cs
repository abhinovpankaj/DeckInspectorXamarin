using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVFoundation;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Mobile.Code.Camera2Forms;
using Mobile.Code.iOS.View;
using UIKit;
using Xamarin.Forms;


namespace Mobile.Code.iOS.View
{
    public class NativeCameraPreview : UIView
    {
        AVCaptureVideoPreviewLayer previewLayer;
        CameraOptions cameraOptions;

        public AVCaptureSession CaptureSession { get; private set; }

        public AVCaptureStillImageOutput CaptureOutput { get; set; }

        public bool IsPreviewing { get; set; }

        public NativeCameraPreview(CameraOptions options)
        {
            cameraOptions = options;
            IsPreviewing = false;
            Initialize();
        }

        public override void LayoutSubviews()
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
                            AVCaptureVideoOrientation.LandscapeLeft);
                        break;
                    case UIDeviceOrientation.LandscapeLeft:
                        UpdatePreviewLayer(previewLayerConnection,
                            AVCaptureVideoOrientation.LandscapeRight);
                        break;
                    case UIDeviceOrientation.PortraitUpsideDown:
                        UpdatePreviewLayer(previewLayerConnection,
                            AVCaptureVideoOrientation.PortraitUpsideDown);
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
        }

        public async Task<NSData> CapturePhoto()
        {
            var videoConnection = CaptureOutput.ConnectionFromMediaType(AVMediaType.Video);
            var sampleBuffer = await CaptureOutput.CaptureStillImageTaskAsync(videoConnection);
            var jpegData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
            var image = UIImage.LoadFromData(jpegData);

            CGImage imgRed = image.CGImage;
            UIDevice device = UIDevice.CurrentDevice;
            UIDeviceOrientation orientation = device.Orientation;
            AVCaptureConnection previewLayerConnection = this.previewLayer.Connection;
            var photo = new UIImage(jpegData);
            if (previewLayerConnection.SupportsVideoOrientation)
               
            {
                if(orientation==UIDeviceOrientation.Portrait)
                {
                    UpdatePreviewLayer(previewLayerConnection,
                             AVCaptureVideoOrientation.Portrait);
                    NSData ret = photo.AsJPEG();
                    return ret; ;
                }

                switch (orientation)
                {
                    case UIDeviceOrientation.PortraitUpsideDown:
                         photo = new UIImage(image.CGImage, 1.0f,UIImageOrientation.Up);
                      //  UpdatePreviewLayer(previewLayerConnection,
                        //    AVCaptureVideoOrientation.Portrait);
                        break;
                    case UIDeviceOrientation.LandscapeLeft:
                        photo = new UIImage(image.CGImage, 1.0f, UIImageOrientation.Up);
                      //  UpdatePreviewLayer(previewLayerConnection,
                      //      AVCaptureVideoOrientation.Portrait);
                        break;
                    case UIDeviceOrientation.LandscapeRight:
                        photo = new UIImage(image.CGImage, 1.0f, UIImageOrientation.Down);
                       // UpdatePreviewLayer(previewLayerConnection,
                          //  AVCaptureVideoOrientation.Portrait);
                        break;
                    case UIDeviceOrientation.Portrait:
                      //  photo = new UIImage(image.CGImage, 1.0f, UIImageOrientation.Up);
                       // UpdatePreviewLayer(previewLayerConnection,
                       //     AVCaptureVideoOrientation.Portrait);
                        break;
                    default:
                        //UpdatePreviewLayer(previewLayerConnection,
                        //  AVCaptureVideoOrientation.Portrait);
                          //photo = new UIImage(image.CGImage, 1.0f, UIImageOrientation.Up);
                        break;
                }
            }
            LayoutSubviews();
            NSData retrunObj = photo.AsJPEG();
            return retrunObj;
           // var photo = new UIImage(jpegData);
           // var rotatedPhoto = RotateImage(photo, 180f);


         //   var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
         //   return jpegImageAsNsData;
           /* CALayer layer = new CALayer
            {
                //ContentsGravity = "kCAGravityResizeAspect",
                //ContentsRect = rect,
                //GeometryFlipped = true,
                ContentsScale = 1.0f,
                Frame = Bounds,
                Contents = rotatedPhoto.CGImage //Contents = photo.CGImage,
            };
           layer.Frame = previewLayer.Frame;

           var image= ImageFromLayer(layer);
            NSData imageData = image.AsJPEG();
            return imageData;
            /*var t=CIIma.LoadFromData()
            image.LoadData(NSData.FromArray(sampleBuffer));
            var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(image.AsJPEG());
            return jpegImageAsNsData;
            ImageFromLayer(layer).AsJPEG().AsStream());
            // MainPage.UpdateImage(ImageFromLayer(layer).AsJPEG().AsStream());*/
        }

        public UIImage RotateImage(UIImage image, float degree)
        {
            float Radians = degree * (float)Math.PI / 180;

            UIView view = new UIView(frame: new CGRect(0, 0, image.Size.Width, image.Size.Height));
            CGAffineTransform t = CGAffineTransform.MakeRotation(Radians);
            view.Transform = t;
            CGSize size = view.Frame.Size;

            UIGraphics.BeginImageContext(size);
            CGContext context = UIGraphics.GetCurrentContext();

            context.TranslateCTM(size.Width / 2, size.Height / 2);
            context.RotateCTM(Radians);
            context.ScaleCTM(1, -1);

            context.DrawImage(new CGRect(-image.Size.Width / 2, -image.Size.Height / 2, image.Size.Width, image.Size.Height), image.CGImage);

            UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return imageCopy;
        }

        UIImage ImageFromLayer(CALayer layer)
        {
            UIGraphics.BeginImageContextWithOptions(
                layer.Frame.Size,
                layer.Opaque,
                0);
            layer.RenderInContext(UIGraphics.GetCurrentContext());
            var outputImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return outputImage;
        }

        void Initialize()
        {
            CaptureSession = new AVCaptureSession();
            CaptureSession.SessionPreset = AVCaptureSession.PresetPhoto;
            previewLayer = new AVCaptureVideoPreviewLayer(CaptureSession)
            {
                Frame = Bounds,
                VideoGravity = AVLayerVideoGravity.ResizeAspectFill
            };

            var videoDevices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
            var cameraPosition = (cameraOptions == CameraOptions.Front) ? AVCaptureDevicePosition.Front : AVCaptureDevicePosition.Back;
            var device = videoDevices.FirstOrDefault(d => d.Position == cameraPosition);

            if (device == null)
            {
                return;
            }

            NSError error;
            var input = new AVCaptureDeviceInput(device, out error);

            var dictionary = new NSMutableDictionary();
            dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
            CaptureOutput = new AVCaptureStillImageOutput()
            {
                OutputSettings = new NSDictionary()
            };
            CaptureSession.AddOutput(CaptureOutput);

            CaptureSession.AddInput(input);
            Layer.AddSublayer(previewLayer);
            CaptureSession.StartRunning();
            IsPreviewing = true;
        }
    }
   
}