using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Mobile.Code.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
//            AppDomain.CurrentDomain.UnhandledException += (o, e) =>
//            {
//#if DEBUG
//                Debugger.Break();
//#endif
//            };

//            TaskScheduler.UnobservedTaskException += (o, e) =>
//            {
//#if DEBUG
//                Debugger.Break();
//#endif
//            };
        }
    }
}
