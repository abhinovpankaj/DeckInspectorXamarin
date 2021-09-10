using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mobile.Code.Services;
using Mobile.Code.Views;
using Xamarin.Essentials;
using Mobile.Code.Models;
using System.Collections.Generic;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Mobile.Code
{
    
    public partial class App : Application
    {
        public static string AzureBackendUrl =
        //DeviceInfo.Platform == DevicePlatform.Android ? "http://192.168.43.248/" : "http://localhost:5000";
        DeviceInfo.Platform == DevicePlatform.Android ? "http://xoricwebapi-prod.us-east-1.elasticbeanstalk.com/" : "http://xoricwebapi-prod.us-east-1.elasticbeanstalk.com/";
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
        public static string  projectBuildingID { get; set; }
        public static int Orientation { get; set; }
        public static string FormString { get; set; }
        public static string ImageFormString { get; set; }
        public static bool IsNewForm { get; set; }
        public static string InvaiveImages { get; set; }
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDE4MTUzQDMxMzgyZTM0MmUzMG02MXh1Z0Y2TXRzK3pmKzJkeW5xcjdvVWxrbndLeVlpSFU1YUJFam5DcDg9");
            InitializeComponent();
            IsInvasive = false;
            VisualEditTracking = new List<MultiImage>();
            CompressionQuality = 100;
            //if (UseMockDataStore)
            //    //DependencyService.Register<MockDataStore>();
            //else
            //    DependencyService.Register<AzureDataStore>();
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
            MainPage = new AppShell();
        }
        protected override void OnStart()
        {
            //AppCenter.Start("ios=5da4b8f5-4212-42ca-81df-3e2eb6d39000;" ,
                    
                    
            //         typeof(Analytics), typeof(Crashes));
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
