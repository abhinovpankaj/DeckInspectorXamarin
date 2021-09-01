using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using Mobile.Code.CustomRenderer;
using UIKit;

namespace Mobile.Code.iOS.CustomRenderer
{
	public class UICameraPreview : UIView
	{
		AVCaptureVideoPreviewLayer previewLayer;
		CameraOptions cameraOptions;

		public event EventHandler<EventArgs> Tapped;
		public event EventHandler<EventArgs> Capture;

		public AVCaptureSession CaptureSession { get; private set; }

		public bool IsPreviewing { get; set; }

		public UICameraPreview(CameraOptions options)
		{
			cameraOptions = options;
			IsPreviewing = false;
			Initialize();
		}
		AVCaptureStillImageOutput stillImageOutput;
		public async Task<NSData> CapturePhoto()
		{
			var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
			var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
			var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
			return jpegImageAsNsData;
		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			if (previewLayer != null)
				previewLayer.Frame = Bounds;
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

		protected virtual void OnCapture()
		{
			var eventHandler = Tapped;
			if (eventHandler != null)
			{
				eventHandler(this, new EventArgs());
			}
		}

		void Initialize()
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

			if (device == null)
			{
				return;
			}

			NSError error;
			var input = new AVCaptureDeviceInput(device, out error);
			CaptureSession.AddInput(input);
			Layer.AddSublayer(previewLayer);
			CaptureSession.StartRunning();
			IsPreviewing = true;
		}
	}
}