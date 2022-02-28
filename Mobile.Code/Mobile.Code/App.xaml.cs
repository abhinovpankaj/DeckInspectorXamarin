using Mobile.Code.Models;
using Mobile.Code.Services;
using Mobile.Code.Services.SQLiteLocal;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mobile.Code
{

    public partial class App : Application
    {
        public static string AzureBackendUrl =
        //DeviceInfo.Platform == DevicePlatform.Android ? "http://192.168.43.248/" : "http://localhost:5000";
        DeviceInfo.Platform == DevicePlatform.Android ? "http://api.deckinspectors.com/v3/" : "http://api.deckinspectors.com/v3/";
        public static bool UseMockDataStore = true;
        public static readonly Guid UserID = new Guid("B339656A-C220-4ED5-88CF-A7EC500BD71A");
        public static int CompressionQuality { get; set; } = 100;
        public static User LogUser = null;
        public static ReportType ReportType { get; set; }
        public static List<MultiImage> ListCamera2Api { get; set; }
        public static List<MultiImage> VisualEditTracking { get; set; }
        public static List<MultiImage> VisualEditTrackingForInvasive { get; set; }
        public static bool IsInvasive { get; set; }
        public static bool IsVisualEdidingMode { get; set; }
        public static string projectBuildingID { get; set; }
        public static int Orientation { get; set; }
        public static string FormString { get; set; }
        public static string ImageFormString { get; set; }
        public static bool IsNewForm { get; set; }
        public static string InvaiveImages { get; set; }
        public static bool AutoLogin { get; set; }
        public static bool IsAppOffline { get; set; }
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDE4MTUzQDMxMzgyZTM0MmUzMG02MXh1Z0Y2TXRzK3pmKzJkeW5xcjdvVWxrbndLeVlpSFU1YUJFam5DcDg9");
            InitializeComponent();
            IsInvasive = false;
            IsAppOffline = false;
            VisualEditTracking = new List<MultiImage>();
            CompressionQuality = 100;
           
            DependencyService.Register<ProjectDataStore>();
            DependencyService.Register<ProjectBuildingDataStore>();
            DependencyService.Register<ProjectLocationDataStore>();
            DependencyService.Register<BuildingLocationDataStore>();
            DependencyService.Register<BuildingApartmentDataStore>();
            DependencyService.Register<ProjectCommonLocationImagesDataStore>();
            DependencyService.Register<BuildingCommonLocationImagesDataStore>();
            DependencyService.Register<BuildingApartmentImagesDataStore>();
            DependencyService.Register<VisualFormProjectLocationDataStore>();
            DependencyService.Register<VisualProjectLocationPhotoDataStore>();
            DependencyService.Register<LoginServices>();
            DependencyService.Register<InvasiveVisualProjectLocationPhotoDataStore>();
            DependencyService.Register<InvasiveVisualBuildingLocationPhotoDataStore>();
            DependencyService.Register<InvasiveVisualApartmentLocationPhotoDataStore>();
            DependencyService.Register<VisualFormBuildingLocationDataStore>();
            DependencyService.Register<VisualBuildingLocationPhotoDataStore>();
            DependencyService.Register<VisualFormApartmentDataStore>();
            DependencyService.Register<VisualApartmentLocationPhotoDataStore>();

            //SQLite Dependency

            DependencyService.Register<SqlLiteConnector>();
            DependencyService.Register<ProjectSqLiteDataStore>();
            DependencyService.Register<ProjectBuildingSqLiteDataStore>();
            DependencyService.Register<ProjectLocationSqLiteDataStore>();
            DependencyService.Register<BuildingLocationSqLiteDataStore>();
            DependencyService.Register<BuildingApartmentSqLiteDataStore>();
            //DependencyService.Register<ProjectCommonLocationImagesSqLiteDataStore>();
            DependencyService.Register<BuildingCommonLocationImagesSqLiteDataStore>();
            DependencyService.Register<BuildingApartmentImagesSqLiteDataStore>();
            DependencyService.Register<VisualFormProjectLocationSqLiteDataStore>();
           // DependencyService.Register<VisualProjectLocationPhotoSqLiteDataStore>();
            
            //DependencyService.Register<InvasiveVisualProjectLocationPhotoSqLiteDataStore>();
            //DependencyService.Register<InvasiveVisualBuildingLocationPhotoSqLiteDataStore>();
            //DependencyService.Register<InvasiveVisualApartmentLocationPhotoSqLiteDataStore>();
            DependencyService.Register<VisualFormBuildingLocationSqLiteDataStore>();
            //DependencyService.Register<VisualBuildingLocationPhotoSqLiteDataStore>();
            DependencyService.Register<VisualFormApartmentSqLiteDataStore>();
            //DependencyService.Register<VisualApartmentLocationPhotoSqLiteDataStore>();



            MainPage = new AppShell();
        }
        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
