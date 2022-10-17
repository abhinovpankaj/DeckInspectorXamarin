using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Database;

using Android.Hardware;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Wibci.Xamarin.Images;
using Wibci.Xamarin.Images.Droid;
using Xamarin.Forms;
using Environment = System.Environment;

namespace Mobile.Code.Droid
{
    [Activity(Label = "DECK INSPECTORS", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IMessageSender, ISensorEventListener
    {
        public const int CameraPermissionsCode = 1;
        //int RequestedOrientation;
        public static readonly string[] CameraPermissions =
        {
            Manifest.Permission.Camera
        };
        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
           
        }
        public static event EventHandler CameraPermissionGranted;

        public static int OPENCAMERACODE = 102;
        private readonly int VOICE = 10;
        internal static MainActivity Instance { get; private set; }
        //public static  int OPENGALLERYCODE = 1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            MessagingCenter.Subscribe<Camera2Forms.CameraPage>(this, "AllowLandscape", sender =>
            {
                RequestedOrientation = (int)ScreenOrientation.Landscape;
            });
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            mSensorManager = (SensorManager)GetSystemService(SensorService);
            var sensor = mSensorManager.GetDefaultSensor(Android.Hardware.SensorType.Gravity);
            mSensorManager.RegisterListener(this, sensor, Android.Hardware.SensorDelay.Normal);
            //mSensorManager.RegisterListener
            base.OnCreate(savedInstanceState);
            
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            DependencyService.Register<IResizeImageCommand, AndroidResizeImageCommand>();
            Xamarin.Forms.Forms.SetFlags(new string[] { "CollectionView_Experimental", "RadioButton_Experimental" });
            
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            
           
            LoadApplication(new App());
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                const string errorFileName = "Fatal.log";
                var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                var errorFilePath = Path.Combine(libraryPath, errorFileName);
                var errorMessage = String.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
                DateTime.Now, exception.ToString());
                File.WriteAllText(errorFilePath, errorMessage);

                // Log to Android Device Logging.
                Android.Util.Log.Error("Crash Report", errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        /// <summary>
        // If there is an unhandled exception, the exception information is diplayed 
        // on screen the next time the app is started (only in debug configuration)
        /// </summary>
        [Conditional("DEBUG")]
        private void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var errorFilePath = Path.Combine(libraryPath, errorFilename);

            if (!File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = File.ReadAllText(errorFilePath);
            new AlertDialog.Builder(this)
                .SetPositiveButton("Clear", (sender, args) =>
                {
                    File.Delete(errorFilePath);
                })
                .SetNegativeButton("Close", (sender, args) =>
                {
                    // User pressed Close.
                })
                .SetMessage(errorText)
                .SetTitle("Crash Report")
                .Show();
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public static int OPENGALLERYCODE = 100;
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == VOICE)
            {
                if (resultCode == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        string textInput = matches[0];
                        MessagingCenter.Send<IMessageSender, string>(this, "STT", textInput);
                    }
                    else
                    {
                        MessagingCenter.Send<IMessageSender, string>(this, "STT", "No input");
                    }

                }
            }
            base.OnActivityResult(requestCode, resultCode, data);

            //If we are calling multiple image selection, enter into here and return photos and their filepaths.
            if (requestCode == OPENGALLERYCODE && resultCode == Result.Ok)
            {
                List<string> images = new List<string>();
                ExifInterface exif = null;
                if (data != null)
                {
                    //Separate all photos and get the path from them all individually.
                    ClipData clipData = data.ClipData;
                    if (clipData != null)
                    {
                        for (int i = 0; i < clipData.ItemCount; i++)
                        {
                            ClipData.Item item = clipData.GetItemAt(i);
                            Android.Net.Uri uri = item.Uri;
                            var path = GetRealPathFromURI(uri);


                            if (path != null)
                            {
                                //exif = new ExifInterface(path);
                                //string orientation = exif.GetAttribute(ExifInterface.TagOrientation);
                                //exif.SetAttribute(ExifInterface.TagOrientation,"0");
                                //exif.SaveAttributes();
                                
                                images.Add(path);
                            }
                        }
                    }
                    else
                    {
                        Android.Net.Uri uri = data.Data;
                        string path = GetRealPathFromURI(uri);

                        if (path != null)
                        {
                            //exif = new ExifInterface(path);
                            //string orientation = exif.GetAttribute(ExifInterface.TagOrientation);
                            //exif.SetAttribute(ExifInterface.TagOrientation, "0");
                            //exif.SaveAttributes();
                            images.Add(path);
                        }
                    }

                    //Send our images to the carousel view.
                    MessagingCenter.Send<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid", images);
                }
            }
        }


        
        /// <summary>
        ///     Get the real path for the current image passed.
        /// </summary>
        public String GetRealPathFromURI(Android.Net.Uri contentURI)
        {
            try
            {
                ICursor imageCursor = null;
                string fullPathToImage = "";

                imageCursor = ContentResolver.Query(contentURI, null, null, null, null);
                imageCursor.MoveToFirst();
                int idx = imageCursor.GetColumnIndex(MediaStore.Images.ImageColumns.Data);

                if (idx != -1)
                {
                    fullPathToImage = imageCursor.GetString(idx);
                }
                else
                {
                    ICursor cursor = null;
                    var docID = DocumentsContract.GetDocumentId(contentURI);
                    var id = docID.Split(':')[1];
                    var whereSelect = MediaStore.Images.ImageColumns.Id + "=?";
                    var projections = new string[] { MediaStore.Images.ImageColumns.Data };

                    cursor = ContentResolver.Query(MediaStore.Images.Media.InternalContentUri, projections, whereSelect, new string[] { id }, null);
                    if (cursor.Count == 0)
                    {
                        cursor = ContentResolver.Query(MediaStore.Images.Media.ExternalContentUri, projections, whereSelect, new string[] { id }, null);
                    }
                    var colData = cursor.GetColumnIndexOrThrow(MediaStore.Images.ImageColumns.Data);
                    cursor.MoveToFirst();
                    fullPathToImage = cursor.GetString(colData);
                }
                return fullPathToImage;
            }
            catch (Exception ex)
            {
                Toast.MakeText(Xamarin.Forms.Forms.Context, "Unable to get path", ToastLength.Long).Show();
            }
            return null;
        }
        private SensorManager mSensorManager;
        private Sensor mSensorGr;

        private float[] mAccelerometerReading = new float[3];

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }
        public static int AppOrientation = 0;
        public void OnSensorChanged(SensorEvent sensorEvent)
        {
            if (sensorEvent.Sensor.Type == SensorType.Gravity)
            {
                sensorEvent.Values.CopyTo(mAccelerometerReading, 0);


                if (Math.Abs(mAccelerometerReading[0]) > Math.Abs(mAccelerometerReading[1]))
                {
                    if ((mAccelerometerReading[0] > 0))
                    {
                        AppOrientation = 2;
                    }
                    else
                    {
                        AppOrientation = 3;
                    }
                    
                }
                else
                {
                    if (mAccelerometerReading[1] > 0)
                    {
                        AppOrientation = 1;
                    }
                    else
                    {
                        AppOrientation = 4;
                    }
                   
                }
            }
        }
        
    }
}