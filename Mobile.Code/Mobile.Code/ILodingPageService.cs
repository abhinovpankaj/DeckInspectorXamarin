using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Code
{
	public interface ILodingPageService
	{
		void InitLoadingPage
					  (Xamarin.Forms.ContentPage loadingIndicatorPage = null);

		void ShowLoadingPage();

		void HideLoadingPage();
	}
}
