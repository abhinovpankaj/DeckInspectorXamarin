using AVFoundation;
using Mobile.Code.Camera2Forms;
using Mobile.Code.iOS.Renderers;
using Mobile.Code.iOS.View;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(CameraPreview), typeof(CameraPreviewRenderer))]
namespace Mobile.Code.iOS.Renderers
{
    public class CameraPreviewRenderer : ViewRenderer<CameraPreview, NativeCameraPreview>
    {
        NativeCameraPreview uiCameraPreview;
        private CameraPreview _currentElement;
        AVCaptureStillImageOutput stillImageOutput;
        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                uiCameraPreview = new NativeCameraPreview(e.NewElement.Camera);
                SetNativeControl(uiCameraPreview);
            }
            if (e.OldElement != null)
            {
                // Unsubscribe
                //   uiCameraPreview.Tapped -= OnCameraPreviewTapped;
            }
            if (e.NewElement != null)
            {
                e.NewElement.CameraClick = new Command(() => TakePicture());
                // Subscribe
                _currentElement = e.NewElement;
                // uiCameraPreview.Tapped += OnCameraPreviewTapped;
                // uiCameraPreview.Photo += OnPhoto;
            }
        }
        //public async Task<NSData> CapturePhoto()
        //{
        //    var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
        //    var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
        //    var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
        //    return jpegImageAsNsData;
        //}
        //private async void OnPhoto(object sender, byte[] imgSource)
        //{


        //    //UIImage imageInfo = new UIImage(data);

        //    //(Element as CameraPage).SetPhotoResult(data.ToArray(),
        //    //                                            (int)imageInfo.Size.Width,
        //    //                                            (int)imageInfo.Size.Height);

        //}
        private async void TakePicture()
        {
            var data = await uiCameraPreview.CapturePhoto();
            UIImage imageInfo = new UIImage(data);

            //(Element as CameraPage).SetPhotoResult(data.ToArray(),
            //                                            (int)imageInfo.Size.Width,
            //                                            (int)imageInfo.Size.Height);
            //uiCameraPreview.CaptureSession.
            //Camera.Ca(UIApplication.SharedApplication.KeyWindow.RootViewController, (imagePickerResult) =>
            //{
            //    var data = imagePickerResult;
            //   // _currentElement?.PictureTaken(data.ToArray())
            //});
            _currentElement?.PictureTaken(data.ToArray());
            //UIImage data =  uiCameraPreview.Capture();
            ////var data = await CapturePhoto();
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    _currentElement?.PictureTaken(data.ToArray());
            //});
            // uiCameraPreview.los

        }

        void OnCameraPreviewTapped(object sender, EventArgs e)
        {
            if (uiCameraPreview.IsPreviewing)
            {
                uiCameraPreview.CaptureSession.StopRunning();
                uiCameraPreview.IsPreviewing = false;
            }
            else
            {
                uiCameraPreview.CaptureSession.StartRunning();
                uiCameraPreview.IsPreviewing = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.CaptureSession.Dispose();
                Control.Dispose();
            }
            //  uiCameraPreview.Photo -= OnPhoto;
            // base.Dispose(disposing);
        }
    }
}