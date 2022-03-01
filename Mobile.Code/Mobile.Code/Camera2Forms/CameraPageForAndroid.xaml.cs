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
    public partial class CameraPageForAndroid : ContentPage
    {
        private double width;
        private double height;

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                    FlexTypes.Direction = FlexDirection.Column;
                    //sc.Orientation = ScrollOrientation.Vertical;
                    innerGrid.RowDefinitions.Clear();
                    innerGrid.ColumnDefinitions.Clear();
                    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                    innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    innerGrid.Children.Remove(controlsGrid);
                    innerGrid.Children.Add(controlsGrid, 1, 0);
                }
                else
                {
                    FlexTypes.Direction = FlexDirection.Row;
                    innerGrid.RowDefinitions.Clear();
                    innerGrid.ColumnDefinitions.Clear();
                    innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    innerGrid.Children.Remove(controlsGrid);
                    innerGrid.Children.Add(controlsGrid, 0, 1);
                }
            }
        }
        //public string PageName { get; set; }
        //public ObservableCollection<VisualProjectLocationPhoto> Items { get; set; }
        //public ObservableCollection<VisualBuildingLocationPhoto> VisualBuildingItems { get; set; }
        //public ObservableCollection<VisualApartmentLocationPhoto> VisualApartmentItems { get; set; }
        public CameraPageForAndroid()
        {

            InitializeComponent();
            this.SizeChanged += CameraPageForAndroid_SizeChanged;

            // Items = new ObservableCollection<VisualProjectLocationPhoto>();
            CameraPreview.PictureFinished += OnPictureFinished;
            //  list = new ObservableCollection<MultiImage>();
        }

        private void CameraPageForAndroid_SizeChanged(object sender, EventArgs e)
        {
            var stack = Navigation.NavigationStack;
            if (this.Width > this.Height) //Landscape
            {
                // if (stack[stack.Count - 1].GetType() == typeof(ScoringPage))  //ScoringPage is my Portrait screen
                //  await Navigation.PushAsync(new ScoreCard_Landscape(), true);  //ScoreCard_Landscape is my Landscape screen
            }
            else
            {

            }
        }

        void OnCameraClicked(object sender, EventArgs e)
        {

            CameraPreview.CameraClick.Execute(null);
        }

        private async void OnPictureFinished()
        {
            CameraViewModel vm = (CameraViewModel)this.BindingContext;

            if (Device.RuntimePlatform == Device.iOS)
            {

                string filepath = await DependencyService.Get<ISaveFile>().SaveFiles(Guid.NewGuid().ToString(), CameraPreview.byteArr);
                
                App.ListCamera2Api.Add(new MultiImage() { Image = filepath, Id = Guid.NewGuid().ToString(), ImageArray = CameraPreview.byteArr });

                vm.ImageList.Add(new MultiImage() { Image = filepath, Id = Guid.NewGuid().ToString(), ImageArray = CameraPreview.byteArr });
                vm.ImageList = new ObservableCollection<MultiImage>(vm.ImageList.OrderByDescending(c => c.CreateOn));
                vm.CountPhoto = vm.ImageList.Count + " Photo(s)";
            }
            if (Device.RuntimePlatform == Device.Android)
            {
                string filepath = await DependencyService.Get<ISaveFile>().SaveFilesForCameraApi(Guid.NewGuid().ToString(), CameraPreview.byteArr);
                
                App.ListCamera2Api.Add(new MultiImage() { Image = filepath, Id = Guid.NewGuid().ToString(), ImageArray = CameraPreview.byteArr });
                vm.ImageList.Add(new MultiImage() { Image = filepath, Id = Guid.NewGuid().ToString(), ImageArray = CameraPreview.byteArr });
                vm.ImageList = new ObservableCollection<MultiImage>(vm.ImageList.OrderByDescending(c => c.CreateOn));
                vm.CountPhoto = vm.ImageList.Count + " Photo(s)";

               
            }

            
        }
        public static event EventHandler<ImageSource> PhotoCapturedEvent;

        public static void OnPhotoCaptured(ImageSource src)
        {
            PhotoCapturedEvent?.Invoke(new Camera2Forms.CameraPage(), src);
        }
        protected override void OnDisappearing()
        {
            

            CameraPreview.PictureFinished -= OnPictureFinished;

            //  MessagingCenter.Unsubscribe<Page1, T>(this, "Listen");
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