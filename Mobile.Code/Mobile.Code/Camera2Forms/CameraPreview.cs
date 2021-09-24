using System;
using System.IO;
using Xamarin.Forms;

namespace Mobile.Code.Camera2Forms
{
    public enum CameraOptions
    {
        Rear,
        Front
    }

    public class CameraPreview : View
    {
        Command cameraClick;

        public static readonly BindableProperty CameraProperty = BindableProperty.Create(
            propertyName: "Camera",
            returnType: typeof(CameraOptions),
            declaringType: typeof(CameraPreview),
            defaultValue: CameraOptions.Rear);

        private ImageSource _mageSouce;

        public ImageSource ImageSource
        {
            get { return _mageSouce; }
            set { _mageSouce = value; }
        }

        public CameraOptions Camera
        {
            get { return (CameraOptions)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }

        public Command CameraClick
        {
            get { return cameraClick; }
            set { cameraClick = value; }
        }
        private byte[] _aa;

        public byte[] byteArr
        {
            get { return _aa; }
            set { _aa = value; }
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
