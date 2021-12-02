using Mobile.Code.Services;
using Mobile.Code.Services.SQLiteLocal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        public ILoginServices LogDataStore => DependencyService.Get<ILoginServices>();
        
        public IProjectDataStore ProjectDataStore => DependencyService.Get<IProjectDataStore>();
        public IProjectLocation ProjectLocationDataStore => DependencyService.Get<IProjectLocation>();
        public IProjectBuilding ProjectBuildingDataStore => DependencyService.Get<IProjectBuilding>();

        public IBuildingLocation BuildingLocationDataStore => DependencyService.Get<IBuildingLocation>();
        public IBuildingApartment BuildingApartmentDataStore => DependencyService.Get<IBuildingApartment>();

        public IProjectCommonLocationImages ProjectCommonLocationImagesDataStore => DependencyService.Get<IProjectCommonLocationImages>();
        public IBuildingCommonLocationImages BuildingCommonLocationImagesDataStore => DependencyService.Get<IBuildingCommonLocationImages>();

        public IBuildingApartmentImages BuildingApartmentImagesDataStore => DependencyService.Get<IBuildingApartmentImages>();

        public IVisualFormProjectLocationDataStore VisualFormProjectLocationDataStore => DependencyService.Get<IVisualFormProjectLocationDataStore>();
        public IVisualProjectLocationPhotoDataStore VisualProjectLocationPhotoDataStore => DependencyService.Get<IVisualProjectLocationPhotoDataStore>();
        public IInvasiveVisualProjectLocationPhotoDataStore InvasiveVisualProjectLocationPhotoDataStore => DependencyService.Get<InvasiveVisualProjectLocationPhotoDataStore>();
        public IInvasiveVisualBuildingLocationPhotoDataStore InvasiveVisualBuildingLocationPhotoDataStore => DependencyService.Get<InvasiveVisualBuildingLocationPhotoDataStore>();
        public IInvasiveVisualApartmentLocationPhotoDataStore InvasiveVisualApartmentLocationPhotoDataStore => DependencyService.Get<InvasiveVisualApartmentLocationPhotoDataStore>();

        public IVisualBuildingLocationPhotoDataStore VisualBuildingLocationPhotoDataStore => DependencyService.Get<VisualBuildingLocationPhotoDataStore>();
        public IVisualFormBuildingLocationDataStore VisualFormBuildingLocationDataStore => DependencyService.Get<IVisualFormBuildingLocationDataStore>();


        public IVisualFormApartmentDataStore VisualFormApartmentDataStore => DependencyService.Get<IVisualFormApartmentDataStore>();
        public IVisualApartmentLocationPhotoDataStore VisualApartmentLocationPhotoDataStore => DependencyService.Get<IVisualApartmentLocationPhotoDataStore>();

        //SQLite DataStores
        public IProjectDataStore ProjectSQLiteDataStore => DependencyService.Get<ProjectSqLiteDataStore>();
        public IProjectLocation ProjectLocationSqLiteDataStore => DependencyService.Get<ProjectLocationSqLiteDataStore>();
        public IProjectBuilding ProjectBuildingSqLiteDataStore => DependencyService.Get<ProjectBuildingSqLiteDataStore>();

        public IBuildingLocation BuildingLocationSqLiteDataStore => DependencyService.Get<BuildingLocationSqLiteDataStore>();
        public IBuildingApartment BuildingApartmentSqLiteDataStore => DependencyService.Get<BuildingApartmentSqLiteDataStore>();

        //public IProjectCommonLocationImages ProjectCommonLocationImagesSqLiteDataStore => DependencyService.Get<ProjectCommonLocationImagesSqLiteDataStore>();
       // public IBuildingCommonLocationImages BuildingCommonLocationImagesSqLiteDataStore => DependencyService.Get<BuildingCommonLocationImagesSqLiteDataStore>();

       // public IBuildingApartmentImages BuildingApartmentImagesSqLiteDataStore => DependencyService.Get<BuildingApartmentImagesSqLiteDataStore>();

        public IVisualFormProjectLocationDataStore VisualFormProjectLocationSqLiteDataStore => DependencyService.Get<VisualFormProjectLocationSqLiteDataStore>();
       
        public IVisualFormBuildingLocationDataStore VisualFormBuildingLocationSqLiteDataStore => DependencyService.Get<VisualFormBuildingLocationSqLiteDataStore>();


        public IVisualFormApartmentDataStore VisualFormApartmentSqLiteDataStore => DependencyService.Get<VisualFormApartmentSqLiteDataStore>();
     //   public IVisualApartmentLocationPhotoDataStore VisualApartmentLocationPhotoSqLiteDataStore => DependencyService.Get<VisualApartmentLocationPhotoSqLiteDataStore>();
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); OnPropertyChanged("IsBusy"); }
        }

        /* string title = string.Empty;
         public string Title
         {
             get { return title; }
             set { SetProperty(ref title, value); }
         }*/
        bool _inv = App.IsInvasive;
        public bool IsInvasiveVisible
        {
            get { return !_inv; }
            set { SetProperty(ref _inv, value); }
        }

        public string LogUserName
        {
            get { return App.LogUser.FullName; }

        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
