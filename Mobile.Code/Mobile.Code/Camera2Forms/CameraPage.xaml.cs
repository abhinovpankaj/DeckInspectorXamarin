using Mobile.Code.Models;
using Mobile.Code.Services;
using Mobile.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
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


            // Items = new ObservableCollection<VisualProjectLocationPhoto>();
            CameraPreview.PictureFinished += OnPictureFinished;
            //  list = new ObservableCollection<MultiImage>();
        }

        void OnCameraClicked(object sender, EventArgs e)
        {

            CameraPreview.CameraClick.Execute(null);
        }

        private async void OnPictureFinished()
        {
            CameraViewModel vm = (CameraViewModel)this.BindingContext;
            string filepath = string.Empty;


            if (Device.RuntimePlatform == Device.iOS)
            {

                filepath = await DependencyService.Get<ISaveFile>().SaveFiles(Guid.NewGuid().ToString(), CameraPreview.byteArr);

            }
            if (Device.RuntimePlatform == Device.Android)
            {
                filepath = await DependencyService.Get<ISaveFile>().SaveFilesForCameraApi(Guid.NewGuid().ToString(), CameraPreview.byteArr);

            }
            MultiImage img = new MultiImage() { Image = filepath, Id = Guid.NewGuid().ToString(), ImageArray = CameraPreview.byteArr, ImageType = vm.ImageType };
            App.ListCamera2Api.Add(img);

            // vm.ImageList.Add(new MultiImage() { Image = filepath, Id = Guid.NewGuid().ToString(), ImageArray = CameraPreview.byteArr });
            vm.ImageList.Add(img);
            vm.ImageList = new ObservableCollection<MultiImage>(vm.ImageList.OrderByDescending(c => c.CreateOn));
            vm.CountPhoto = vm.ImageList.Count + " Photo(s)";



        }
        public static event EventHandler<ImageSource> PhotoCapturedEvent;

        public static void OnPhotoCaptured(ImageSource src)
        {
            PhotoCapturedEvent?.Invoke(new Camera2Forms.CameraPage(), src);
        }
        protected override void OnDisappearing()
        {
            //CameraViewModel vm = (CameraViewModel)this.BindingContext;
            //vm.IsBusyProgress = true;



            //MessagingCenter.Send(this, "ImageList", vm.ImageList);


            CameraPreview.PictureFinished -= OnPictureFinished;

            //  MessagingCenter.Unsubscribe<Page1, T>(this, "Listen");
            base.OnDisappearing();
            // vm.ImageList.Clear();
            // MessagingCenter.Unsubscribe<Camera2Forms.CameraPage, ObservableCollection<MultiImage>>(this, "ImageList");
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
    }
}