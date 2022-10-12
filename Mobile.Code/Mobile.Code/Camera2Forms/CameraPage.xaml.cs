using Mobile.Code.Models;
using Mobile.Code.Services;
using Mobile.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Camera2Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        private double width;
        private double height;


        public CameraPage()
        {

            InitializeComponent();
            //CameraPreview.PictureFinished += OnPictureFinished;
            // cameraView.MediaCaptured += CameraView_MediaCaptured; ;
        }

        private async void CameraView_MediaCaptured(object sender, Xamarin.CommunityToolkit.UI.Views.MediaCapturedEventArgs e)
        {

            //switch (cameraView.CaptureMode)
            //{
            //    default:
            //    case CameraCaptureMode.Default:
            //    case CameraCaptureMode.Photo:
            //        previewImage.Rotation = e.Rotation;
            //        previewImage.Source = e.Image;                    
            //        break;

            //}

            CameraViewModel vm = (CameraViewModel)this.BindingContext;
            string filepath = string.Empty;


            if (Device.RuntimePlatform == Device.iOS)
            {

                filepath = await DependencyService.Get<ISaveFile>().SaveFiles(Guid.NewGuid().ToString(), e.ImageData);

            }
            if (Device.RuntimePlatform == Device.Android)
            {
                filepath = await DependencyService.Get<ISaveFile>().SaveFilesForCameraApi(Guid.NewGuid().ToString(), e.ImageData);

            }
            MultiImage img = new MultiImage() { Image = filepath, Id = Guid.NewGuid().ToString(), ImageArray = e.ImageData, ImageType = vm.ImageType };
            App.ListCamera2Api.Add(img);


            vm.ImageList.Add(img);
            vm.ImageList = new ObservableCollection<MultiImage>(vm.ImageList.OrderByDescending(c => c.CreateOn));
            vm.CountPhoto = vm.ImageList.Count + " Photo(s)";
        }

        void OnCameraClicked(object sender, EventArgs e)
        {
            //CameraPreview.CameraClick.Execute(null);
            cameraView.Shutter();
        }

        private void OnPictureFinished()
        {
            //CameraViewModel vm = (CameraViewModel)this.BindingContext;
            //string filepath = string.Empty;


            //if (Device.RuntimePlatform == Device.iOS)
            //{

            //    filepath = await DependencyService.Get<ISaveFile>().SaveFiles(Guid.NewGuid().ToString(), CameraPreview.byteArr);

            //}
            //if (Device.RuntimePlatform == Device.Android)
            //{
            //    filepath = await DependencyService.Get<ISaveFile>().SaveFilesForCameraApi(Guid.NewGuid().ToString(), CameraPreview.byteArr);

            //}
            //MultiImage img = new MultiImage() { Image = filepath, Id = Guid.NewGuid().ToString(), ImageArray = CameraPreview.byteArr, ImageType = vm.ImageType };
            //App.ListCamera2Api.Add(img);


            //vm.ImageList.Add(img);
            //vm.ImageList = new ObservableCollection<MultiImage>(vm.ImageList.OrderByDescending(c => c.CreateOn));
            //vm.CountPhoto = vm.ImageList.Count + " Photo(s)";
        }
        public static event EventHandler<ImageSource> PhotoCapturedEvent;

        public static void OnPhotoCaptured(ImageSource src)
        {
            PhotoCapturedEvent?.Invoke(new Camera2Forms.CameraPage(), src);
        }
        protected override void OnDisappearing()
        {


            // CameraPreview.PictureFinished -= OnPictureFinished;


            base.OnDisappearing();

        }
        protected override void OnAppearing()
        {
            App.ListCamera2Api = new List<MultiImage>();
            base.OnAppearing();
        }
        private async void btnSave_Clicked(object sender, EventArgs e)
        {

            await Shell.Current.Navigation.PopModalAsync();
            // CameraViewModel vm = (CameraViewModel)this.BindingContext;

            //vm.IsBusyProgress = true;

            //bool complete = await Task.Run(Running);
            //if (complete == true)
            //{
            //    vm.IsBusyProgress = false;

            //    //  if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(ProjectLocationDetail))

            //    await Shell.Current.Navigation.PopModalAsync();
            //    // DependencyService.Get<ILodingPageService>().HideLoadingPage();

            //}
        }
        private async Task<bool> Running()
        {
            CameraViewModel vm = (CameraViewModel)this.BindingContext;
            if (vm.IsProjectLocation)
            {
                bool result = HttpUtil.Upload(vm.ProjectLocation.Name, "/api/ProjectLocationImage/AddEdit?ParentId=" + vm.ProjectLocation.Id + "&UserId=" + App.LogUser.Id.ToString(), vm.ImageList);

            }
            if (vm.IsBuildingLocation)
            {
                bool result = HttpUtil.Upload(vm.BuildingLocation.Name, "/api/BuildingLocationImage/AddEdit?ParentId=" + vm.BuildingLocation.Id + "&UserId=" + App.LogUser.Id.ToString(), vm.ImageList);

            }
            if (vm.IsApartment)
            {
                bool result = HttpUtil.Upload(vm.BuildingApartment.Name, "/api/BuildingApartmentImage/AddEdit?ParentId=" + vm.BuildingApartment.Id + "&UserId=" + App.LogUser.Id.ToString(), vm.ImageList);

            }
            return await Task.FromResult(true);
        }

        //private void ZoomSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        //{
        //    cameraView.Zoom = (float)zoomSlider.Value;
        //    zoomLabel.Text = string.Format("Zoom: {0}", Math.Round(zoomSlider.Value));
        //}

        //private void CameraView_OnAvailable(object sender, bool e)
        //{
        //    if (e)
        //    {
        //        zoomSlider.Value = cameraView.Zoom;
        //        var max = cameraView.MaxZoom;
        //        if (max > zoomSlider.Minimum && max > zoomSlider.Value)
        //            zoomSlider.Maximum = max;
        //        else
        //            zoomSlider.Maximum = zoomSlider.Minimum + 1; // if max == min throws exception
        //    }
        //    zoomSlider.IsEnabled = e;
        //}
        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;
        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            var Content = cameraView;
            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);

            }
            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.
                //xOffset = Content.TranslationX;
                //yOffset = Content.TranslationY;
                cameraView.Zoom = currentScale;
                zoomfactor.Text = Math.Round(currentScale,2).ToString() + "x";
            }
        }

        private void OnFlashClicked(object sender, EventArgs e)
        {
            if (cameraView.FlashMode == CameraFlashMode.On)
            {
                cameraView.FlashMode = CameraFlashMode.Off;                
            }
            else
            {
                cameraView.FlashMode = CameraFlashMode.On;
            }               
        }
    }
}