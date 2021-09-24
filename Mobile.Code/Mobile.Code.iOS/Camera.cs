using Foundation;
using System;
using UIKit;

namespace Mobile.Code.iOS
{
    public static class Camera
    {
        static UIImagePickerController picker;
        static Action<NSDictionary> _callback;

        static void Init()
        {
            if (picker != null)
                return;

            picker = new UIImagePickerController();
            picker.Delegate = new CameraDelegate();
        }

        class CameraDelegate : UIImagePickerControllerDelegate
        {
            public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
            {
                var cb = _callback;
                _callback = null;

                picker.DismissViewController(true, (Action)null);
                cb(info);
            }

            public override void Canceled(UIImagePickerController picker)
            {
                var cb = _callback;
                _callback = null;

                picker.DismissViewController(true, (Action)null);
                cb(null);
            }
        }

        public static void TakePicture(UIViewController parent, Action<NSDictionary> callback)
        {
            Init();
            picker.SourceType = UIImagePickerControllerSourceType.Camera;
            _callback = callback;
            parent.PresentViewController((UIViewController)picker, true, (Action)null);
        }

        public static void SelectPicture(UIViewController parent, Action<NSDictionary> callback)
        {
            Init();
            picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            _callback = callback;
            parent.PresentViewController((UIViewController)picker, true, (Action)null);
        }
    }
}