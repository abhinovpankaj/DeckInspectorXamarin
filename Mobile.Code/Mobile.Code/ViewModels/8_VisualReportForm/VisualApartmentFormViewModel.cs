using ImageEditor.ViewModels;
using Mobile.Code.Media;
using Mobile.Code.Models;
using Mobile.Code.Views;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class VisualApartmentFormViewModel : BaseViewModel
    {
        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }
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

        public bool CheckAnyRadioButtonChecked { get; set; }

        private async Task GoBack()
        {
            if (App.IsInvasive == true)
            {
                string currentForm = JsonConvert.SerializeObject(VisualForm);
                int check = string.Compare(App.FormString, currentForm);
                string currentImage = JsonConvert.SerializeObject(InvasiveVisualApartmentLocationPhotoItems);
                int countImage = string.Compare(App.InvaiveImages, currentImage);
                if (check == 0 && countImage == 0)
                {
                    await Shell.Current.Navigation.PopAsync();
                    App.IsNewForm = false;
                    return;

                }
            }
            else
            {
                if (App.IsNewForm == true)
                {
                    string currentForm = JsonConvert.SerializeObject(VisualForm);
                    int check = string.Compare(App.FormString, currentForm);
                    int countImage = VisualApartmentLocationPhotoItems.Select(c => c.ImageUrl).Count();
                    bool checkElements = false;


                    if (ExteriorElements.Count != 0)
                    {
                        checkElements = true;
                    }
                    if (WaterProofingElements.Count != 0)
                    {
                        checkElements = true;
                    }
                    if (CheckAnyRadioButtonChecked == true)
                    {
                        checkElements = true;
                    }
                    //VisualForm.ExteriorElements = string.Join(",", ExteriorElements.ToArray());
                    //VisualForm.WaterProofingElements = string.Join(",", WaterProofingElements.ToArray());

                    //VisualForm.VisualReview = RadioList_VisualReviewItems.Where(c => c.IsChecked == true).Single().Name;
                    //VisualForm.AnyVisualSign = RadioList_AnyVisualSignItems.Where(c => c.IsChecked == true).Single().Name;

                    //VisualForm.FurtherInasive = RadioList_FurtherInasiveItems.Where(c => c.IsChecked == true).Single().Name;

                    //VisualForm.ConditionAssessment = RadioList_ConditionAssessment.Where(c => c.IsChecked == true).Single().Name;

                    //VisualForm.LifeExpectancyEEE = RadioList_LifeExpectancyEEE.Where(c => c.IsChecked == true).Single().Name;

                    //VisualForm.LifeExpectancyLBC = RadioList_LifeExpectancyLBC.Where(c => c.IsChecked == true).Single().Name;

                    //VisualForm.LifeExpectancyAWE = RadioList_LifeExpectancyAWE.Where(c => c.IsChecked == true).Single().Name;
                    if (check == 0 && countImage == 0 && checkElements == false)
                    {
                        await Shell.Current.Navigation.PopAsync();
                        App.IsNewForm = false;
                        return;

                    }
                }
                else
                {
                    string currentForm = JsonConvert.SerializeObject(VisualForm);
                    int check = string.Compare(App.FormString, currentForm);

                    bool checkElements = false;
                  
                    Apartment_Visual old = JsonConvert.DeserializeObject<Apartment_Visual>(App.FormString);

                    string Exte = string.Join(",", ExteriorElements.ToArray());
                    string WateE = string.Join(",", WaterProofingElements.ToArray());
                    int checkExElements = string.Compare(old.ExteriorElements, Exte);
                    int checkWFElements = string.Compare(old.WaterProofingElements, WateE);
                    if (checkWFElements != 0)
                    {
                        checkElements = true;
                    }
                    if (checkExElements != 0)
                    {
                        checkElements = true;
                    }
                    if (old.VisualReview != RadioList_VisualReviewItems.Where(c => c.IsChecked == true).Single().Name)
                    {
                        checkElements = true;
                    }
                    if (old.AnyVisualSign != RadioList_AnyVisualSignItems.Where(c => c.IsChecked == true).Single().Name)
                    {
                        checkElements = true;
                    }

                    if (old.FurtherInasive != RadioList_FurtherInasiveItems.Where(c => c.IsChecked == true).Single().Name)
                    {
                        checkElements = true;
                    }

                    if (old.ConditionAssessment != RadioList_ConditionAssessment.Where(c => c.IsChecked == true).Single().Name)
                    {
                        checkElements = true;
                    }

                    if (old.LifeExpectancyEEE != RadioList_LifeExpectancyEEE.Where(c => c.IsChecked == true).Single().Name)
                    {
                        checkElements = true;
                    }

                    if (old.LifeExpectancyLBC != RadioList_LifeExpectancyLBC.Where(c => c.IsChecked == true).Single().Name)
                    {
                        checkElements = true;
                    }

                    if (old.LifeExpectancyAWE != RadioList_LifeExpectancyAWE.Where(c => c.IsChecked == true).Single().Name)
                    {
                        checkElements = true;
                    }
                    string newImages = JsonConvert.SerializeObject(App.VisualEditTracking);

                    int checkImages = string.Compare(App.ImageFormString, newImages);
                    if (checkImages != 0)
                    {
                        checkElements = true;
                    }
                    //if (App.VisualEditTracking!=null)
                    //{
                    //    checkElements = true;
                    //}
                    //if (CheckAnyRadioButtonChecked == true)
                    //{
                    //    checkElements = true;
                    //}
                    if (check == 0 && checkElements == false)
                    {
                        await Shell.Current.Navigation.PopAsync();
                        return;

                    }
                }
            }

            var result = await Shell.Current.DisplayAlert(
                  "Alert",
                  "Are you sure you want to go back?",
                  "Yes", "No");



            if (result)
            {
                await Shell.Current.Navigation.PopAsync();
               
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
        private BuildingApartment _buildingLocation;
        public BuildingApartment BuildingApartment
        {
            get { return _buildingLocation; }
            set { _buildingLocation = value; OnPropertyChanged("BuildingApartment"); }
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
                if (RadioList_VisualReviewItems.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nVisual Review required\n";
                }
                if (RadioList_AnyVisualSignItems.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nAny visual signs of leaksrequired\n";
                }
                if (RadioList_FurtherInasiveItems.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nFurther Invasive Review Required Yes/No required\n";
                }
                if (RadioList_ConditionAssessment.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nCondition Assessment Required Yes/No required\n";
                }
                if (RadioList_LifeExpectancyEEE.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nLife Expectancy Exterior Elevated Elements (EEE) required\n";
                }
                if (RadioList_LifeExpectancyLBC.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nLife Expectancy Load Bearing Componenets (LBC) required\n";
                }
                if (RadioList_LifeExpectancyAWE.Where(c => c.IsChecked).Any() == false)
                {
                    errorMessage += "\nLife Expectancy Associated Waterproofing Elements (AWE) required\n";
                }
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    response.Message = errorMessage;
                    response.Status = ApiResult.Fail;
                   // return await Task.FromResult(response);
                }
                else
                {
                    VisualForm.CreatedOn = DateTime.Now.ToString("dd-MMM-yyy hh:mm:ss");


                    VisualForm.ExteriorElements = string.Join(",", ExteriorElements.ToArray());
                    VisualForm.WaterProofingElements = string.Join(",", WaterProofingElements.ToArray());

                    VisualForm.VisualReview = RadioList_VisualReviewItems.Where(c => c.IsChecked == true).Single().Name;
                    VisualForm.AnyVisualSign = RadioList_AnyVisualSignItems.Where(c => c.IsChecked == true).Single().Name;

                    VisualForm.FurtherInasive = RadioList_FurtherInasiveItems.Where(c => c.IsChecked == true).Single().Name;

                    VisualForm.ConditionAssessment = RadioList_ConditionAssessment.Where(c => c.IsChecked == true).Single().Name;

                    VisualForm.LifeExpectancyEEE = RadioList_LifeExpectancyEEE.Where(c => c.IsChecked == true).Single().Name;

                    VisualForm.LifeExpectancyLBC = RadioList_LifeExpectancyLBC.Where(c => c.IsChecked == true).Single().Name;

                    VisualForm.LifeExpectancyAWE = RadioList_LifeExpectancyAWE.Where(c => c.IsChecked == true).Single().Name;


                    if (await VisualFormApartmentDataStore.GetItemAsync(VisualForm.Id) == null)
                    {
                        List<string> list = VisualApartmentLocationPhotoItems.Select(c => c.ImageUrl).ToList();
                        response = await VisualFormApartmentDataStore.AddItemAsync(VisualForm, list);
                       // return await Task.FromResult(response);
                    }
                    else
                    {
                        List<MultiImage> finelList = new List<MultiImage>();
                        if (App.IsInvasive == false)
                            response = await VisualFormApartmentDataStore.UpdateItemAsync(VisualForm, App.VisualEditTracking);
                         else
                        {

                            if (InvasiveVisualApartmentLocationPhotoItems.Count == 0)
                            {
                                errorMessage += "\nInvasive photo required\n"; ;

                            }
                            if (string.IsNullOrEmpty(visualForm.ImageDescription))
                            {
                                errorMessage += "\nDescription required\n"; ;

                            }
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                response.Message = errorMessage;
                                response.Status = ApiResult.Fail;

                                // await Shell.Current.DisplayAlert("Validation Error", errorMessage, "OK");
                            }
                            else
                            {

                                response = await VisualFormApartmentDataStore.UpdateItemAsync(VisualForm, App.VisualEditTrackingForInvasive);
                            }

                        }
                        //await VisualFormApartmentDataStore.UpdateItemAsync(VisualForm);
                        // return await Task.FromResult(response);
                    }


                }
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                response.Status = ApiResult.Fail;
            }

            return await Task.FromResult(response);
        }
        private ObservableCollection<VisualApartmentLocationPhoto> _InvvisualInvasiveVisualApartmentLocationPhotoItems;

        public ObservableCollection<VisualApartmentLocationPhoto> InvasiveVisualApartmentLocationPhotoItems
        {
            // get { return _InvvisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(_visualProjectLocationPhotoItems.Where(c => c.InvasiveImage == false)); }
            get { return _InvvisualInvasiveVisualApartmentLocationPhotoItems; }
            set { _InvvisualInvasiveVisualApartmentLocationPhotoItems = value; OnPropertyChanged("InvasiveVisualApartmentLocationPhotoItems"); }
        }
        public ObservableCollection<CustomRadioItem> RadioList_VisualReviewItems { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_AnyVisualSignItems { get; set; }

        public ObservableCollection<CustomRadioItem> RadioList_FurtherInasiveItems { get; set; }


        public ObservableCollection<CustomRadioItem> RadioList_ConditionAssessment { get; set; }

        public ObservableCollection<CustomRadioItem> RadioList_LifeExpectancyEEE { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_LifeExpectancyLBC { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_LifeExpectancyAWE { get; set; }

       // public VisualFormProjectLocation MyProperty { get; set; }
        private Apartment_Visual visualForm;

        public Apartment_Visual VisualForm
        {
            get { return visualForm; }
            set { visualForm = value; OnPropertyChanged("VisualForm"); }
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
        public VisualApartmentFormViewModel()
        {
            IsVisualApartment = true;
            IsVisualProjectLocatoion = false;
            IsVisualBuilding = false;
            RadioList_VisualReviewItems = new ObservableCollection<CustomRadioItem>();
            RadioList_VisualReviewItems.Add(new CustomRadioItem() { ID = 1, Name = "Good", IsChecked = false , GroupName ="VR"});
            RadioList_VisualReviewItems.Add(new CustomRadioItem() { ID = 2, Name = "Bad", IsChecked = false, GroupName = "VR" });
            RadioList_VisualReviewItems.Add(new CustomRadioItem() { ID = 3, Name = "Fair", IsChecked = false, GroupName = "VR" });

            RadioList_AnyVisualSignItems = new ObservableCollection<CustomRadioItem>();
            RadioList_AnyVisualSignItems.Add(new CustomRadioItem() { ID = 1, Name = "Yes", IsChecked = false, GroupName = "AVS" });
            RadioList_AnyVisualSignItems.Add(new CustomRadioItem() { ID = 2, Name = "No", IsChecked = false, GroupName = "AVS" });


            RadioList_FurtherInasiveItems = new ObservableCollection<CustomRadioItem>();
            RadioList_FurtherInasiveItems.Add(new CustomRadioItem() { ID = 1, Name = "Yes", IsChecked = false, GroupName = "FIR" });
            RadioList_FurtherInasiveItems.Add(new CustomRadioItem() { ID = 2, Name = "No", IsChecked = false, GroupName = "FIR" });

            RadioList_ConditionAssessment = new ObservableCollection<CustomRadioItem>();
            RadioList_ConditionAssessment.Add(new CustomRadioItem() { ID = 1, Name = "Pass", IsChecked = false, GroupName = "CA" });
            RadioList_ConditionAssessment.Add(new CustomRadioItem() { ID = 2, Name = "Fail", IsChecked = false, GroupName = "CA" });
            RadioList_ConditionAssessment.Add(new CustomRadioItem() { ID = 3, Name = "Future Inspection", IsChecked = false, GroupName = "CA" });

            RadioList_LifeExpectancyEEE = new ObservableCollection<CustomRadioItem>();
            RadioList_LifeExpectancyEEE.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsChecked = false, GroupName = "EEE" });
            RadioList_LifeExpectancyEEE.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsChecked = false, GroupName = "EEE" });
            RadioList_LifeExpectancyEEE.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsChecked = false, GroupName = "EEE" });
            RadioList_LifeExpectancyEEE.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsChecked = false, GroupName = "EEE" });



            //RadioList_LifeExpectancyLBC = new ObservableCollection<CustomRadioItem>();
            //RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsChecked = false, GroupName = "LBC" });
            //RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsChecked = false, GroupName = "LBC" });
            //RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsChecked = false, GroupName = "LBC" });
            //RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsChecked = false, GroupName = "LBC" });


            RadioList_LifeExpectancyLBC = new ObservableCollection<CustomRadioItem>();
            RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsChecked = false, GroupName = "LBC" });
            RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsChecked = false, GroupName = "LBC" });
            RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsChecked = false, GroupName = "LBC" });
            RadioList_LifeExpectancyLBC.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsChecked = false, GroupName = "LBC" });


            RadioList_LifeExpectancyAWE = new ObservableCollection<CustomRadioItem>();
            RadioList_LifeExpectancyAWE.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsChecked = false, GroupName = "AWE" });
            RadioList_LifeExpectancyAWE.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsChecked = false, GroupName = "AWE" });
            RadioList_LifeExpectancyAWE.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsChecked = false, GroupName = "AWE" });
            RadioList_LifeExpectancyAWE.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsChecked = false, GroupName = "AWE" });


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
            MessagingCenter.Subscribe<PopUpCheakListBox, ObservableCollection<string>>(this, "SelectedItem",  (obj, item) =>
            {
                ExteriorElements = item as ObservableCollection<string>;
                CountExteriorElements = ExteriorElements.Count.ToString();


            });
            MessagingCenter.Subscribe<PopUpCheakListBoxWaterProofing, ObservableCollection<string>>(this, "SelectedItem", (obj, item) =>
            {
                WaterProofingElements = item as ObservableCollection<string>;
                CountWaterProofingElements = WaterProofingElements.Count.ToString();


            });
            
            App.ListCamera2Api = new List<MultiImage>();
        }
        private string _countExteriorElements;

        public string CountExteriorElements
        {
            get { return _countExteriorElements; }
            set { _countExteriorElements = value;OnPropertyChanged("CountExteriorElements"); }
        }

        private string _countWaterProofingElements;

        public string CountWaterProofingElements
        {
            get { return _countWaterProofingElements; }
            set { _countWaterProofingElements = value; OnPropertyChanged("CountWaterProofingElements"); }
        }

        private ObservableCollection<VisualApartmentLocationPhoto> _visualApartmentLocationPhotoItems;

        public ObservableCollection<VisualApartmentLocationPhoto> VisualApartmentLocationPhotoItems
        {
            get { return _visualApartmentLocationPhotoItems; }
            set { _visualApartmentLocationPhotoItems = value; OnPropertyChanged("VisualApartmentLocationPhotoItems"); }
        }
        private string _unitPhotCount;

        public string UnitPhotoCount
        {
            get { return _unitPhotCount; }
            set { _unitPhotCount = value; OnPropertyChanged("UnitPhotoCount"); }
        }
        private string _InvunitPhotCount;

        public string InvasiveUnitPhotoCount
        {
            get { return _InvunitPhotCount; }
            set { _InvunitPhotCount = value; OnPropertyChanged("InvasiveUnitPhotoCount"); }
        }
        //  public ObservableCollection<VisualProjectLocationPhoto> UnitPhotos { get; set; }
        private ObservableCollection<VisualApartmentLocationPhoto> _unitPhoto;

        public ObservableCollection<VisualApartmentLocationPhoto> UnitPhotos
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
                    //Later
                    await Shell.Current.Navigation.PushAsync(new UnitPhtoForm() { BindingContext = new UnitPhotoViewModel() { Apartment_Visual = VisualForm, IsVisualApartment = true } });
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

            if (App.ListCamera2Api != null)
            {

                foreach (var photo in App.ListCamera2Api)
                {
                    VisualApartmentLocationPhoto newObj = new VisualApartmentLocationPhoto() { ImageUrl = photo.Image, Id = Guid.NewGuid().ToString(), VisualApartmentId = VisualForm.Id, DateCreated = DateTime.Now };
                    if (App.IsInvasive == true)
                    {

                        _ = AddNewPhoto(newObj).ConfigureAwait(false);
                    }
                    else
                    {

                        _ = AddNewPhoto(newObj).ConfigureAwait(false);
                        //  await VisualProjectLocationPhotoDataStore.AddItemAsync(newObj);
                    }
                }
                App.ListCamera2Api.Clear();
            }
            UnitPhotos = new ObservableCollection<VisualApartmentLocationPhoto>();
            if (VisualForm != null)
            {
                if (string.IsNullOrEmpty(VisualForm.Id))
                {
                    VisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id,false));
                }
                else
                {
                    VisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id,false));
                    if (App.IsInvasive == true)
                    {
                        InvasiveVisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>((await InvasiveVisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id, false)));
                        InvasiveUnitPhotoCount = InvasiveVisualApartmentLocationPhotoItems.Count.ToString();
                    }
                }
                UnitPhotoCount = VisualApartmentLocationPhotoItems.Count.ToString();
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
        private async Task ChoosePhotoFromCameraCommandExecute()
        {
            string selectedOption = await App.Current.MainPage.DisplayActionSheet("Select Option", "Cancel", null,
               new string[] { "Take New Photo", "From Gallery" });

            switch (selectedOption)
            {
                case "Take New Photo":

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        await Shell.Current.Navigation.PushModalAsync(new Camera2Forms.CameraPage() { BindingContext = new CameraViewModel() { Apartment_Visual = VisualForm, IsVisualApartment = true } });
                        //await Shell.Current.Navigation.PushModalAsync(new Camera2Forms.CameraPageForAndroid() { BindingContext = new CameraViewModel() { Apartment_Visual = VisualForm, IsVisualApartment = true } });
                    }
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        await Shell.Current.Navigation.PushModalAsync(new Camera2Forms.CameraPage() { BindingContext = new CameraViewModel() { Apartment_Visual = VisualForm, IsVisualApartment = true } });
                        // await Shell.Current.Navigation.PushModalAsync(new Camera2Forms.CameraPage() { BindingContext = new CameraViewModel() { Apartment_Visual = VisualForm, IsVisualApartment = true } });
                    }
                   
                    break;
                case "From Gallery":
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        //If the image is modified (drawings, etc) by the users, you will need to change the delivery mode to HighQualityFormat.
                        bool imageModifiedWithDrawings = false;
                        if (imageModifiedWithDrawings)
                        {
                            await GMMultiImagePicker.Current.PickMultiImage(true);
                        }
                        else
                        {
                            await GMMultiImagePicker.Current.PickMultiImage();
                        }

                        //  MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS");
                        MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS", (s, images) =>
                        {
                            //If we have selected images, put them into the carousel view.
                            if (images.Count > 0)
                            {
                                foreach (var item in images)
                                {
                                    VisualApartmentLocationPhoto obj = new VisualApartmentLocationPhoto() { ImageUrl = item, Id = Guid.NewGuid().ToString(), VisualApartmentId = VisualForm.Id, DateCreated = DateTime.Now };
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
                                VisualApartmentLocationPhoto obj = new VisualApartmentLocationPhoto() { ImageUrl = item, Id = Guid.NewGuid().ToString(), VisualApartmentId = VisualForm.Id, DateCreated = DateTime.Now };
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
        public async Task AddNewPhoto(VisualApartmentLocationPhoto obj)
        {
            if (App.IsInvasive == true)
            {
                // InvasiveVisualProjectLocationPhotoItems.Add(obj);
                await InvasiveVisualApartmentLocationPhotoDataStore.AddItemAsync(obj);
                InvasiveVisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(await InvasiveVisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id, false));
                InvasiveUnitPhotoCount = InvasiveVisualApartmentLocationPhotoItems.Count.ToString();
                return;
            }
            await VisualApartmentLocationPhotoDataStore.AddItemAsync(obj);
            VisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id,false));
            UnitPhotoCount = VisualApartmentLocationPhotoItems.Count.ToString();
        }
        public ICommand DeleteImageCommandLocation => new Command<VisualApartmentLocationPhoto>(async (VisualApartmentLocationPhoto parm) => await DeleteImageCommandCommandExecute(parm));
        private async Task DeleteImageCommandCommandExecute(VisualApartmentLocationPhoto parm)
        {
            var result = await Shell.Current.DisplayAlert(
                  "Alert",
                  "Are you sure you want to remove?",
                  "Yes", "No");



            if (result)
            {
                //if (parm.GetType() == typeof(VisualProjectLocationPhoto))
                //{
                VisualApartmentLocationPhoto obj = parm as VisualApartmentLocationPhoto;
                await InvasiveVisualApartmentLocationPhotoDataStore.DeleteItemAsync(obj);
                
                 await Load();
            }


        }

        public ICommand ImageDetailCommand => new Command<VisualApartmentLocationPhoto>(async (VisualApartmentLocationPhoto parm) => await ImageDetailCommandCommandExecute(parm));
        private async Task ImageDetailCommandCommandExecute(VisualApartmentLocationPhoto parm)
        {



            //if (parm.GetType() == typeof(VisualProjectLocationPhoto))
            //{
            //  VisualProjectLocationPhoto = parm as VisualProjectLocationPhoto;
            // VisualProjectLocationPhoto = parm;
            ImgData.Path = parm.ImageUrl;
            ImgData.ParentID = parm.VisualApartmentId;
            ImgData.VisualApartmentLocationPhoto = parm;
            ImgData.IsEditVisual = true;
            //ImgData.VisualProjectLocationPhotos = VisualProjectLocationPhotoItems;
            ImgData.FormType = "VA";
            await CurrentWithoutDetail.EditImage(ImgData, GetImage);



        }
        private async void GetImage(ImageData ImgData)
        {


            await Load();
        }
        public ICommand ChooseExteriorCommand => new Command(async () => await ChooseExteriorCommandCommandExecute());
        private async Task ChooseExteriorCommandCommandExecute()
        {
            await Shell.Current.Navigation.PushModalAsync(new PopUpCheakListBox() { BindingContext=new PopUpCheakListBoxViewModel() { CheakBoxSelectedItems = ExteriorElements } });
        }

        public ICommand ChooseWaterproofingCommand => new Command(async () => await ChooseWaterproofingCommandCommandExecute());
        private async Task ChooseWaterproofingCommandCommandExecute()
        {
            await Shell.Current.Navigation.PushModalAsync(new PopUpCheakListBoxWaterProofing() { BindingContext = new PopUpCheakListBoxWaterproofingViewModel() { CheakBoxSelectedItems = WaterProofingElements  } });
        }
        private async Task<string> TakePictureFromLibrary()
        {
            IsBusy = true;
            var file = await CrossMedia.Current.PickPhotoAsync
                (new PickMediaOptions()
                {
                    SaveMetaData = true,
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