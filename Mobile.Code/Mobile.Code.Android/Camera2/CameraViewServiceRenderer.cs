using Android.Content;
using Android.Runtime;
using Android.Views;
using Camera2Forms.Camera2;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CameraPreview = Mobile.Code.Camera2Forms.CameraPreview;

[assembly: ExportRenderer(typeof(CameraPreview), typeof(CameraViewServiceRenderer))]
namespace Camera2Forms.Camera2
{
    public class CameraViewServiceRenderer : ViewRenderer<CameraPreview, CameraDroid>
    {
        private CameraDroid _camera;
        private CameraPreview _currentElement;
        private readonly Context _context;
        private readonly IWindowManager _windowManager;
        public CameraViewServiceRenderer(Context context) : base(context)
        {
            _context = context;
            _windowManager = _context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

        }

        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
        {
            base.OnElementChanged(e);

            _camera = new CameraDroid(Context);

            SetNativeControl(_camera);

            if (e.NewElement != null && _camera != null)
            {
                e.NewElement.CameraClick = new Command(() => TakePicture());
                _currentElement = e.NewElement;
                _camera.SetCameraOption(_currentElement.Camera);
                _camera.Photo += OnPhoto;
            }
        }

        public void TakePicture()
        {

            _camera.LockFocus();
        }

        private void OnPhoto(object sender, byte[] imgSource)
        {
            IWindowManager _windowManager = Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

            var displayRotation = _windowManager.DefaultDisplay.Orientation;
            //switch (_windowManager.DefaultDisplay.Rotation)
            //{
            //    case SurfaceOrientation.Rotation0:
            //       // return Orientation.PortraitUp;
            //    case SurfaceOrientation.Rotation180:
            //      //  return Orientation.PortraitDown;
            //    case SurfaceOrientation.Rotation90:
            //       // return Orientation.LandscapeLeft;
            //    case SurfaceOrientation.Rotation270:
            //       // return Orientation.LandscapeRight;
            //    default:
            //       // return Orientation.None;
            //}
            //var t= Android.Content.Res.Orientation.;
            // if(t==)
            ////Here you have the image byte data to do whatever you want 
            //var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //documentsPath = System.IO.Path.Combine(documentsPath, "WICR_Pictures", "temp");
            //Directory.CreateDirectory(documentsPath);

            //string filePath = System.IO.Path.Combine(documentsPath, Guid.NewGuid().ToString());

            //byte[] bArray = new byte[imgSource.Length];
            //Stream stream = new MemoryStream(bArray);
            //using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            //{
            //    using (bArray)
            //    {
            //        data.Read(bArray, 0, (int)data.Length);
            //    }
            //    int length = bArray.Length;
            //    fs.Write(bArray, 0, length);
            //}
            //return filePath;

            Device.BeginInvokeOnMainThread(() =>
            {
                _currentElement?.PictureTaken(imgSource);
            });
        }

        protected override void Dispose(bool disposing)
        {
            _camera.Photo -= OnPhoto;

            base.Dispose(disposing);
        }
    }
}
