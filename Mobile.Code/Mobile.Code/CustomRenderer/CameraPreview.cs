using System;
using System.IO;
using Xamarin.Forms;

namespace Mobile.Code.CustomRenderer
{
    public class CameraPreview : View
    {
        public event EventHandler PhotoCaptured;
        public static readonly BindableProperty CameraProperty = BindableProperty.Create(
            propertyName: "Camera",
            returnType: typeof(CameraOptions),
            declaringType: typeof(CameraPreview),
            defaultValue: CameraOptions.Rear);

        public CameraOptions Camera
        {
            get { return (CameraOptions)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }
        private byte[] _aa;
        public byte[] byteArr
        {
            get { return _aa; }
            set { _aa = value; }
        }
        private ImageSource _mageSouce;

        public ImageSource ImageSource
        {
            get { return _mageSouce; }
            set { _mageSouce = value; }
        }
        public void PictureTaken(byte[] imgSource)
        {
            byteArr = imgSource;
            var stream1 = new MemoryStream(imgSource);
            ImageSource = ImageSource.FromStream(() => new MemoryStream(imgSource));
            PictureFinished?.Invoke();

        }
        public event Action PictureFinished;
    }
}