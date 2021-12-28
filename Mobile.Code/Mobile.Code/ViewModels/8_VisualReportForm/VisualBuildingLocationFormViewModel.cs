using ImageEditor.ViewModels;
using Mobile.Code.Media;
using Mobile.Code.Models;
using Mobile.Code.Views;
using Mobile.Code.Views._8_VisualReportForm;
using Newtonsoft.Json;
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
    public class VisualBuildingLocationFormViewModel : BaseViewModel
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

        public Command SaveAndCreateNewCommand { get; set; }
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
                string currentImage = JsonConvert.SerializeObject(InvasiveVisualBuildingLocationPhotoItems);
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
                    int countImage = VisualBuildingLocationPhotoItems.Select(c => c.ImageUrl).Count();
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
                    //  int countImage = VisualProjectLocationPhotoItems.Select(c => c.ImageUrl).Count();

                    BuildingLocation_Visual old = JsonConvert.DeserializeObject<BuildingLocation_Visual>(App.FormString);

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
                    if (old.VisualReview != RadioList_VisualReviewItems.Where(c => c.IsSelected == true).Single().Name)
                    {
                        checkElements = true;
                    }
                    if (old.AnyVisualSign != RadioList_AnyVisualSignItems.Where(c => c.IsSelected == true).Single().Name)
                    {
                        checkElements = true;
                    }

                    if (old.FurtherInasive != RadioList_FurtherInasiveItems.Where(c => c.IsSelected == true).Single().Name)
                    {
                        checkElements = true;
                    }

                    if (old.ConditionAssessment != RadioList_ConditionAssessment.Where(c => c.IsSelected == true).Single().Name)
                    {
                        checkElements = true;
                    }

                    if (old.LifeExpectancyEEE != RadioList_LifeExpectancyEEE.Where(c => c.IsSelected == true).Single().Name)
                    {
                        checkElements = true;
                    }

                    if (old.LifeExpectancyLBC != RadioList_LifeExpectancyLBC.Where(c => c.IsSelected == true).Single().Name)
                    {
                        checkElements = true;
                    }

                    if (old.LifeExpectancyAWE != RadioList_LifeExpectancyAWE.Where(c => c.IsSelected == true).Single().Name)
                    {
                        checkElements = true;
                    }
                    string newImages = JsonConvert.SerializeObject(App.VisualEditTracking);

                    int checkImages = string.Compare(App.ImageFormString, newImages);
                    if (checkImages != 0)
                    {
                        checkElements = true;
                    }

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
        private BuildingLocation _buildingLocation;
        public BuildingLocation BuildingLocation
        {
            get { return _buildingLocation; }
            set { _buildingLocation = value; OnPropertyChanged("BuildingLocation"); }
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
        private async Task SaveCreateNew()
        {
            IsBusyProgress = true;
            Response result = await Task.Run(SaveLoad);
            if (result != null)
            {
                if (result.Status == ApiResult.Success)
                {
                    //Create New.
                    IsBusyProgress = false;
                    await Shell.Current.Navigation.PopAsync();
                    visualForm = new BuildingLocation_Visual();
                   
                    visualForm.BuildingLocationId = BuildingLocation.Id;
                    VisualBuildingLocationPhotoDataStore.Clear();

                    App.VisualEditTracking = new List<MultiImage>();
                    App.VisualEditTrackingForInvasive = new List<MultiImage>();
                    VisualBuildingLocationPhotoDataStore.Clear();
                    InvasiveVisualBuildingLocationPhotoDataStore.Clear();

                    App.FormString = JsonConvert.SerializeObject(visualForm);
                    App.IsNewForm = true;
                    
                    var locPage= new VisualBuildingLocationForm() { BindingContext = new VisualBuildingLocationFormViewModel() { BuildingLocation = BuildingLocation, VisualForm = visualForm } };
                    await Shell.Current.Navigation.PushAsync(locPage,true);
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
            string errorMessage = string.Empty;
            try
            {
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
                    //  await Shell.Current.DisplayAlert("Validation Error", errorMessage, "OK");
                    response.Message = errorMessage;
                    response.Status = ApiResult.Fail;
                    // return await Task.FromResult(response);
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

                    //Conclusive data

                    if (App.IsInvasive == true)
                    {
                        errorMessage = "";
                        if (RadioList_OwnerAgreedToRepair.FirstOrDefault(c => c.Name == "Yes").IsSelected)
                        {
                            VisualForm.IsInvasiveRepairApproved = true;
                            if (RadioList_RepairComplete.FirstOrDefault(c => c.Name == "Yes").IsSelected)
                            {
                                VisualForm.IsInvasiveRepairComplete = false;
                                VisualForm.IsInvasiveRepairComplete = RadioList_RepairComplete.Where(c => c.IsSelected == true).Single().Name == "Yes" ? true : false;
                                if (RadioList_ConclusiveLifeExpectancyAWE.Where(c => c.IsSelected).Any() == false)
                                {
                                    errorMessage += "\nConclusive: Life Expectancy Load Bearing Componenets (AWE) required\n";
                                }
                                if (RadioList_ConclusiveLifeExpectancyLBC.Where(c => c.IsSelected).Any() == false)
                                {
                                    errorMessage += "\nConclusive: Life Expectancy Load Bearing Componenets (LBC) required\n";
                                }
                                if (RadioList_ConclusiveLifeExpectancyEEE.Where(c => c.IsSelected).Any() == false)
                                {
                                    errorMessage += "\nConclusive: Life Expectancy Load Bearing Componenets (EEE) required\n";
                                }
                                if (!string.IsNullOrEmpty(errorMessage))
                                {
                                    response.Message = errorMessage;
                                    response.Status = ApiResult.Fail;
                                }
                                else
                                {
                                    VisualForm.ConclusiveLifeExpAWE = RadioList_ConclusiveLifeExpectancyAWE.Where(c => c.IsSelected == true).Single().Name;
                                    VisualForm.ConclusiveLifeExpLBC = RadioList_ConclusiveLifeExpectancyLBC.Where(c => c.IsSelected == true).Single().Name;
                                    VisualForm.ConclusiveLifeExpEEE = RadioList_ConclusiveLifeExpectancyEEE.Where(c => c.IsSelected == true).Single().Name;
                                }

                            }
                            else
                                VisualForm.IsInvasiveRepairComplete = false;
                        }
                        else
                            VisualForm.IsInvasiveRepairApproved = false;

                    }

                    if (App.IsAppOffline)
                    {
                        if (await VisualFormBuildingLocationSqLiteDataStore.GetItemAsync(VisualForm.Id) == null)
                        {
                            List<string> list = VisualBuildingLocationPhotoItems.Select(c => c.ImageUrl).ToList();
                            response = await VisualFormBuildingLocationSqLiteDataStore.AddItemAsync(VisualForm, list);
                            foreach (var item in VisualBuildingLocationPhotoItems)
                            {
                                item.VisualBuildingId = response.ID;
                                await VisualBuildingLocationPhotoDataStore.AddItemAsync(item, true);
                            }
                            //  return await Task.FromResult(response);
                        }
                        else
                        {
                            List<MultiImage> finelList = new List<MultiImage>();
                            if (App.IsInvasive == false)
                                response = await VisualFormBuildingLocationSqLiteDataStore.UpdateItemAsync(VisualForm, App.VisualEditTracking);
                            else
                            {

                                if (InvasiveVisualBuildingLocationPhotoItems.Count == 0)
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

                                }
                                else
                                {

                                    //response = await VisualFormBuildingLocationDataStore.UpdateItemAsync(VisualForm, App.VisualEditTrackingForInvasive);
                                    response = await VisualFormBuildingLocationSqLiteDataStore.UpdateItemAsync(VisualForm, App.VisualEditTrackingForInvasive.Where(x => x.ImageType == "TRUE").ToList());

                                    response = await VisualFormBuildingLocationSqLiteDataStore.UpdateItemAsync(VisualForm, App.VisualEditTrackingForInvasive.Where(x => x.ImageType == "CONCLUSIVE").ToList(), "CONCLUSIVE");
                                }

                            }

                        }
                    }
                    else
                    {
                        if (await VisualFormBuildingLocationDataStore.GetItemAsync(VisualForm.Id) == null)
                        {
                            List<string> list = VisualBuildingLocationPhotoItems.Select(c => c.ImageUrl).ToList();
                            response = await VisualFormBuildingLocationDataStore.AddItemAsync(VisualForm, list);
                            //  return await Task.FromResult(response);
                        }
                        else
                        {
                            List<MultiImage> finelList = new List<MultiImage>();
                            if (App.IsInvasive == false)
                                response = await VisualFormBuildingLocationDataStore.UpdateItemAsync(VisualForm, App.VisualEditTracking);
                            else
                            {

                                if (InvasiveVisualBuildingLocationPhotoItems.Count == 0)
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

                                }
                                else
                                {

                                    //response = await VisualFormBuildingLocationDataStore.UpdateItemAsync(VisualForm, App.VisualEditTrackingForInvasive);
                                    response = await VisualFormBuildingLocationDataStore.UpdateItemAsync(VisualForm, App.VisualEditTrackingForInvasive.Where(x => x.ImageType == "TRUE").ToList());

                                    response = await VisualFormBuildingLocationDataStore.UpdateItemAsync(VisualForm, App.VisualEditTrackingForInvasive.Where(x => x.ImageType == "CONCLUSIVE").ToList(), "CONCLUSIVE");
                                }

                            }

                        }
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

        #region conclusive

        public ObservableCollection<CustomRadioItem> RadioList_ConclusiveLifeExpectancyEEE { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_ConclusiveLifeExpectancyLBC { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_ConclusiveLifeExpectancyAWE { get; set; }

        public ObservableCollection<CustomRadioItem> RadioList_OwnerAgreedToRepair { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_RepairComplete { get; set; }


        private void PopulateConclusiveRadios()
        {
            RadioList_OwnerAgreedToRepair = new ObservableCollection<CustomRadioItem>();
            RadioList_OwnerAgreedToRepair.Add(new CustomRadioItem() { ID = 1, Name = "Yes", IsSelected = false, GroupName = "Repair" });
            RadioList_OwnerAgreedToRepair.Add(new CustomRadioItem() { ID = 2, Name = "No", IsSelected = false, GroupName = "Repair" });


            RadioList_RepairComplete = new ObservableCollection<CustomRadioItem>();
            RadioList_RepairComplete.Add(new CustomRadioItem() { ID = 1, Name = "Yes", IsSelected = false, GroupName = "RepairState" });
            RadioList_RepairComplete.Add(new CustomRadioItem() { ID = 2, Name = "No", IsSelected = false, GroupName = "RepairState" });


            RadioList_ConclusiveLifeExpectancyEEE = new ObservableCollection<CustomRadioItem>();
            RadioList_ConclusiveLifeExpectancyEEE.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsSelected = false, GroupName = "EEEC" });
            RadioList_ConclusiveLifeExpectancyEEE.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsSelected = false, GroupName = "EEEC" });
            RadioList_ConclusiveLifeExpectancyEEE.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsSelected = false, GroupName = "EEEC" });
            RadioList_ConclusiveLifeExpectancyEEE.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsSelected = false, GroupName = "EEEC" });


            RadioList_ConclusiveLifeExpectancyLBC = new ObservableCollection<CustomRadioItem>();
            RadioList_ConclusiveLifeExpectancyLBC.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsSelected = false, GroupName = "LBCC" });
            RadioList_ConclusiveLifeExpectancyLBC.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsSelected = false, GroupName = "LBCC" });
            RadioList_ConclusiveLifeExpectancyLBC.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsSelected = false, GroupName = "LBCC" });
            RadioList_ConclusiveLifeExpectancyLBC.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsSelected = false, GroupName = "LBCC" });


            RadioList_ConclusiveLifeExpectancyAWE = new ObservableCollection<CustomRadioItem>();
            RadioList_ConclusiveLifeExpectancyAWE.Add(new CustomRadioItem() { ID = 1, Name = "0-1 Years", IsSelected = false, GroupName = "AWEC" });
            RadioList_ConclusiveLifeExpectancyAWE.Add(new CustomRadioItem() { ID = 2, Name = "1-4 Years", IsSelected = false, GroupName = "AWEC" });
            RadioList_ConclusiveLifeExpectancyAWE.Add(new CustomRadioItem() { ID = 3, Name = "4-7 Years", IsSelected = false, GroupName = "AWEC" });
            RadioList_ConclusiveLifeExpectancyAWE.Add(new CustomRadioItem() { ID = 4, Name = "7+ Years", IsSelected = false, GroupName = "AWEC" });


        }

        private ObservableCollection<VisualBuildingLocationPhoto> _conclusiveVisualBuildingLocationPhotoItems;
        public ObservableCollection<VisualBuildingLocationPhoto> ConclusiveVisualBuildingLocationPhotoItems
        {
            get
            {
                return _conclusiveVisualBuildingLocationPhotoItems;
            }
            set
            {
                _conclusiveVisualBuildingLocationPhotoItems = value;
                OnPropertyChanged("ConclusiveVisualBuildingLocationPhotoItems");
            }
        }
        private string _conclusiveUnitPhotoCount;
        public string ConclusiveUnitPhotoCount
        {
            get { return _conclusiveUnitPhotoCount; }
            set { _conclusiveUnitPhotoCount = value; OnPropertyChanged("ConclusiveUnitPhotoCount"); }
        }
        #endregion

        private ObservableCollection<VisualBuildingLocationPhoto> _InvvisualProjectLocationPhotoItems;

        public ObservableCollection<VisualBuildingLocationPhoto> InvasiveVisualBuildingLocationPhotoItems
        {
            //get { return _InvvisualProjectLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(_InvvisualProjectLocationPhotoItems.Where(c => c.InvasiveImage == false)); }
            get { return _InvvisualProjectLocationPhotoItems; }
            set { _InvvisualProjectLocationPhotoItems = value; OnPropertyChanged("InvasiveVisualBuildingLocationPhotoItems"); }
        }
        public ObservableCollection<CustomRadioItem> RadioList_VisualReviewItems { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_AnyVisualSignItems { get; set; }

        public ObservableCollection<CustomRadioItem> RadioList_FurtherInasiveItems { get; set; }


        public ObservableCollection<CustomRadioItem> RadioList_ConditionAssessment { get; set; }

        public ObservableCollection<CustomRadioItem> RadioList_LifeExpectancyEEE { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_LifeExpectancyLBC { get; set; }
        public ObservableCollection<CustomRadioItem> RadioList_LifeExpectancyAWE { get; set; }

        // public VisualFormProjectLocation MyProperty { get; set; }
        private BuildingLocation_Visual visualForm;

        public BuildingLocation_Visual VisualForm
        {
            get { return visualForm; }
            set { visualForm = value; OnPropertyChanged("VisualForm"); }
        }

        public VisualBuildingLocationFormViewModel()
        {

            IsVisualApartment = false;
            IsVisualProjectLocatoion = false;
            IsVisualBuilding = true;
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

            PopulateConclusiveRadios();

            GoBackCommand = new Command(async () => await GoBack());
            SaveCommand = new Command(async () => await Save());
            SaveAndCreateNewCommand = new Command(async () => await SaveCreateNew());
            ExteriorElements = new ObservableCollection<string>();
            WaterProofingElements = new ObservableCollection<string>();

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

            App.ListCamera2Api = new List<MultiImage>();

        }

        public VisualBuildingLocationFormViewModel(VisualBuildingLocationFormViewModel viewModel)
        {
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

        private ObservableCollection<VisualBuildingLocationPhoto> _visualbuildingLocationPhotoItems;

        public ObservableCollection<VisualBuildingLocationPhoto> VisualBuildingLocationPhotoItems
        {
            get { return _visualbuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(_visualbuildingLocationPhotoItems.Where(c => c.InvasiveImage == false)); }
            //get { return _visualbuildingLocationPhotoItems; }
            set { _visualbuildingLocationPhotoItems = value; OnPropertyChanged("VisualBuildingLocationPhotoItems"); }
        }
        private string _unitPhotCount;

        public string UnitPhotoCount
        {
            get { return _unitPhotCount; }
            set { _unitPhotCount = value; OnPropertyChanged("UnitPhotoCount"); }
        }

        //  public ObservableCollection<VisualProjectLocationPhoto> UnitPhotos { get; set; }
        private ObservableCollection<VisualBuildingLocationPhoto> _unitPhoto;

        public ObservableCollection<VisualBuildingLocationPhoto> UnitPhotos
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
                    if (App.IsInvasive)
                    {
                        if (string.IsNullOrEmpty(visualForm.Id))
                        {
                            await Shell.Current.Navigation.PushAsync(new InvasiveUnitPhotoForm() { BindingContext = new UnitPhotoViewModel() { BuildingLocation_Visual = VisualForm, IsVisualBuilding = true, IsEdit = false } });
                        }
                        else
                        {
                            await Shell.Current.Navigation.PushAsync(new InvasiveUnitPhotoForm() { BindingContext = new UnitPhotoViewModel() { BuildingLocation_Visual = VisualForm, IsVisualBuilding = true, IsEdit = false } });
                        }
                    }
                    else

                        await Shell.Current.Navigation.PushAsync(new UnitPhtoForm() { BindingContext = new UnitPhotoViewModel() { BuildingLocation_Visual = VisualForm, IsVisualBuilding = true } });
                }

            }
        }
        public ICommand ViusalReviewDetailCommand => new Command<CustomRadioItem>(async (CustomRadioItem parm) => await ExecuteViusalReviewDetailCommand(parm));

        private Task ExecuteViusalReviewDetailCommand(CustomRadioItem parm)
        {
            throw new NotImplementedException();
        }
        private string _InvunitPhotCount;

        public string InvasiveUnitPhotoCount
        {
            get { return _InvunitPhotCount; }
            set { _InvunitPhotCount = value; OnPropertyChanged("InvasiveUnitPhotoCount"); }
        }
        private async Task<bool> Running()
        {
            if (App.ListCamera2Api != null)
            {

                foreach (var photo in App.ListCamera2Api)
                {
                    VisualBuildingLocationPhoto newObj = new VisualBuildingLocationPhoto() { ImageDescription = photo.ImageType, ImageUrl = photo.Image, Id = Guid.NewGuid().ToString(), VisualBuildingId = VisualForm.Id, DateCreated = DateTime.Now };
                    newObj.ImageDescription = photo.ImageType;
                    _ = AddNewPhoto(newObj).ConfigureAwait(false);

                }
                App.ListCamera2Api.Clear();
            }
            UnitPhotos = new ObservableCollection<VisualBuildingLocationPhoto>();
            if (VisualForm != null)
            {
                if (string.IsNullOrEmpty(visualForm.Id))
                {
                    if (App.IsAppOffline)
                    {
                        VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectIDSqLite(VisualForm.Id, true));
                    }
                    else
                        VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id, false));
                }
                else
                {
                    if (App.IsAppOffline)
                    {
                        VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectIDSqLite(VisualForm.Id, true));
                    }
                    else
                        VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id, false));

                    if (App.IsInvasive == true)
                    {
                        var photos = new ObservableCollection<VisualBuildingLocationPhoto>((await InvasiveVisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id, false)));
                        InvasiveUnitPhotoCount = InvasiveVisualBuildingLocationPhotoItems.Count.ToString();

                        InvasiveVisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(photos.Where(x => x.ImageDescription == "TRUE"));
                        InvasiveUnitPhotoCount = InvasiveVisualBuildingLocationPhotoItems.Count.ToString();

                        ConclusiveVisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(photos.Where(x => x.ImageDescription == "CONCLUSIVE"));
                        ConclusiveUnitPhotoCount = ConclusiveVisualBuildingLocationPhotoItems.Count.ToString();
                    }

                }

                UnitPhotoCount = VisualBuildingLocationPhotoItems.Count.ToString();

            }

            return await Task.FromResult(true);
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
        public ICommand ChooseConclusivePhotoFromCameraCommand => new Command(async () => await ChooseConclusvePhotoFromCameraCommandExecute());
        private async Task ChooseConclusvePhotoFromCameraCommandExecute()
        {
            await ChoosePhotoFromCameraCommandExecute("CONCLUSIVE");
        }
        private async Task ChoosePhotoFromCameraCommandExecute(string imgType = "TRUE")
        {
            string selectedOption = await App.Current.MainPage.DisplayActionSheet("Select Option", "Cancel", null,
               new string[] { "Take New Photo", "From Gallery" });

            switch (selectedOption)
            {
                case "Take New Photo":
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        await Shell.Current.Navigation.PushModalAsync(new Camera2Forms.CameraPage() { BindingContext = new CameraViewModel() { BuildingLocation_Visual = VisualForm, IsVisualBuilding = true, ImageType = imgType } });
                        //await Shell.Current.Navigation.PushModalAsync(new Camera2Forms.CameraPageForAndroid() { BindingContext = new CameraViewModel() { BuildingLocation_Visual = VisualForm, IsVisualBuilding = true } });
                    }
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        await Shell.Current.Navigation.PushModalAsync(new Camera2Forms.CameraPage() { BindingContext = new CameraViewModel() { BuildingLocation_Visual = VisualForm, IsVisualBuilding = true, ImageType = imgType } });
                    }

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
                                    VisualBuildingLocationPhoto obj = new VisualBuildingLocationPhoto() { ImageUrl = item, Id = Guid.NewGuid().ToString(), VisualBuildingId = VisualForm.Id, DateCreated = DateTime.Now, ImageDescription = imgType };
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
                                VisualBuildingLocationPhoto obj = new VisualBuildingLocationPhoto() { ImageUrl = item, Id = Guid.NewGuid().ToString(), VisualBuildingId = VisualForm.Id, DateCreated = DateTime.Now, ImageDescription = imgType };
                                _ = AddNewPhoto(obj);
                            }

                        });
                    }

                    break;
                default:
                    break;
            }

        }
        public async Task AddNewPhoto(VisualBuildingLocationPhoto obj)
        {
            if (App.IsInvasive == true)
            {
                IEnumerable<VisualBuildingLocationPhoto> photos;
                //updated for Conclusive
                if (App.IsAppOffline)
                {
                    await InvasiveVisualBuildingLocationPhotoDataStore.AddItemAsync(obj, true);
                    photos = await InvasiveVisualBuildingLocationPhotoDataStore.GetItemsAsyncByLoacationIDSqLite(VisualForm.Id, true);
                }
                else
                {
                    await InvasiveVisualBuildingLocationPhotoDataStore.AddItemAsync(obj);
                    photos = await InvasiveVisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id, false);
                }
                
                InvasiveVisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(photos.Where(x => x.ImageDescription == "TRUE"));
                InvasiveUnitPhotoCount = InvasiveVisualBuildingLocationPhotoItems.Count.ToString();

                ConclusiveVisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(photos.Where(x => x.ImageDescription == "CONCLUSIVE"));
                ConclusiveUnitPhotoCount = ConclusiveVisualBuildingLocationPhotoItems.Count.ToString();

            }
            else
            {
                
                if (App.IsAppOffline)
                {
                    
                    await VisualBuildingLocationPhotoDataStore.AddItemAsync(obj, true);
                    VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectIDSqLite(VisualForm.Id, true));
                }
                else
                {
                    
                    await VisualBuildingLocationPhotoDataStore.AddItemAsync(obj);
                    VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id, false));
                }
                   
                UnitPhotoCount = VisualBuildingLocationPhotoItems.Count.ToString();
            }
        }
        public ICommand DeleteImageCommandLocation => new Command<VisualBuildingLocationPhoto>(async (VisualBuildingLocationPhoto parm) => await DeleteImageCommandCommandExecute(parm));
        private async Task DeleteImageCommandCommandExecute(VisualBuildingLocationPhoto parm)
        {
            var result = await Shell.Current.DisplayAlert(
                  "Alert",
                  "Are you sure you want to remove?",
                  "Yes", "No");



            if (result)
            {

                VisualBuildingLocationPhoto obj = parm as VisualBuildingLocationPhoto;
                await InvasiveVisualBuildingLocationPhotoDataStore.DeleteItemAsync(obj);
                var photos = new ObservableCollection<VisualBuildingLocationPhoto>(await InvasiveVisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(VisualForm.Id, false));

                InvasiveVisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(photos.Where(x => x.ImageDescription == "TRUE"));
                InvasiveUnitPhotoCount = InvasiveVisualBuildingLocationPhotoItems.Count.ToString();

                ConclusiveVisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(photos.Where(x => x.ImageDescription == "CONCLUSIVE"));
                ConclusiveUnitPhotoCount = ConclusiveVisualBuildingLocationPhotoItems.Count.ToString();
            }


        }

        public ICommand ImageDetailCommand => new Command<VisualBuildingLocationPhoto>(async (VisualBuildingLocationPhoto parm) => await ImageDetailCommandCommandExecute(parm));
        private async Task ImageDetailCommandCommandExecute(VisualBuildingLocationPhoto parm)
        {
            ImgData.Path = parm.ImageUrl;
            ImgData.ParentID = parm.VisualBuildingId;
            ImgData.VisualBuildingLocationPhoto = parm;
            ImgData.IsEditVisual = true;

            ImgData.FormType = "VB";
            await CurrentWithoutDetail.EditImage(ImgData, GetImage);

        }
        private async void GetImage(ImageData ImgData)
        {


            await Load();
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