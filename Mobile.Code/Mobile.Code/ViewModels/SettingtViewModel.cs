using Mobile.Code.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class SettingtViewModel : BaseViewModel
    {
        public Command ProjectDetailCommand { get; set; }
        public Command SelectCameraOption { get; set; }

        public ObservableCollection<CameraSetting> CameraSettingItems { get; set; }
        public Command GoBackCommand { get; set; }
        public SettingtViewModel()
        {
            CameraSettingItems = new ObservableCollection<CameraSetting>();
            CameraSettingItems.Add(new CameraSetting() { Id = 1, Name = "High", compression = 100, IsSelected = false });
            CameraSettingItems.Add(new CameraSetting() { Id = 2, Name = "Medium", compression = 50, IsSelected = false });
            CameraSettingItems.Add(new CameraSetting() { Id = 3, Name = "Low", compression = 20, IsSelected = false });
            SelectCameraOption = new Command<CameraSetting>(async (CameraSetting parm) => await ExecuteSelectCameraOption(parm));
            GoBackCommand = new Command(async () => await GoBack());
        }
        private async Task GoBack()
        {
            await Shell.Current.Navigation.PopAsync();



        }
        private async Task ExecuteSelectCameraOption(CameraSetting parm)
        {

            foreach (var item in CameraSettingItems)
            {
                item.IsSelected = false;

            }
            parm.IsSelected = true;
            await SecureStorage.SetAsync("CompressionQuality", parm.compression.ToString());

            App.CompressionQuality = parm.compression;
            // return await Task.FromResult(true);
        }
        public async Task LoadData()
        {
            string CompressionQuality = await SecureStorage.GetAsync("CompressionQuality");
            if (string.IsNullOrEmpty(CompressionQuality))
            {
                CompressionQuality = "100";
            }
            CameraSetting cs = CameraSettingItems.Where(c => c.compression == Convert.ToInt32(CompressionQuality)).SingleOrDefault();
            cs.IsSelected = true;
        }

        //public class ProjectViewModel : BaseViewModel
        //{
        //    public Command ProjectDetailCommand { get; set; }
        //    public ProjectViewModel()
        //    {
        //        ProjectDetailCommand = new Command(async () => await ExecuteProjectDetailCommand());

        //    }
        //    public void Load()
        //    {

        //    }
        //    async Task ExecuteProjectDetailCommand()
        //    {
        //      await  Application.Current.MainPage.DisplayAlert("Selected Peron", "Person id : ", "Ok","cancel");
        //        // await Shell.Current.GoToAsync("projectdetail");
        //    }
        //}
    }
}
