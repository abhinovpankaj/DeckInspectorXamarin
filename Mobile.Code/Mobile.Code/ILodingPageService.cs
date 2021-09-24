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
