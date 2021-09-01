﻿using System;
using System.Collections.Generic;
using System.Text;
using ImageEditor.ViewModels;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using System.Collections.ObjectModel;
using Mobile.Code.Models;
using Mobile.Code.Views;

namespace Mobile.Code.ViewModels._8_VisualReportForm
{
    public class AdditionalInvasiveViewModel : BaseViewModel
    {

        private ImageData _imgData;
        public Command GoBackCommand { get; set; }
        public Command SaveCommand { get; set; }

        public ImageData ImgData
        {
            get { return _imgData; }
            set { _imgData = value; }
        }

        public string SelectedImage { get; set; }


        private string _heading;

        public string Heading
        {
            get { return _heading; }
            set { _heading = value; OnPropertyChanged("Heading"); }
        }
        public Command LoadItemsCommand { get; set; }
        private async Task GoBack()
        {
            await Shell.Current.Navigation.PopAsync();
            //var result = await Shell.Current.DisplayAlert(
            //      "Alert",
            //      "Are you sure you want to go back?",
            //      "Yes", "No");



            //if (result)
            //{
            //    //if (!string.IsNullOrEmpty(Project.Id))
            //    //{
            //    //    await Shell.Current.Navigation.PopAsync();
            //    //}
            //    //else
            //    //{
            //    //    await Shell.Current.GoToAsync("//main");
            //    //}
            //    // await Shell.Current.Navigation.Cle ;
            //}
        }
        private CheakBoxListReturntModel _exteriorElements;

        public CheakBoxListReturntModel ExteriorElements
        {
            get { return _exteriorElements; }
            set { _exteriorElements = value; OnPropertyChanged("ExteriorElements"); }
        }

        private async Task Save()
        {
            //var picker = new GMImagePickerController();
            //picker.FinishedPickingAssets += (sender, args) => {
            //    Console.WriteLine("User finished picking assets. {0} items selected.", args.Assets.Length);
            //};
            //await PresentViewControllerAsync(picker, true);


            //if (string.IsNullOrEmpty(project.ProjectName))
            //{
            //    await Shell.Current.DisplayAlert("Alert", "Project name is required", "OK");
            //    return;
            //}
            //if (string.IsNullOrEmpty(Project.Id))
            //{
            //    Project.Id = Guid.NewGuid().ToString();

            //    //Project.
            //    //   Project.ProjectImage = ImgPata;
            //    Project.CreatedOn = DateTime.Now.ToString("MMM dd,yyyy");
            //    await ProjectDataStore.AddItemAsync(Project);
            //}
            //else
            //{
            //    await ProjectDataStore.UpdateItemAsync(project);
            //}
            //await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = Project } });
        }
        private ProjectLocation_Visual visualForm;

        public ProjectLocation_Visual ProjectLocation_Visual
        {
            get { return visualForm; }
            set { visualForm = value; OnPropertyChanged("ProjectLocation_Visual"); }
        }

        private BuildingLocation_Visual _buildingLocation_Visual;

        public BuildingLocation_Visual BuildingLocation_Visual
        {
            get { return _buildingLocation_Visual; }
            set { _buildingLocation_Visual = value; OnPropertyChanged("BuildingLocation_Visual"); }
        }

        private Apartment_Visual _Apa_Visual;

        public Apartment_Visual Apartment_Visual
        {
            get { return _Apa_Visual; }
            set { _Apa_Visual = value; OnPropertyChanged("Apartment_Visual"); }
        }

        private bool _isprojectLoc;

        public bool IsVisualProjectLocatoion
        {
            get { return _isprojectLoc; }
            set { _isprojectLoc = value; OnPropertyChanged("IsVisualProjectLocatoion"); }
        }
        private bool _isbuildingLoc;
        public bool IsVisualBuilding
        {
            get { return _isbuildingLoc; }
            set { _isbuildingLoc = value; OnPropertyChanged("IsVisualBuilding"); }
        }


        private bool _isA;
        public bool IsVisualApartment
        {
            get { return _isA; }
            set { _isA = value; OnPropertyChanged("IsVisualApartment"); }
        }
        public ICommand ShowImagesCommand => new Command(async () => await ShowImagesExecute());
        
        private async Task ShowImagesExecute()
        {
            if (!string.IsNullOrEmpty(UnitPhotoCount))
            {
                if (UnitPhotoCount != "0")
                {
                    if (string.IsNullOrEmpty(visualForm.Id))
                    {
                        await Shell.Current.Navigation.PushAsync(new UnitPhtoForm() { BindingContext = new UnitPhotoViewModel() { ProjectLocation_Visual = visualForm, IsVisualProjectLocatoion = true, IsEdit = true } });
                    }
                    else
                    {
                        await Shell.Current.Navigation.PushAsync(new UnitPhtoForm() { BindingContext = new UnitPhotoViewModel() { ProjectLocation_Visual = visualForm, IsVisualProjectLocatoion = true, IsEdit = true } });
                    }
                }
            }
        }
        public AdditionalInvasiveViewModel()
        {

            ImgData = new ImageData();
            GoBackCommand = new Command(async () => await GoBack());
            SaveCommand = new Command(async () => await Save());
            //    ExteriorElements = new CheakBoxListReturntModel();
            //MessagingCenter.Subscribe<ImageEditor.Pages.ImageEditorPage, string>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as string;
            //    await App.Current.MainPage.DisplayAlert(newItem,newItem,"ok","cancel");
            //});
            //LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            //Load();
            //ImgData = new ImageData();
            //MessagingCenter.Subscribe<PopUpCheakListBox, CheakBoxListReturntModel>(this, "SelectedItem", async (obj, item) =>
            //{
            //    ExteriorElements = item as CheakBoxListReturntModel;

            //});
            //UnitPhotos = new ObservableCollection<ImageSource>();
            //MessagingCenter.Subscribe<Camera2Forms.CameraPage, ObservableCollection<ImageSource>>(this, "Images", async (obj, item) =>
            //{
            //    //UnitPhotos.Clear();
            //    ObservableCollection < ImageSource > listTempImage = item as ObservableCollection<ImageSource>;

            //    foreach (var img in listTempImage)
            //    {
            //        UnitPhotos.Add(img);
            //    }
            //    UnitPhotoCount = UnitPhotos.Count.ToString();

            //});
        }
        private string _unitPhotCount;

        public string UnitPhotoCount
        {
            get { return _unitPhotCount; }
            set { _unitPhotCount = value; OnPropertyChanged("UnitPhotoCount"); }
        }

        public ObservableCollection<ImageSource> UnitPhotos { get; set; }
        public ICommand ViusalReviewDetailCommand => new Command<CustomRadioItem>(async (CustomRadioItem parm) => await ExecuteViusalReviewDetailCommand(parm));

        private Task ExecuteViusalReviewDetailCommand(CustomRadioItem parm)
        {
            throw new NotImplementedException();
        }
        private ObservableCollection<VisualProjectLocationPhoto> _visualProjectLocationPhotoItems;

        public ObservableCollection<VisualProjectLocationPhoto> VisualProjectLocationPhotoItems
        {
            get { return _visualProjectLocationPhotoItems; }
            set { _visualProjectLocationPhotoItems = value; OnPropertyChanged("VisualProjectLocationPhotoItems"); }
        }
        private ObservableCollection<VisualBuildingLocationPhoto> _visualBuildingLocationPhotoItems;

        public ObservableCollection<VisualBuildingLocationPhoto> VisualBuildingLocationPhotoItems
        {
            get { return _visualBuildingLocationPhotoItems; }
            set { _visualBuildingLocationPhotoItems = value; OnPropertyChanged("VisualBuildingLocationPhotoItems"); }
        }

        private ObservableCollection<VisualApartmentLocationPhoto> _visualApartmentLocationPhotoPhotoItems;

        public ObservableCollection<VisualApartmentLocationPhoto> VisualApartmentLocationPhotoItems
        {
            get { return _visualApartmentLocationPhotoPhotoItems; }
            set { _visualApartmentLocationPhotoPhotoItems = value; OnPropertyChanged("VisualApartmentLocationPhotoItems"); }
        }
        private bool _isEdit;

        public bool IsEdit
        {
            get { return _isEdit; }
            set { _isEdit = value; OnPropertyChanged("IsEdit"); }
        }
        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }
        private async Task<bool> Running()
        {
            if (IsVisualProjectLocatoion)
            {
                VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(ProjectLocation_Visual.Id, false));
                if (VisualProjectLocationPhotoItems.Count == 0)
                {
                    await Shell.Current.Navigation.PopAsync();
                }
            }
            if (IsVisualBuilding)
            {
                VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(BuildingLocation_Visual.Id, false));
                if (VisualBuildingLocationPhotoItems.Count == 0)
                {
                    await Shell.Current.Navigation.PopAsync();
                }
            }
            if (IsVisualApartment)
            {
                VisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(Apartment_Visual.Id, false));
                if (VisualApartmentLocationPhotoItems.Count == 0)
                {
                    await Shell.Current.Navigation.PopAsync();
                }
            }


            return await Task.FromResult(true);
        }
        public async Task<bool> LoadAsync()
        {
            IsBusyProgress = true;
            bool complete = await Task.Run(Running).ConfigureAwait(false);
            if (complete == true)
            {



                IsBusyProgress = false;


            }
            return await Task.FromResult(true);

        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }


        private VisualProjectLocationPhoto _visualProjectLocationPhoto;

        public VisualProjectLocationPhoto VisualProjectLocationPhoto
        {
            get { return _visualProjectLocationPhoto; }
            set { _visualProjectLocationPhoto = value; OnPropertyChanged("VisualProjectLocationPhoto"); }
        }

        private VisualBuildingLocationPhoto _visualBuildingLocationPhoto;

        public VisualBuildingLocationPhoto VisualBuildingLocationPhoto
        {
            get { return _visualBuildingLocationPhoto; }
            set { _visualBuildingLocationPhoto = value; OnPropertyChanged("VisualBuildingLocationPhoto"); }
        }


        private VisualApartmentLocationPhoto _visualApartmentLocationPhoto;

        public VisualApartmentLocationPhoto VisualApartmentLocationPhoto
        {
            get { return _visualApartmentLocationPhoto; }
            set { _visualApartmentLocationPhoto = value; OnPropertyChanged("VisualApartmentLocationPhoto"); }
        }

        public ICommand ImageDetailCommand => new Command<BindingModel>(async (BindingModel parm) => await ImageDetailCommandCommandExecute(parm));
        private async Task ImageDetailCommandCommandExecute(BindingModel parm)
        {



            if (parm.GetType() == typeof(VisualProjectLocationPhoto))
            {
                VisualProjectLocationPhoto = parm as VisualProjectLocationPhoto;
                // VisualProjectLocationPhoto = parm;
                ImgData.Path = VisualProjectLocationPhoto.ImageUrl;
                ImgData.ParentID = VisualProjectLocationPhoto.VisualLocationId;
                ImgData.VisualProjectLocationPhoto = VisualProjectLocationPhoto;
                ImgData.IsEditVisual = IsEdit;
                //ImgData.VisualProjectLocationPhotos = VisualProjectLocationPhotoItems;
                ImgData.FormType = "VP";
                await CurrentWithoutDetail.EditImage(ImgData, GetImage);



            }
            if (parm.GetType() == typeof(VisualBuildingLocationPhoto))
            {
                VisualBuildingLocationPhoto = parm as VisualBuildingLocationPhoto;
                ImgData.Path = VisualBuildingLocationPhoto.ImageUrl;
                ImgData.FormType = "VB";
                ImgData.IsEditVisual = IsEdit;
                ImgData.VisualBuildingLocationPhoto = VisualBuildingLocationPhoto;
                await CurrentWithoutDetail.EditImage(ImgData, GetImage);

            }
            if (parm.GetType() == typeof(VisualApartmentLocationPhoto))
            {
                VisualApartmentLocationPhoto = parm as VisualApartmentLocationPhoto;
                ImgData.Path = VisualApartmentLocationPhoto.ImageUrl;
                ImgData.FormType = "VA";
                ImgData.IsEditVisual = IsEdit;
                ImgData.VisualApartmentLocationPhoto = VisualApartmentLocationPhoto;
                await CurrentWithoutDetail.EditImage(ImgData, GetImage);

            }

        }

        public ICommand DeleteImageCommand => new Command<BindingModel>(async (BindingModel parm) => await DeleteImageCommandCommandExecute(parm));
        private async Task DeleteImageCommandCommandExecute(BindingModel parm)
        {
            var result = await Shell.Current.DisplayAlert(
                  "Alert",
                  "Are you sure you want to remove?",
                  "Yes", "No");



            if (result)
            {
                if (parm.GetType() == typeof(VisualProjectLocationPhoto))
                {
                    VisualProjectLocationPhoto obj = parm as VisualProjectLocationPhoto;
                    await VisualProjectLocationPhotoDataStore.DeleteItemAsync(obj, IsEdit);

                }
                if (parm.GetType() == typeof(VisualBuildingLocationPhoto))
                {
                    VisualBuildingLocationPhoto obj = parm as VisualBuildingLocationPhoto;
                    await VisualBuildingLocationPhotoDataStore.DeleteItemAsync(obj);

                }
                if (parm.GetType() == typeof(VisualApartmentLocationPhoto))
                {
                    VisualApartmentLocationPhoto obj = parm as VisualApartmentLocationPhoto;
                    await VisualApartmentLocationPhotoDataStore.DeleteItemAsync(obj);

                }
                await LoadAsync();
            }


        }

        private string _imgPath;

        public string ImgPata
        {
            get { return _imgPath; }
            set { _imgPath = value; OnPropertyChanged(); }
        }

        private async void GetImage(ImageData ImgData)
        {
            //if (IsVisualProjectLocatoion)
            //{
            //   // VisualProjectLocationPhoto obj = parm as VisualProjectLocationPhoto;
            //    // VisualProjectLocationPhoto = parm;
            //    VisualProjectLocationPhoto.ImageUrl = ImgData.Path;
            //    await VisualProjectLocationPhotoDataStore.UpdateItemAsync(VisualProjectLocationPhoto,IsEdit);



            //}
            //if (IsVisualBuilding)
            //{
            //    VisualBuildingLocationPhoto.ImageUrl = ImgData.Path;
            //    await VisualBuildingLocationPhotoDataStore.UpdateItemAsync(VisualBuildingLocationPhoto);

            //}
            //if (IsVisualApartment)
            //{
            //    VisualApartmentLocationPhoto.ImageUrl = ImgData.Path;
            //    await VisualApartmentLocationPhotoDataStore.UpdateItemAsync(VisualApartmentLocationPhoto);

            //}

            await LoadAsync();
        }
    }
}
