using Mobile.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views._8_VisualReportForm
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InvasiveUnitPhotoForm : ContentPage
	{
		public InvasiveUnitPhotoForm ()
		{
			InitializeComponent ();
		}
		protected async override void OnAppearing()
		{
			UnitPhotoViewModel vm = ((UnitPhotoViewModel)this.BindingContext);
			await ((UnitPhotoViewModel)this.BindingContext).LoadAsync();


			base.OnAppearing();
			//vm.Load();
		}
	}

	
}