using ImageEditor.ViewModels;
using Mobile.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageEditor.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageEditorPageForIOS : ContentPage
    {
        List<IEnumerable<Point>> redostrokeslist;
        private ISpeechToText _speechRecongnitionInstance;
        public ICommand SaveImageCommand { get; set; }
        public ImageEditorPageForIOS(string SelectedImage, string Name, string Description)
        {
            InitializeComponent();

            txtName.Text = Name;
            txtDes.Text = Description;
            if (Device.RuntimePlatform == Device.iOS)
            {
                recordDes.IsVisible = false;
                recordDes.IsEnabled = false;
            }
            //MessagingCenter.Subscribe<ColorPicker, string>(this, "SelectedColor",  (obj, item) =>
            //{
            //    string colorcode = item as string;
            //    signaturepad.StrokeColor= editorcomment.TextColor= labelcomment.TextColor = Color.FromHex(colorcode);

            //});
            ////BindingContext = new ImageEditorViewModel(SelectedImage);
            //signaturepad.StrokeColor = editorcomment.TextColor= labelcomment.TextColor= Color.FromHex("#FF0000");
            SaveImageCommand = new Command(async () => await ExecuteSaveItemsCommand());
            txtName.Completed += (s, e) => txtDes.Focus();
            _speechRecongnitionInstance = DependencyService.Get<ISpeechToText>();

            MessagingCenter.Subscribe<ISpeechToText, string>(this, "STT", (sender, args) =>
            {

                SpeechToTextFinalResultRecieved(args);
            });

            MessagingCenter.Subscribe<ISpeechToText>(this, "Final", (sender) =>
            {
                ImageButton btn = sender as ImageButton;
                if (btn.ClassId == "recordName")
                {
                    recordName.IsEnabled = true;

                }

                else if (btn.ClassId == "recordDes")
                {
                    recordDes.IsEnabled = true;

                }

            });

            MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });
        }
        private void SpeechToTextFinalResultRecieved(string args)
        {
            if (txtName.IsFocused)
            {
                txtName.Text += args;
            }

            if (txtDes.IsFocused)
            {
                txtDes.Text += args;
            }
        }

        private void recordClick(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            try
            {
                if (btn.ClassId == "recordName")
                {
                    txtName.Focus();

                }

                else if (btn.ClassId == "recordDes")
                {
                    txtDes.Focus();

                }
                _speechRecongnitionInstance.StartSpeechToText();
            }
            catch (Exception ex)
            {

                txtName.Text = ex.Message;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                recordName.IsEnabled = false;

                recordDes.IsEnabled = false;
            }

        }
        async Task ExecuteSaveItemsCommand()
        {
            IsBusy = true;

            try
            {
                MessagingCenter.Send(this, "AddItem", "Hello");
                await Navigation.PopModalAsync();
            }
            catch (Exception)
            {

            }
            finally
            {

            }
        }


        private async void TapGestureShowColor_Tapped(object sender, EventArgs e)
        {
            //  signaturepad.StrokeColor = Color.FromHex("#0000FF");
            await Shell.Current.Navigation.PushModalAsync(new ColorPicker());
        }

        private void imagebackground_ImageSaved(object sender, Syncfusion.SfImageEditor.XForms.ImageSavedEventArgs args)
        {

            // vm.SelectedImage = args.Location;
        }

        void imagebackground_ImageSaving(System.Object sender, Syncfusion.SfImageEditor.XForms.ImageSavingEventArgs args)
        {
            Current vm = (Current)this.BindingContext;
            MemoryStream m = new MemoryStream();
            args.Stream.CopyTo(m);

            vm.array = m.ToArray();
            //  vm.SaveImageCommand(data,this);
            args.Cancel = true;
        }

        private void txtName_Focused(object sender, FocusEventArgs e)
        {

        }

        void imagebackground_ImageLoaded(System.Object sender, Syncfusion.SfImageEditor.XForms.ImageLoadedEventArgs args)
        {
            //(sender as ImageE)
        }
    }
}