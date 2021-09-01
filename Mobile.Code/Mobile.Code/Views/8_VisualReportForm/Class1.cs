using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace Mobile.Code.Views._8_VisualReportForm
{
    class ViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Xamarin.Forms.ImageSource _imageSource;

        public Xamarin.Forms.ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;

            }
        }

        async void PickImage()
        {
            // Get service (recommended to use constructor parameter aproach instead)
         //   IImagePickerService imagePickerService = Xamarin.Forms.DependencyService.Get<IImagePickerService>();

            // Pick the image
          //  ImageSource = await imagePickerService.PickImageAsync();
        }
    }
}