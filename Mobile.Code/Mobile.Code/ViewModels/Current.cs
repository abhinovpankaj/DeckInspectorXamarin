using ImageEditor.Helpers;
using ImageEditor.Pages;
using Mobile.Code;
using Mobile.Code.Controls;
using Mobile.Code.Models;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ImageEditor.ViewModels
{
    public class Current : INotifyPropertyChanged
    {
        public delegate void CallbackEventHandler(ImageData data);
        public event CallbackEventHandler Callback;
        public string SelectedImage { get; set; }
        public string CommentColor { get; set; }
        public string StrokeColor { get; set; }
        public double ColorSliderValue { get; set; }
        public double ScratchSliderValue { get; set; }
        public ICommand SaveImageCommand { get; set; }
        public ICommand ClosePageCommand { get; set; }

        private ImageData _imageData;

        public ImageData imageData
        {
            get { return _imageData; }
            set { _imageData = value; OnPropertyChanged("ImageSize"); }
        }

        // public ImageData imageData { get; set; }

        private string _imageSize;

        public string ImageSize
        {
            get { return _imageSize; }
            set { _imageSize = value; OnPropertyChanged("ImageSize"); }
        }

        public Current(ImageData _data)
        {
            imageData = new ImageData();
            imageData = _data;
            ImageSize = _data.Size;
            SelectedImage = _data.Path;
            CommentColor = StrokeColor = "Black";
            SaveImageCommand = new Command(SaveImageCommandExecute);
            ClosePageCommand = new Command(ClosePageCommandExecute);

            var fileLength = new FileInfo(SelectedImage).Length;
            ImageSize = GetFileSize(fileLength);
        }
        public static string GetFileSize(long length)
        {
            //string[] sizes = { "B", "KB", "MB", "GB" };
            //int order = 0;
            //while (fileLength >= 1024 && order + 1 < sizes.Length)
            //{
            //    order++;
            //    fileLength = fileLength / 1024;
            //}
            //string result = String.Format("{0:0.##} {1}", fileLength, sizes[order]);
            //return result;
            long B = 0, KB = 1024, MB = KB * 1024, GB = MB * 1024, TB = GB * 1024;
            double size = length;
            string suffix = nameof(B);

            if (length >= TB)
            {
                size = Math.Round((double)length / TB, 2);
                suffix = nameof(TB);
            }
            else if (length >= GB)
            {
                size = Math.Round((double)length / GB, 2);
                suffix = nameof(GB);
            }
            else if (length >= MB)
            {
                size = Math.Round((double)length / MB, 2);
                suffix = nameof(MB);
            }
            else if (length >= KB)
            {
                size = Math.Round((double)length / KB, 2);
                suffix = nameof(KB);
            }

            return $"{size} {suffix}";
        }
        private async void ClosePageCommandExecute(object obj)
        {
            var selectedOption = await App.Current.MainPage.DisplayAlert("Discard Photo",
                 "if you discard now,you'll lose your photos and edits.", "Discard", "Keep");
            if (selectedOption)
            {
                await Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        App.Current.MainPage.Navigation.PopAsync();
                    });
                });
            }
            //  await App.Current.MainPage.Navigation.PopAsync();
        }

        private void OnScratchSliderValueChanged()
        {
            var colorvalue = Convert.ToInt32(ScratchSliderValue);
            var selectedcolor = SliderColorsList.SliderColors.FirstOrDefault(x => x.ID == colorvalue);
            StrokeColor = selectedcolor.ColorOnHEX;
        }
        private void OnColorSliderValueChanged()
        {
            var colorvalue = Convert.ToInt32(ColorSliderValue);
            var selectedcolor = SliderColorsList.SliderColors.FirstOrDefault(x => x.ID == colorvalue);
            CommentColor = selectedcolor.ColorOnHEX;
        }
        public byte[] array = null;
        public async void SaveImageCommandExecute(object obj)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                var editorPage = obj as ImageEditorPage;
                var txtName = editorPage.Content.FindByName("txtName") as BorderlessEntry;

                var detailGrid = editorPage.Content.FindByName("detailGrid") as Grid;
                detailGrid.IsVisible = false;


                var txtDescription = editorPage.Content.FindByName("txtDes") as XEditor;
                //commentslider.IsVisible = false;

                if (array != null)
                {
                    string filepath = await DependencyService.Get<ISaveFile>().SaveFiles(Guid.NewGuid().ToString(), array);
                    imageData.Path = SelectedImage = filepath;
                }
                else
                {
                    imageData.Path = SelectedImage;
                }


                imageData.Name = txtName.Text;
                imageData.Description = txtDescription.Text;

                Callback?.Invoke(imageData);
                await Shell.Current.Navigation.PopAsync();
            }
            if (Device.RuntimePlatform == Device.iOS)
            {
                var editorPage = obj as ImageEditorPageForIOS;
                var txtName = editorPage.Content.FindByName("txtName") as BorderlessEntry;

                var detailGrid = editorPage.Content.FindByName("detailGrid") as Grid;
                detailGrid.IsVisible = false;


                var txtDescription = editorPage.Content.FindByName("txtDes") as XEditor;
                //commentslider.IsVisible = false;

                if (array != null)
                {
                    string filepath = await DependencyService.Get<ISaveFile>().SaveFiles(Guid.NewGuid().ToString(), array);
                    imageData.Path = filepath;
                }
                else
                {
                    imageData.Path = SelectedImage;
                }


                imageData.Name = txtName.Text;
                imageData.Description = txtDescription.Text;

                Callback?.Invoke(imageData);
                await Shell.Current.Navigation.PopAsync();
            }

        }

        private async void SaveImageCommandExecute1(object obj)
        {
            var editorPage = obj as ImageEditorPageForIOS;
            var txtName = editorPage.Content.FindByName("txtName") as BorderlessEntry;

            var detailGrid = editorPage.Content.FindByName("detailGrid") as Grid;
            detailGrid.IsVisible = false;


            var txtDescription = editorPage.Content.FindByName("txtDes") as XEditor;


            imageData.Name = txtName.Text;
            imageData.Description = txtDescription.Text;

            Callback?.Invoke(imageData);
            await Shell.Current.Navigation.PopAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImagePath">Send ImagePath that need to Edit</param>
        /// <param name="callbackEventHandler">return imagepath after click save button</param>
        /// <returns>return ImagePath on callback(imagepath)</returns>
        public static async Task EditImage(ImageData data, CallbackEventHandler callbackEventHandler)
        {


            // string filepath = await DependencyService.Get<ISaveFile>().SaveFiles(data.Path, null);
            ////  data.Path = filepath;
            var imgviewmodel = new Current(data);
            imgviewmodel.Callback += callbackEventHandler;
            if (Device.RuntimePlatform == Device.Android)
            {
                var imgpage = new ImageEditorPage(string.Empty, data.Name, data.Description);
                imgpage.BindingContext = imgviewmodel;
                await App.Current.MainPage.Navigation.PushAsync(imgpage, true);
            }
            if (Device.RuntimePlatform == Device.iOS)
            {
                var imgpage = new ImageEditorPageForIOS(string.Empty, data.Name, data.Description);
                imgpage.BindingContext = imgviewmodel;
                await App.Current.MainPage.Navigation.PushAsync(imgpage, true);
            }
            //    var imgpage = new ImageEditorPage(string.Empty,data.Name,data.Description);
            //imgpage.BindingContext = imgviewmodel;
            //await App.Current.MainPage.Navigation.PushAsync(imgpage, true);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

