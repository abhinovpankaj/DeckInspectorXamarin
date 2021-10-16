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
    public partial class ImageEditorPageWithoutDetail : ContentPage
    {
        List<IEnumerable<Point>> redostrokeslist;
        public ICommand SaveImageCommand { get; set; }
        public ImageEditorPageWithoutDetail(string SelectedImage)
        {
            InitializeComponent();


            //BindingContext = new ImageEditorViewModel(SelectedImage);
            //signaturepad.StrokeColor = editorcomment.TextColor = labelcomment.TextColor = Color.FromHex("#FF0000");
            //MessagingCenter.Subscribe<ColorPicker, string>(this, "SelectedColor",  (obj, item) =>
            //{
            //    string colorcode = item as string;
            //    signaturepad.StrokeColor = editorcomment.TextColor = labelcomment.TextColor = Color.FromHex(colorcode);

            //});
            MessagingCenter.Subscribe<CurrentWithoutDetail, string>(this, "Change", (obj, item) =>
           {
               Task.Run(() => this.Reset()).Wait();
               //  await Reset();

           });
            SaveImageCommand = new Command(async () => await ExecuteSaveItemsCommand());
        }
        private async Task<bool> Reset()
        {
            // labelcomment.Text = editorcomment.Text = string.Empty;
            return await Task.FromResult(true);
            // signaturepad.StrokeColor = Color.FromHex("#0000FF");
            //  await Shell.Current.Navigation.PushModalAsync(new ColorPicker());
        }
        private async void TapGestureShowColor_Tapped(object sender, EventArgs e)
        {
            //  signaturepad.StrokeColor = Color.FromHex("#0000FF");
            await Shell.Current.Navigation.PushModalAsync(new ColorPicker());
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

        private void imagebackground_ImageSaved(object sender, Syncfusion.SfImageEditor.XForms.ImageSavedEventArgs args)
        {
            //   CurrentWithoutDetail vm = (CurrentWithoutDetail)this.BindingContext;

        }

        void imagebackground_ImageSaving(System.Object sender, Syncfusion.SfImageEditor.XForms.ImageSavingEventArgs args)
        {

            CurrentWithoutDetail vm = (CurrentWithoutDetail)this.BindingContext;

            MemoryStream m = new MemoryStream();
            args.Stream.CopyTo(m);

            byte[] data = m.ToArray();
            vm.Save(data);
            // args.Cancel = true;
        }

        private void imagebackground_ImageLoaded(object sender, Syncfusion.SfImageEditor.XForms.ImageLoadedEventArgs args)
        {
            
        }
    }
}