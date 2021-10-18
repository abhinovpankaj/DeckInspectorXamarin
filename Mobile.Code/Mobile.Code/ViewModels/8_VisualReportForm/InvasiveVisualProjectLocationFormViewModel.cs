using ImageEditor.ViewModels;
using Mobile.Code.Media;
using Mobile.Code.Models;
using Mobile.Code.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class InvasiveVisualProjectLocationFormViewModel : BaseViewModel
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
            var result = await Shell.Current.DisplayAlert(
                  "Alert",
                  "Are you sure you want to go back?",
                  "Yes", "No");



            if (result)
            {
                await Shell.Current.Navigation.PopAsync();
                //if (!string.IsNullOrEmpty(Project.Id))
                //{
                //    await Shell.Current.Navigation.PopAsync();
                //}
                //else
                //{
                //    await Shell.Current.GoToAsync("//main");
                //}
                // await Shell.Current.Navigation.Cle ;
            }
        }
        private ObservableCollection<string> _exteriorElements;

        public ObservableCollection<string> ExteriorElements
        {
            get { return _exteriorElements; }
            set { _exteriorElements = value; OnPropertyChanged("ExteriorElements"); }
        }
        private ObservableCollection<string> _wpe;
        public ObservableCollection<string> WaterProofingElements
        {
            get { return _wpe; }
            set { _wpe = value; OnPropertyChanged("WaterProofingElements"); }
        }
        private ProjectLocation _projectLocation;
        public ProjectLocation ProjectLocation
        {
            get { return _projectLocation; }
            set { _projectLocation = value; OnPropertyChanged("ProjectLocation"); }
        }

        private async Task Save()
        {
            IsBusyProgress = true;
            Response result = await Task.Run(SaveLoad);
            if (result != null)
            {

                if (result.Status == ApiResult.Success)
                {



                    IsBusyProgress = false;
                    await Shell.Current.Navigation.PopAsync();

                }
                else
                {
                    IsBusyProgress = false;
                    await Shell.Current.DisplayAlert("Validation Error", result.Message, "OK");
                }
            }

        }
        private async Task<Response> SaveLoad()
        {
            Response response = new Response();
            try
            {
                string errorMessage = string.Empty;
                if (string.IsNullOrEmpty(VisualForm.Name))
                {
                    errorMessage += "\nName is required\n";
                }
                if (string.IsNullOrEmpty(UnitPhotoCount) || UnitPhotoCount == "0")
                {
                    errorMessage += "\nUnit photo required\n";
                }
                if (ExteriorElements.Count == 0)
                {
                    errorMessage += "\nExterior Elements photo required\n";
                }
                if (WaterProofingElements.Count == 0)
                {
                    errorMessage += "\nWaterProofing Elements required\n";
                }
                if (RadioList_VisualReviewItems.Where(c => c.IsSelected).Any() == false)
                {
                    errorMessage += "\nVisual Review required\n";
                }
                if (RadioList_AnyVisualSignItems.Where(c => c.IsSelected).Any() == false)
                {
                    errorMessage += "\nAny visual signs of leaksrequired\n";
                }
                if (RadioList_FurtherInasiveItems.Where(c => c.IsSelected).Any() == false)
                {
                    errorMessage += "\nFurther Invasive Review Required Yes/No required\n";
                }
                if (RadioList_ConditionAssessment.Where(c => c.IsSelected).Any() == false)
                {
                    errorMessage += "\nCondition Assessment Required Yes/No required\n";
                }
                if (RadioList_LifeExpectancyEEE.Where(c => c.IsSelected).Any() == false)
                {
                    errorMessage += "\nLife Expectancy Exterior Elevated Elements (EEE) required\n";
                }
                if (RadioList_LifeExpectancyLBC.Where(c => c.IsSelected).Any() == false)
                {
                    errorMessage += "\nLife Expectancy Load Bearing Componenets (LBC) required\n";
                }
                if (RadioList_LifeExpectancyAWE.Where(c => c.IsSelected).Any() == false)
                {
                    errorMessage += "\nLife Expectancy Associated Waterproofing Elements (AWE) required\n";
                }
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    response.Message = errorMessage;
                    response.Status = ApiResult.Fail;

                    // await Shell.Current.DisplayAlert("Validation Error", errorMessage, "OK");
                }
                else
                {
                    // VisualForm.CreatedOn = DateTime.Now.ToString("dd-MMM-yyy hh:mm:ss");


                    VisualForm.ExteriorElements = string.Join(",", ExteriorElements.ToArray());
                    VisualForm.WaterProofingElements = string.Join(",", WaterProofingElements.ToArray());

                    VisualForm.VisualReview = RadioList_VisualReviewItems.Where(c => c.IsSelected == true).Single().Name;
                    VisualForm.AnyVisualSign = RadioList_AnyVisualSignItems.Where(c => c.IsSelected == true).Single().Name;

                    VisualForm.FurtherInasive = RadioList_FurtherInasiveItems.Where(c => c.IsSelected == true).Single().Name;

                    VisualForm.ConditionAssessment = RadioList_ConditionAssessment.Where(c => c.IsSelected == true).Single().Name;

                    VisualForm.LifeExpectancyEEE = RadioList_LifeExpectancyEEE.Where(c => c.IsSelected == true).Single().Name;

                    VisualForm.LifeExpectancyLBC = RadioList_LifeExpectancyLBC.Where(c => c.IsSelected == true).Single().Name;

                    VisualForm.LifeExpectancyAWE = RadioList_LifeExpectancyAWE.Where(c => c.IsSelected == true).Single().Name;


                    if (await VisualFormProjectLocationDataStore.GetItemAsync(VisualForm.Id) == null)
                    {
                        List<string> list = VisualProjectLocationPhotoItems.Select(c => c.ImageUrl).ToList();
                        response = await VisualFormProjectLocationDataStore.AddItemAsync(VisualForm, list);
                        // VisualProjectLocationPhotoItems.Clear();


                    }
                    else
                    {
                        List<MultiImage> finelList = new List<MultiImage>();
                        response = await VisualFormProjectLocationDataStore.UpdateItemAsync(VisualForm, App.VisualEditTracking);
                        // VisualProjectLocationPhotoItems.Clear();

                    }


                }
            }

            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = ApiResult.Fail;

            }
            return await Task.FromResult(response);

        }
        public ObservableCollection<CustomRadioItem> RadioList_VisualReviewItems { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_AnyVisualSignItems { get; set; }

        public ObservableCollection<CustomRadioItem> RadioList_FurtherInasiveItems { get; set; }


        public ObservableCollection<CustomRadioItem> RadioList_ConditionAssessment { get; set; }

        public ObservableCollection<CustomRadioItem> RadioList_LifeExpectancyEEE { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_LifeExpectancyLBC { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_LifeExpectancyAWE { get; set; }

        // public VisualFormProjectLocation MyProperty { get; set; }
        private ProjectLocation_Visual visualForm;

        public ProjectLocation_Visual VisualForm
        {
            get { return visualForm; }
            set { visualForm = value; OnPropertyChanged("VisualForm"); }
        }

        public InvasiveVisualProjectLocationFormViewModel()
        {

            RadioList_VisualReviewItems = new ObservableCollection<CustomRadioItem>();
            RadioList_VisualReviewItems.Add(new CustomRadioItem() { ID = 1, Name = "Good", IsSelected = false, GroupName = "VR" });
            RadioList_VisualReviewItems.Add(new CustomRadioItem() { ID = 2, Name = "Bad", IsSelected = false, GroupName = "VR" });
            RadioList_VisualReviewItems.Add(new CustomRadioItem() { ID = 3, Name = "Fair", IsSelected = false, GroupName = "VR" });

            RadioList_AnyVisualSignItems = new ObservableCollection<CustomRadioItem>();
            RadioList_AnyVisualSignItems.Add(new CustomRadioItem() { ID = 1, Name = "Yes", IsSelected = false, GroupName = "AVS" });
            RadioList_AnyVisualSignItems.Add(new CustomRadioItem() { ID = 2, Name = "No", IsSelected = false, GroupName = "AVS" });


            RadioList_FurtherInasiveItems = new ObservableCollection<CustomRadioItem>();
            RadioList_FurtherInasiveItems.Add(new CustomRadioItem() { ID = 1, Name = "Yes", IsSelected = false, GroupName = "FIR" });
            RadioList_FurtherInasiveItems.Add(new CustomRadioItem() { ID = 2, Name = "No", IsSelected = false, GroupName = "FIR" });

            RadioList_ConditionAssessment = new ObservableCollection<CustomRadioItem>();
            RadioList_ConditionAssessment.Add(new CustomRadioItem() { ID = 1, Name = "Pass", IsSelected = false, GroupName = "CA" });
            RadioList_ConditionAssessment.Add(new CustomRadioItem() { ID = 2, Name = "Fail", IsSelected = false, GroupName = "CA" });
            RadioList_ConditionAssessment.Add(new CustomRadioItem() { ID = 3, Name = "Future Inspection", IsSelected = false, GroupName = "CA" });

            RadioList_LifeExpectancyEEE = new ObservableCollection<CustomRadioItem>();
            RadioList_LifeExpectancyEEE.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsSelected = false, GroupName = "EEE" });
            RadioList_LifeExpectancyEEE.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsSelected = false, GroupName = "EEE" });
            RadioList_LifeExpectancyEEE.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsSelected = false, GroupName = "EEE" });
            RadioList_LifeExpectancyEEE.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsSelected = false, GroupName = "EEE" });



            //RadioList_LifeExpectancyLBC = new ObservableCollection<CustomRadioItem>();
            //RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsSelected = false, GroupName = "LBC" });
            //RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsSelected = false, GroupName = "LBC" });
            //RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsSelected = false, GroupName = "LBC" });
            //RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsSelected = false, GroupName = "LBC" });


            RadioList_LifeExpectancyLBC = new ObservableCollection<CustomRadioItem>();
            RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsSelected = false, GroupName = "LBC" });
            RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsSelected = false, GroupName = "LBC" });
            RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsSelected = false, GroupName = "LBC" });
            RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsSelected = false, GroupName = "LBC" });


            RadioList_LifeExpectancyAWE = new ObservableCollection<CustomRadioItem>();
            RadioList_LifeExpectancyAWE.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsSelected = false, GroupName = "AWE" });
            RadioList_LifeExpectancyAWE.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsSelected = false, GroupName = "AWE" });
            RadioList_LifeExpectancyAWE.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsSelected = false, GroupName = "AWE" });
            RadioList_LifeExpectancyAWE.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsSelected = false, GroupName = "AWE" });


            GoBackCommand = new Command(async () => await GoBack());
            SaveCommand = new Command(async () => await Save());
            ExteriorElements = new ObservableCollection<string>();
            WaterProofingElements = new ObservableCollection<string>();
            //MessagingCenter.Subscribe<ImageEditor.Pages.ImageEditorPage, string>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as string;
            //    await App.Current.MainPage.DisplayAlert(newItem,newItem,"ok","cancel");
            //});
            //LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            //Load();
            ImgData = new ImageData();
            MessagingCenter.Subscribe<PopUpCheakListBox, ObservableCollection<string>>(this, "SelectedItem", (obj, item) =>
           {
               ExteriorElements = item as ObservableCollection<string>;
               CountExteriorElements = ExteriorElements.Count.ToString();


           });
            MessagingCenter.Subscribe<PopUpCheakListBoxWaterProofing, ObservableCollection<string>>(this, "SelectedItem", (obj, item) =>
           {
               WaterProofingElements = item as ObservableCollection<string>;
               CountWaterProofingElements = WaterProofingElements.Count.ToString();


           });
            //MessagingCenter.Subscribe<Camera2Forms.CameraPage, string>(this, "Count", async (obj, item) =>
            //{
            //    //UnitPhotos.Clear();
            //    //ObservableCollection <VisualProjectLocationPhoto> listTempImage = item as ObservableCollection<VisualProjectLocationPhoto>;

            //    //foreach (var data in listTempImage)
            //    //{
            //    //    UnitPhotos.Add(data);
            //    //}
            //    UnitPhotoCount = item as string;

            //});
            //MessagingCenter.Subscribe<Camera2Forms.CameraPage, ObservableCollection<MultiImage>>(this, "ImageList", async (obj, item) =>
            //{
            //    var items = item as ObservableCollection<MultiImage>;

            //    foreach (var photo in items)
            //    {
            //        VisualProjectLocationPhoto newObj = new VisualProjectLocationPhoto() { ImageUrl = photo.Image, Id = Guid.NewGuid().ToString(), VisualLocationId = VisualForm.Id };
            //        _ = AddNewPhoto(newObj);
            //     //   await VisualProjectLocationPhotoDataStore.AddItemAsync(newObj);

            //    }
            //    //await Load();
            // //   VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id));
            //    //UnitPhotoCount = VisualProjectLocationPhotoItems.Count.ToString();
            //    //  UnitPhotoCount = (await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(ProjectLocation.Id)).Count().ToString();

            //});

        }
        private string _countExteriorElements;

        public string CountExteriorElements
        {
            get { return _countExteriorElements; }
            set { _countExteriorElements = value; OnPropertyChanged("CountExteriorElements"); }
        }

        private string _countWaterProofingElements;

        public string CountWaterProofingElements
        {
            get { return _countWaterProofingElements; }
            set { _countWaterProofingElements = value; OnPropertyChanged("CountWaterProofingElements"); }
        }

        private ObservableCollection<VisualProjectLocationPhoto> _visualProjectLocationPhotoItems;

        public ObservableCollection<VisualProjectLocationPhoto> VisualProjectLocationPhotoItems
        {
            // get { return _visualProjectLocationPhotoItems ; }
            get { return _visualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(_visualProjectLocationPhotoItems.Where(c => c.InvasiveImage == true)); }
            set { _visualProjectLocationPhotoItems = value; OnPropertyChanged("VisualProjectLocationPhotoItems"); }
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
                await Load();
            }


        }


        public ICommand ImageDetailCommand => new Command<BindingModel>(async (BindingModel parm) => await ImageDetailCommandCommandExecute(parm));

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
        private async void GetImage(ImageData ImgData)
        {

            await Load();
        }
        private bool _isEdit;

        public bool IsEdit
        {
            get { return _isEdit; }
            set { _isEdit = value; OnPropertyChanged("IsEdit"); }
        }
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

        private ObservableCollection<VisualProjectLocationPhoto> _visualProjectLocationPhotoItemsInvasive;

        public ObservableCollection<VisualProjectLocationPhoto> VisualProjectLocationPhotoItemsInvasive
        {
            get
            {


                var itemsInvasive = new ObservableCollection<VisualProjectLocationPhoto>(VisualProjectLocationPhotoItems.Where(c => c.InvasiveImage == true));
                return itemsInvasive;


            }
            set { _visualProjectLocationPhotoItemsInvasive = value; OnPropertyChanged("VisualProjectLocationPhotoItemsInvasive"); }
            //   set { _visualProjectLocationPhotoItemsInvasive = value; OnPropertyChanged("VisualProjectLocationPhotoItems"); }
        }
        private string _unitPhotCount;

        public string UnitPhotoCount
        {
            get { return _unitPhotCount; }
            set { _unitPhotCount = value; OnPropertyChanged("UnitPhotoCount"); }
        }

        //  public ObservableCollection<VisualProjectLocationPhoto> UnitPhotos { get; set; }
        private ObservableCollection<VisualProjectLocationPhoto> _unitPhoto;

        public ObservableCollection<VisualProjectLocationPhoto> UnitPhotos
        {
            get { return _unitPhoto; }
            set { _unitPhoto = value; OnPropertyChanged("UnitPhotos"); }
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
                        await Shell.Current.Navigation.PushAsync(new UnitPhtoForm() { BindingContext = new UnitPhotoViewModel() { ProjectLocation_Visual = VisualForm, IsVisualProjectLocatoion = true, IsEdit = true } });
                    }
                    else
                    {
                        await Shell.Current.Navigation.PushAsync(new UnitPhtoForm() { BindingContext = new UnitPhotoViewModel() { ProjectLocation_Visual = VisualForm, IsVisualProjectLocatoion = true, IsEdit = true } });
                    }
                }
            }
        }
        public ICommand ViusalReviewDetailCommand => new Command<CustomRadioItem>(async (CustomRadioItem parm) => await ExecuteViusalReviewDetailCommand(parm));

        private Task ExecuteViusalReviewDetailCommand(CustomRadioItem parm)
        {
            throw new NotImplementedException();
        }
        private async Task<bool> Running()
        {

            UnitPhotos = new ObservableCollection<VisualProjectLocationPhoto>();
            if (VisualForm != null)
            {
                if (string.IsNullOrEmpty(visualForm.Id))
                {
                    VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id, false));
                }
                else
                {
                    App.IsVisualEdidingMode = true;
                    VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>((await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id, false)));

                    // VisualProjectLocationPhotoItems=new ObservableCollection<VisualProjectLocationPhoto>(VisualProjectLocationPhotoItems.Where(c=>c.InvasiveImage==true))
                    // VisualProjectLocationPhotoItems = VisualProjectLocationPhotoItems.Where(c => c.IsDelete = false).ToList();
                    //if (App.IsVisualEdidingMode == true)
                    //{
                    // VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id, true));
                    //    App.IsVisualEdidingMode = false;
                    //}
                    //else
                    //{
                    //    VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id, false));

                    //}

                }
                UnitPhotoCount = VisualProjectLocationPhotoItems.Count.ToString();
            }
            return await Task.FromResult(true);
        }
        public async Task<bool> Load()
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
        public ICommand ChoosePhotoFromCameraCommand => new Command(async () => await ChoosePhotoFromCameraCommandExecute());

        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }
        private async Task ChoosePhotoFromCameraCommandExecute()
        {
            string selectedOption = await App.Current.MainPage.DisplayActionSheet("Select Option", "Cancel", null,
               new string[] { "Take New Photo", "From Gallery" });

            switch (selectedOption)
            {
                case "Take New Photo":
                    await Shell.Current.Navigation.PushModalAsync(new Camera2Forms.CameraPage() { BindingContext = new CameraViewModel() { ProjectLocation_Visual = VisualForm, IsVisualProjectLocatoion = true } });
                    break;
                case "From Gallery":
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        //If the image is modified (drawings, etc) by the users, you will need to change the delivery mode to HighQualityFormat.
                        bool imageModifiedWithDrawings = true;
                        if (imageModifiedWithDrawings)
                        {
                            await GMMultiImagePicker.Current.PickMultiImage(true);
                        }
                        else
                        {
                            await GMMultiImagePicker.Current.PickMultiImage();
                        }

                        MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS");
                        MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS", (s, images) =>
                        {
                            //If we have selected images, put them into the carousel view.
                            if (images.Count > 0)
                            {
                                foreach (var item in images)
                                {
                                    VisualProjectLocationPhoto obj = new VisualProjectLocationPhoto() { ImageUrl = item, Id = Guid.NewGuid().ToString(), VisualLocationId = VisualForm.Id };
                                    _ = AddNewPhoto(obj);
                                }
                            }
                        });
                    }

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        DependencyService.Get<IMediaService>().OpenGallery();
                        MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid");
                        MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid", (s, images) =>
                        {
                            foreach (var item in images)
                            {
                                VisualProjectLocationPhoto obj = new VisualProjectLocationPhoto() { ImageUrl = item, Id = Guid.NewGuid().ToString(), VisualLocationId = VisualForm.Id };
                                _ = AddNewPhoto(obj);
                            }
                            //If we have selected images, put them into the carousel view.
                            //if (images.Count > 0)
                            //{

                            //    // ImgCarouselView.ItemsSource = images;
                            //    //InfoText.IsVisible = true; //InfoText is optional
                            //}
                        });
                    }

                    break;
                default:
                    break;
            }

        }
        public async Task AddNewPhoto(VisualProjectLocationPhoto obj)
        {

            await VisualProjectLocationPhotoDataStore.AddItemAsync(obj).ConfigureAwait(false);
            VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id, false));
            UnitPhotoCount = VisualProjectLocationPhotoItems.Count.ToString();
        }

        public ICommand ChooseExteriorCommand => new Command(async () => await ChooseExteriorCommandCommandExecute());
        private async Task ChooseExteriorCommandCommandExecute()
        {
            await Shell.Current.Navigation.PushModalAsync(new PopUpCheakListBox() { BindingContext = new PopUpCheakListBoxViewModel() { CheakBoxSelectedItems = ExteriorElements } });
        }

        public ICommand ChooseWaterproofingCommand => new Command(async () => await ChooseWaterproofingCommandCommandExecute());
        private async Task ChooseWaterproofingCommandCommandExecute()
        {
            await Shell.Current.Navigation.PushModalAsync(new PopUpCheakListBoxWaterProofing() { BindingContext = new PopUpCheakListBoxWaterproofingViewModel() { CheakBoxSelectedItems = WaterProofingElements } });
        }
        private async Task<string> TakePictureFromLibrary()
        {
            IsBusy = true;
            var file = await CrossMedia.Current.PickPhotoAsync
                (new PickMediaOptions()
                {
                    SaveMetaData = false,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    CompressionQuality = App.CompressionQuality


                }); ;

            IsBusy = false;
            if (file == null)
                return null;

            return file.Path;

        }

        private async Task<string> TakePictureFromCamera()
        {
            IsBusy = true;
            var file = await CrossMedia.Current.TakePhotoAsync
                (new StoreCameraMediaOptions()
                {
                    SaveMetaData = true,
                    DefaultCamera = CameraDevice.Rear,
                    CompressionQuality = App.CompressionQuality
                });

            IsBusy = false;
            if (file == null)
                return null;

            return file.Path;
        }
        private string _imgPath;

        public string ImgPata
        {
            get { return _imgPath; }
            set { _imgPath = value; OnPropertyChanged(); }
        }

        private void testphoto(ImageData ImgData)
        {

        }
    }
}