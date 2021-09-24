using Mobile.Code.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _uname;

        public string Username
        {
            get { return _uname; }
            set { _uname = value; OnPropertyChanged("Username"); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); }
        }
        public ICommand LoginCommand => new Command(async () => await Login());
        private async Task Login()
        {
            if (await CheakAppPermission() == false)
            {
                await Shell.Current.DisplayAlert("Permission", "Camera and storage permission required", "OK");
                return;
            }
            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(Username))
            {
                errorMessage += "\nUsername is required\n";
            }
            if (string.IsNullOrEmpty(Password))
            {
                errorMessage += "\nPassword required\n";
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                await Shell.Current.DisplayAlert("Validation Error", errorMessage, "OK");
                return;

            }
            IsBusyProgress = true;
            var response = await Task.Run(() =>
               Running()
            );
            if (response.Status == ApiResult.Success)
            {
                Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
                Shell.Current.BindingContext = new AppShell() { LogUserName = App.LogUser.FullName };
                IsBusyProgress = false;
                await Shell.Current.GoToAsync("//main");

                //  await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                IsBusyProgress = false;
                await Shell.Current.DisplayAlert("Message", response.Message, "OK");
            }

            //await Shell.Current.GoToAsync("//main");
        }
        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }
        public async Task<Response> Running()
        {

            Response result = new Response();


            try
            {
                User user = new User();
                user.UserName = Username;
                user.Pwd = Password;
                Response response = await LogDataStore.ValidateLogin(user);
                if (response.Status == ApiResult.Success)
                {
                    user = JsonConvert.DeserializeObject<User>(response.Data.ToString());
                    if (user.ErrNo == 1)
                    {
                        if (user.RoleName == "Mobile" || user.RoleName == "Admin" || user.RoleName == "Desktop,Mobile")
                        {
                            App.LogUser = user;
                            if (Savecredentials == true)
                            {
                                await SecureStorage.SetAsync("Username", Username);
                                await SecureStorage.SetAsync("Password", Password);
                                await SecureStorage.SetAsync("Savecredential", "True");
                            }
                            else
                            {
                                await SecureStorage.SetAsync("Username", string.Empty);
                                await SecureStorage.SetAsync("Password", string.Empty);
                                await SecureStorage.SetAsync("Savecredential", "False");
                            }
                            result.Status = ApiResult.Success; ;

                        }
                        else
                        {

                            result.Message = "you are not authorized to access this application";
                            result.Status = ApiResult.Fail; ;
                        }
                    }
                    else
                    {
                        result.Message = "you are not authorized to access this application";
                        result.Status = ApiResult.Fail; ;
                    }

                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Status = ApiResult.Fail; ;

            }
            return await Task.FromResult(result);
        }
        private bool _Savecredentials;

        public bool Savecredentials
        {
            get { return _Savecredentials; }
            set { _Savecredentials = value; OnPropertyChanged("Savecredentials"); }
        }


        public ICommand SavecredentialCommand => new Command(async () => await SavecredentialCommandExecute());
        private async Task SavecredentialCommandExecute()
        {
            try
            {

                Savecredentials = !Savecredentials;
                if (Savecredentials == true)
                {
                    await SecureStorage.SetAsync("Savecredential", "True");
                }
                else
                {
                    await SecureStorage.SetAsync("Savecredential", "False");
                    SecureStorage.Remove("Username");
                    SecureStorage.Remove("Password");
                }


            }
            catch (Exception)
            {
                // Possible that device doesn't support secure storage on device.
            }

            await Shell.Current.GoToAsync("//main");
        }
        public LoginViewModel()
        {




        }
        public async Task<bool> CheakAppPermission()
        {
            bool statusRequirment = false;
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status == PermissionStatus.Denied)
            {
                var Camerastatus = await Permissions.RequestAsync<Permissions.Camera>();
                if (Camerastatus == PermissionStatus.Denied)
                {
                    // await Shell.Current.DisplayAlert("Permission", "Camera Permission required", "OK");
                    return false;
                }
            }


            PermissionStatus storageRead = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (storageRead == PermissionStatus.Denied)
            {
                var storageReadStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
                if (storageReadStatus == PermissionStatus.Denied)
                {
                    //await Shell.Current.DisplayAlert("Permission", "Storage Read Permission required", "OK");
                    // return;
                    return false;
                }
            }

            PermissionStatus storagewrite = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (storagewrite == PermissionStatus.Denied)
            {
                var storageWriteStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
                if (storageWriteStatus == PermissionStatus.Denied)
                {
                    return false;
                    //  await Shell.Current.DisplayAlert("Permission", "Storage Write Permission required", "OK");
                    //return;
                }
            }

            statusRequirment = true;
            return statusRequirment;

        }
        public async Task Load()
        {


            var Savecredential = await SecureStorage.GetAsync("Savecredential");
            if (Savecredential == "True")
            {
                this.Savecredentials = true;
                var Username = await SecureStorage.GetAsync("Username");
                var Password = await SecureStorage.GetAsync("Password");
                this.Username = Username;
                this.Password = Password;

            }
            else
            {
                await SecureStorage.SetAsync("Username", string.Empty);
                await SecureStorage.SetAsync("Password", string.Empty);
                this.Savecredentials = false;
            }

        }
    }
}
