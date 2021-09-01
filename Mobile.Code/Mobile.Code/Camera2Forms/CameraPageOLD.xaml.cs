using Mobile.Code.Models;
using Mobile.Code.Services;
using Mobile.Code.ViewModels;
using Plugin.Screenshot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Camera2Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
       
        //public string PageName { get; set; }
        //public ObservableCollection<VisualProjectLocationPhoto> Items { get; set; }
        //public ObservableCollection<VisualBuildingLocationPhoto> VisualBuildingItems { get; set; }
        //public ObservableCollection<VisualApartmentLocationPhoto> VisualApartmentItems { get; set; }
        public CameraPage()
        {
           
            InitializeComponent();

           
           // Items = new ObservableCollection<VisualProjectLocationPhoto>();
            CameraPreview.PictureFinished += OnPictureFinished;
            list = new List<string>();
        }

        void OnCameraClicked(object sender, EventArgs e)
        {
           
            OnPictureFinished();
           // CameraPreview.CameraClick.Execute(null);
        }
        public List<string> list { get; set; }
        private async void OnPictureFinished()
        {
            // ImageSource temp = CameraPreview.ImageSource;
            CameraViewModel vm = (CameraViewModel)this.BindingContext;

            // await DisplayAlert("Confirm", "Picture Taken", "", "Ok");
            //string filepath = await DependencyService.Get<ISaveFile>().SaveFiles(Guid.NewGuid().ToString(), CameraPreview.byteArr);
            //img1.Source = filepath;
            //list.Add(filepath);
          //  detailGrid.IsVisible = false;
            string filepath = await CrossScreenshot.Current.CaptureAndSaveAsync();
            if (vm.IsVisualProjectLocatoion)
            {
                VisualProjectLocationPhoto obj = new VisualProjectLocationPhoto() { Image = filepath, Id = Guid.NewGuid().ToString(), VisualID = vm.ProjectLocation_Visual.Id };
                await vm.AddNewPhoto(obj);
            }
            if (vm.IsVisualBuilding)
            {
                VisualBuildingLocationPhoto obj = new VisualBuildingLocationPhoto() { Image = filepath, Id = Guid.NewGuid().ToString(), VisualID = vm.BuildingLocation_Visual.Id };
                await vm.AddNewPhoto(obj);
            }
            if (vm.IsVisualApartment)
            {
                VisualApartmentLocationPhoto obj = new VisualApartmentLocationPhoto() { Image = filepath, Id = Guid.NewGuid().ToString(), VisualID = vm.Apartment_Visual.Id };
                await vm.AddNewPhoto(obj);
            }
            if (vm.IsProjectLocation)
            {
                ProjectCommonLocationImages obj = new ProjectCommonLocationImages() { ImageUrl = filepath, Id = Guid.NewGuid().ToString(), ProjectLocationId = vm.ProjectLocation.Id };
                await vm.AddNewPhoto(obj);
            }
            if (vm.IsBuildingLocation)
            {
                BuildingCommonLocationImages obj = new BuildingCommonLocationImages() { Image = filepath, Id = Guid.NewGuid().ToString(), BuildingId = vm.BuildingLocation.Id };
                await vm.AddNewPhoto(obj);
            }
            if (vm.IsApartment)
            {
                BuildingApartmentImages obj = new BuildingApartmentImages() { Image = filepath, Id = Guid.NewGuid().ToString(), ApartmentID = vm.BuildingApartment.Id };
                await vm.AddNewPhoto(obj);
            }
            detailGrid.IsVisible = true;
            img1.Source = filepath;
            // countSelect.Text = list.Count + " Photo Taken";
        }
        protected override void OnDisappearing()
        {
            CameraViewModel vm = (CameraViewModel)this.BindingContext;

            MessagingCenter.Send(this, "Count", vm.CountItem);
            base.OnDisappearing();

            base.OnDisappearing();
        }
    }
}