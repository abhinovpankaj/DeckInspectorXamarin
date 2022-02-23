using Mobile.Code.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class CameraViewModel : BaseViewModel
    {
        private VisualProjectLocationPhoto _vpc;
        private ObservableCollection<MultiImage> _imageList;


        private string _CountPhoto;

        public string CountPhoto
        {
            get { return _CountPhoto; }
            set { _CountPhoto = value; OnPropertyChanged("CountPhoto"); }
        }

        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }

        public ObservableCollection<MultiImage> ImageList
        {

            get
            {

                return _imageList;


            }
            set { _imageList = value; OnPropertyChanged("ImageList"); }
        }


        public VisualProjectLocationPhoto VisualProjectLocationPhoto
        {
            get { return _vpc; }
            set { _vpc = value; OnPropertyChanged("VisualProjectLocationPhoto"); }
        }
        private VisualBuildingLocationPhoto _vbpc;

        public VisualBuildingLocationPhoto VisualBuildingLocationPhoto
        {
            get { return _vbpc; }
            set { _vbpc = value; OnPropertyChanged("VisualBuildingLocationPhoto"); }
        }
        private Apartment_Visual _abpc;

        public Apartment_Visual Apartment_Visual
        {
            get { return _abpc; }
            set { _abpc = value; OnPropertyChanged("Apartment_Visual"); }
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
        private bool _isApartment;
        public bool IsApartment
        {
            get { return _isApartment; }
            set { _isApartment = value; OnPropertyChanged("IsApartment"); }
        }
        private bool _isBuildingLocation;
        public bool IsBuildingLocation
        {
            get { return _isBuildingLocation; }
            set { _isBuildingLocation = value; OnPropertyChanged("IsBuildingLocation"); }
        }
        private bool _isA;
        public bool IsVisualApartment
        {
            get { return _isA; }
            set { _isA = value; OnPropertyChanged("IsVisualApartment"); }
        }
        private bool _ispl;
        public bool IsProjectLocation
        {
            get { return _ispl; }
            set { _ispl = value; OnPropertyChanged("IsProjectLocation"); }
        }
        public ICommand GoBackCommand => new Command(async () => await GoBack());

        private async Task GoBack()
        {
            await Shell.Current.Navigation.PopModalAsync();
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


        private ObservableCollection<VisualApartmentLocationPhoto> _visualApartmentLocationPhotoitems;

        public ObservableCollection<VisualApartmentLocationPhoto> VisualApartmentLocationPhotoItems
        {
            get { return _visualApartmentLocationPhotoitems; }
            set { _visualApartmentLocationPhotoitems = value; OnPropertyChanged("VisualApartmentLocationPhoto"); }
        }
        public async Task AddNewPhoto(VisualProjectLocationPhoto obj)
        {
            await VisualProjectLocationPhotoDataStore.AddItemAsync(obj);
            VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(ProjectLocation_Visual.Id, false));
            CountItem = VisualProjectLocationPhotoItems.Count.ToString();
        }
        public async Task AddNewPhoto(VisualBuildingLocationPhoto obj)
        {
            await VisualBuildingLocationPhotoDataStore.AddItemAsync(obj);
            VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(BuildingLocation_Visual.Id, false));
            CountItem = VisualBuildingLocationPhotoItems.Count.ToString();
        }
        public async Task AddNewPhoto(VisualApartmentLocationPhoto obj)
        {
            await VisualApartmentLocationPhotoDataStore.AddItemAsync(obj);
            VisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(Apartment_Visual.Id, false));
            CountItem = VisualApartmentLocationPhotoItems.Count.ToString();
        }
        public async Task AddNewPhoto(ProjectCommonLocationImages obj)
        {
            await ProjectCommonLocationImagesDataStore.AddItemAsync(obj);
            //     ProjectCommonLocationImagesDataStore = new ObservableCollection<ProjectCommonLocationImages>(await ProjectCommonLocationImagesDataStore.GetItemsAsyncByProjectLocationId(ProjectLocation.Id));
            //  CountItem = ProjectCommonLocationImagesDataStore.Count.ToString();
        }
        public async Task AddNewPhoto(BuildingCommonLocationImages obj)
        {
            await BuildingCommonLocationImagesDataStore.AddItemAsync(obj);
            //     ProjectCommonLocationImagesDataStore = new ObservableCollection<ProjectCommonLocationImages>(await ProjectCommonLocationImagesDataStore.GetItemsAsyncByProjectLocationId(ProjectLocation.Id));
            //  CountItem = ProjectCommonLocationImagesDataStore.Count.ToString();
        }
        public async Task AddNewPhoto(BuildingApartmentImages obj)
        {
            await BuildingApartmentImagesDataStore.AddItemAsync(obj);
            //     ProjectCommonLocationImagesDataStore = new ObservableCollection<ProjectCommonLocationImages>(await ProjectCommonLocationImagesDataStore.GetItemsAsyncByProjectLocationId(ProjectLocation.Id));
            //  CountItem = ProjectCommonLocationImagesDataStore.Count.ToString();
        }
        private BuildingLocation buildingLocation;

        public BuildingLocation BuildingLocation
        {
            get { return buildingLocation; }
            set { buildingLocation = value; OnPropertyChanged("BuildingLocation"); }
        }

        private BuildingApartment buildingApartment;

        public BuildingApartment BuildingApartment
        {
            get { return buildingApartment; }
            set { buildingApartment = value; OnPropertyChanged("BuildingApartment"); }
        }
        private string _count;

        public string CountItem
        {
            get { return _count; }
            set { _count = value; OnPropertyChanged("CountItem"); }
        }
        private ProjectLocation_Visual _plv;
        public ProjectLocation_Visual ProjectLocation_Visual
        {
            get { return _plv; }
            set { _plv = value; OnPropertyChanged("ProjectLocation_Visual"); }
        }
        private ProjectLocation _pl;
        public ProjectLocation ProjectLocation
        {
            get { return _pl; }
            set { _pl = value; OnPropertyChanged("ProjectLocation"); }
        }
        private BuildingLocation_Visual _blv;
        public BuildingLocation_Visual BuildingLocation_Visual
        {
            get { return _blv; }
            set { _blv = value; OnPropertyChanged("BuildingLocation_Visual"); }
        }
        public CameraViewModel()
        {
            IsBusyProgress = false;
            ImageList = new ObservableCollection<MultiImage>();
            CountPhoto = "0 Photo(s)";
        }
        public ICommand DeleteCommand => new Command<MultiImage>(async (MultiImage parm) => await Delete(parm));
        private async Task Delete(MultiImage parm)
        {
            ImageList.Remove(parm);
            App.ListCamera2Api.Remove(parm);
            CountPhoto = ImageList.Count + " Photo(s)";
            await Task.FromResult(true);
            

        }

        public string ImageType { get; set; }
    }
}
