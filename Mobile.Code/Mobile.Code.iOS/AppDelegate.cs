﻿using CarouselView.FormsPlugin.iOS;
using Foundation;
using UIKit;

namespace Mobile.Code.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //  global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.SetFlags(new string[] { "CollectionView_Experimental", "Expander_Experimental", "RadioButton_Experimental" });
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Forms.FormsMaterial.Init();
            
            // global::Xamarin.Forms.Forms.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            
            Syncfusion.SfImageEditor.XForms.iOS.SfImageEditorRenderer.Init();
            CarouselViewRenderer.Init();
            LoadApplication(new App());

            // Xamarin.Forms.DependencyService.Register<Xamarin.Forms.ImagePicker.IImagePickerService, Xamarin.Forms.ImagePicker.iOS.ImagePickerService>();
            return base.FinishedLaunching(app, options);
        }

    }
}
