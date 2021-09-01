using Mobile.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Mobile.Code.Views._8_VisualReportForm
{
    public class InvasiveTabbed : TabbedPage
    {
        public InvasiveTabbed(VisualProjectLocationFormViewModel vm)
        {
           // NavigationPage navigationPage = new NavigationPage(new InvasiveVisualProjectLocationForm() { BindingContext = vm });
            //  navigationPage.IconImageSource = "schedule.png";
          //  navigationPage.Title = "ProjectLocation";

          //  Children.Add(new InvasiveVisualProjectLocationForm() { Title = "Invasive", BindingContext = vm });
           // Children.Add(new AdditionalInvasive() { Title = "Invasive2" });
           // Children.Add(navigationPage);
        }
    }
}