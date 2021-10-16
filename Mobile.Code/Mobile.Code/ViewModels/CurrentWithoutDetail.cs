using ImageEditor.Helpers;
using ImageEditor.Pages;
using Mobile.Code;
using Mobile.Code.Models;
using Mobile.Code.ViewModels;
using Plugin.Screenshot;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ImageEditor.ViewModels
{
    public class CurrentWithoutDetail : BaseViewModel, INotifyPropertyChanged
    {
        public delegate void CallbackEventHandler(ImageData data);
        public event CallbackEventHandler Callback;

        private string _selectedImage;

        public string SelectedImage
        {
            get { return _selectedImage; }
            set { _selectedImage = value; OnPropertyChanged("SelectedImage"); }
        }
        private ObservableCollection<VisualProjectLocationPhoto> _visualProjectLocationPhotoItems;

        public ObservableCollection<VisualProjectLocationPhoto> VisualProjectLocationPhotoItems
        {
            get { return _visualProjectLocationPhotoItems; }
            set { _visualProjectLocationPhotoItems = value; OnPropertyChanged("VisualProjectLocationPhotoItems"); }
        }

        private ObservableCollection<VisualBuildingLocationPhoto> _visualBuildingLocationPhotoItems;

        public ObservableCollection<VisualBuildingLocationPhoto> VisualBuildingLocationPhotoItems
        {
            get { return _visualBuildingLocationPhotoItems; }
            set { _visualBuildingLocationPhotoItems = value; OnPropertyChanged("VisualProjectLocationPhotoItems"); }
        }


        private ObservableCollection<VisualApartmentLocationPhoto> _visualApartmentLocationPhoto;

        public ObservableCollection<VisualApartmentLocationPhoto> VisualApartmentLocationPhotoItems
        {
            get { return _visualApartmentLocationPhoto; }
            set { _visualApartmentLocationPhoto = value; OnPropertyChanged("VisualBuildingLocationPhotoItems"); }
        }

        private ObservableCollection<ProjectCommonLocationImages> _projectCommonLocationImages;

        public ObservableCollection<ProjectCommonLocationImages> ProjectCommonLocationImagesItems
        {
            get { return _projectCommonLocationImages; }
            set { _projectCommonLocationImages = value; OnPropertyChanged("ProjectCommonLocationImagesItems"); }
        }


        private ObservableCollection<BuildingCommonLocationImages> _buildingCommonLocationImages;

        public ObservableCollection<BuildingCommonLocationImages> BuildingCommonLocationImagesItems
        {
            get { return _buildingCommonLocationImages; }
            set { _buildingCommonLocationImages = value; OnPropertyChanged("BuildingCommonLocationImagesItems"); }
        }
        private ObservableCollection<BuildingApartmentImages> _buildingApartmentImages;

        public ObservableCollection<BuildingApartmentImages> BuildingApartmentImagesItems
        {
            get { return _buildingApartmentImages; }
            set { _buildingApartmentImages = value; OnPropertyChanged("BuildingApartmentImagesItems"); }
        }
        // public string SelectedImage { get; set; }
        public string CommentColor { get; set; }
        public string StrokeColor { get; set; }
        public double ColorSliderValue { get; set; }
        public double ScratchSliderValue { get; set; }
        public ICommand SaveImageCommand { get; set; }
        public ICommand ClosePageCommand { get; set; }
        public ImageData imageData { get; set; }

        private string _imageSize;

        public string ImageSize
        {
            get { return _imageSize; }
            set { _imageSize = value; OnPropertyChanged("ImageSize"); }
        }

        public CurrentWithoutDetail(ImageData _data)
        {

            imageData = new ImageData();
            imageData = _data;
            ImageSize = _data.Size;

            // i =_data.VisualProjectLocationPhotos.IndexOf(_data.VisualProjectLocationPhotos.Where(c => c.Id == _data.VisualProjectLocationPhoto.Id).Single());
            //  imageData.VisualProjectLocationPhotos = _data.VisualProjectLocationPhotos;

            CommentColor = StrokeColor = "Black";
            SaveImageCommand = new Command(SaveImageCommandExecute);
            ClosePageCommand = new Command(ClosePageCommandExecute);
            //var fileLength = new FileInfo(SelectedImage).Length;
            //  ImageSize = GetFileSize(fileLength);
            Task.Run(() => this.Load()).Wait();

        }
        public async Task Load()
        {
            if (imageData.FormType == "VP")
            {
                if (App.IsInvasive == false)
                    VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualProjectLocationPhoto.VisualLocationId, false));
                else
                    VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await InvasiveVisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualProjectLocationPhoto.VisualLocationId, false));

                i = VisualProjectLocationPhotoItems.IndexOf(VisualProjectLocationPhotoItems.Where(c => c.Id == imageData.VisualProjectLocationPhoto.Id).Single());
                //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                SelectedImage = VisualProjectLocationPhotoItems[i].ImageUrl;
            }
            else if (imageData.FormType == "VB")
            {
                if (App.IsInvasive == false)
                    VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualBuildingLocationPhoto.VisualBuildingId, false));
                else
                    VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await InvasiveVisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualBuildingLocationPhoto.VisualBuildingId, false));


                i = VisualBuildingLocationPhotoItems.IndexOf(VisualBuildingLocationPhotoItems.Where(c => c.Id == imageData.VisualBuildingLocationPhoto.Id).Single());
                //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                SelectedImage = VisualBuildingLocationPhotoItems[i].ImageUrl;
            }
            else if (imageData.FormType == "VA")
            {
                if (App.IsInvasive == false)
                    VisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualApartmentLocationPhoto.VisualApartmentId, false));
                else
                    VisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(await InvasiveVisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualApartmentLocationPhoto.VisualApartmentId, false));
                i = VisualApartmentLocationPhotoItems.IndexOf(VisualApartmentLocationPhotoItems.Where(c => c.Id == imageData.VisualApartmentLocationPhoto.Id).Single());
                //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                SelectedImage = VisualApartmentLocationPhotoItems[i].ImageUrl;
            }
            else if (imageData.FormType == "P")
            {
                ProjectCommonLocationImagesItems = new ObservableCollection<ProjectCommonLocationImages>(await ProjectCommonLocationImagesDataStore.GetItemsAsyncByProjectLocationId(imageData.ProjectCommonLocationImages.ProjectLocationId));
                i = ProjectCommonLocationImagesItems.IndexOf(ProjectCommonLocationImagesItems.Where(c => c.Id == imageData.ProjectCommonLocationImages.Id).Single());
                //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                SelectedImage = ProjectCommonLocationImagesItems[i].ImageUrl;
            }
            else if (imageData.FormType == "B")
            {
                BuildingCommonLocationImagesItems = new ObservableCollection<BuildingCommonLocationImages>(await BuildingCommonLocationImagesDataStore.GetItemsAsyncByBuildingId(imageData.BuildingCommonLocationImages.BuildingLocationId));
                i = BuildingCommonLocationImagesItems.IndexOf(BuildingCommonLocationImagesItems.Where(c => c.Id == imageData.BuildingCommonLocationImages.Id).Single());
                //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                SelectedImage = BuildingCommonLocationImagesItems[i].ImageUrl;
            }
            else if (imageData.FormType == "A")
            {
                BuildingApartmentImagesItems = new ObservableCollection<BuildingApartmentImages>(await BuildingApartmentImagesDataStore.GetItemsAsyncByApartmentID(imageData.BuildingApartmentImages.BuildingApartmentId));
                i = BuildingApartmentImagesItems.IndexOf(BuildingApartmentImagesItems.Where(c => c.Id == imageData.BuildingApartmentImages.Id).Single());
                //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                SelectedImage = BuildingApartmentImagesItems[i].ImageUrl;
            }
        }
        public int i = 0;
        public ICommand NextCommand => new Command<object>(async (object obj) => await Next(obj));


        private async Task Next(object obj)
        {
            // MessagingCenter.Send(this, "Change", "Change");
            // var editorPage = obj as ImageEditorPageWithoutDetail;
            //   var prvBtn = editorPage.Content.FindByName("prvBtn") as ImageButton;
            //   var labelcomment = editorPage.Content.FindByName("labelcomment") as Label;
            //  labelcomment.IsVisible = false;
            // labelcomment.Text = string.Empty;
            //  var signaturepad = editorPage.Content.FindByName("signaturepad") as SignaturePadView;
            //signaturepad.Clear();
            //   signaturepad.ClearLabel.IsVisible = false;

            // prvBtn.IsVisible = false;
            if (imageData.FormType == "VP")
            {
                if (i != VisualProjectLocationPhotoItems.Count - 1)
                {
                    //  signaturepad.Clear();
                    i++;
                    //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                    SelectedImage = VisualProjectLocationPhotoItems[i].ImageUrl;
                }
                else
                {
                    //    SelectedImage = VisualProjectLocationPhotoItems[i].ImageUrl;
                    //  return;
                    //await Shell.Current.DisplayAlert("Alert", "It is last photo", "Ok", "cancel");
                    //  i = i - 1;

                    //var Nextbutton = editorPage.Content.FindByName("btnNext") as ImageButton;
                    //   Nextbutton.IsVisible = false;
                }
            }
            else if (imageData.FormType == "VB")
            {
                if (i != VisualBuildingLocationPhotoItems.Count - 1)
                {
                    // signaturepad.Clear();
                    i++;
                    //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                    SelectedImage = VisualBuildingLocationPhotoItems[i].ImageUrl;
                }
                else
                {

                    //await Shell.Current.DisplayAlert("Alert", "It is last photo", "Ok", "cancel");
                    //  i = i - 1;

                    //  var Nextbutton = editorPage.Content.FindByName("btnNext") as ImageButton;
                    //  Nextbutton.IsVisible = false;
                }

            }
            else if (imageData.FormType == "VA")
            {
                if (i != VisualApartmentLocationPhotoItems.Count - 1)
                {
                    //   signaturepad.Clear();
                    i++;
                    //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                    SelectedImage = VisualApartmentLocationPhotoItems[i].ImageUrl;
                }
                else
                {

                    //await Shell.Current.DisplayAlert("Alert", "It is last photo", "Ok", "cancel");
                    //  i = i - 1;

                    // var Nextbutton = editorPage.Content.FindByName("btnNext") as ImageButton;
                    //   Nextbutton.IsVisible = false;
                }

            }
            if (imageData.FormType == "P")
            {
                if (i != ProjectCommonLocationImagesItems.Count - 1)
                {
                    //  signaturepad.Clear();
                    i++;
                    //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                    SelectedImage = ProjectCommonLocationImagesItems[i].ImageUrl;
                }
                else
                {

                    //await Shell.Current.DisplayAlert("Alert", "It is last photo", "Ok", "cancel");
                    //  i = i - 1;

                    //  var Nextbutton = editorPage.Content.FindByName("btnNext") as ImageButton;
                    //  Nextbutton.IsVisible = false;
                }
            }
            else if (imageData.FormType == "B")
            {
                if (i != BuildingCommonLocationImagesItems.Count - 1)
                {
                    //  signaturepad.Clear();
                    i++;
                    //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                    SelectedImage = BuildingCommonLocationImagesItems[i].ImageUrl;
                }
                else
                {

                    //await Shell.Current.DisplayAlert("Alert", "It is last photo", "Ok", "cancel");
                    //  i = i - 1;

                    //   var Nextbutton = editorPage.Content.FindByName("btnNext") as ImageButton;
                    //  Nextbutton.IsVisible = false;
                }
            }
            else if (imageData.FormType == "A")
            {
                if (i != BuildingApartmentImagesItems.Count - 1)
                {
                    //  signaturepad.Clear();
                    i++;
                    //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                    SelectedImage = BuildingApartmentImagesItems[i].ImageUrl;
                }
                else
                {

                    //await Shell.Current.DisplayAlert("Alert", "It is last photo", "Ok", "cancel");
                    //  i = i - 1;

                    //   var Nextbutton = editorPage.Content.FindByName("btnNext") as ImageButton;
                    // Nextbutton.IsVisible = false;
                }
            }

            await Task.FromResult(true);
        }
        private async Task Prv(object obj)
        {
            // MessagingCenter.Send(this, "Change", "Change");
            var editorPage = obj as ImageEditorPageWithoutDetail;
            var Nextbutton = editorPage.Content.FindByName("btnNext") as ImageButton;
            //  var labelcomment = editorPage.Content.FindByName("labelcomment") as Label;
            //  labelcomment.IsVisible = false;
            //  labelcomment.Text = string.Empty;
            //  Nextbutton.IsVisible = false;

            // var signaturepad = editorPage.Content.FindByName("signaturepad") as SignaturePadView;
            // signaturepad.Clear();
            //signaturepad.ClearLabel.IsVisible = false;
            if (imageData.FormType == "VP")
            {
                if (i > 0)
                {
                    i--;
                    //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                    SelectedImage = VisualProjectLocationPhotoItems[i].ImageUrl;

                }
                else
                {
                    // var prvBtn = editorPage.Content.FindByName("prvBtn") as ImageButton;
                    //prvBtn.IsVisible = false;
                    //  await Shell.Current.DisplayAlert("Alert", "It is first photo", "Ok", "cancel");
                    //  i = i + 1;

                    //var Nextbutton = editorPage.Content.FindByName("btnNext") as ImageButton;
                    //Nextbutton.IsEnabled = false;
                }

            }
            else if (imageData.FormType == "VB")
            {
                if (i > 0)
                {
                    i--;
                    //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                    SelectedImage = VisualBuildingLocationPhotoItems[i].ImageUrl;
                }
                else
                {
                    //var prvBtn = editorPage.Content.FindByName("prvBtn") as ImageButton;
                    //  prvBtn.IsVisible = false;
                    //  await Shell.Current.DisplayAlert("Alert", "It is first photo", "Ok", "cancel");
                    //  i = i + 1;

                    //var Nextbutton = editorPage.Content.FindByName("btnNext") as ImageButton;
                    //Nextbutton.IsEnabled = false;
                }

            }
            else if (imageData.FormType == "VA")
            {
                if (i > 0)
                {
                    i--;
                    //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                    SelectedImage = VisualApartmentLocationPhotoItems[i].ImageUrl;
                }
                else
                {
                    // var prvBtn = editorPage.Content.FindByName("prvBtn") as ImageButton;
                    //  prvBtn.IsVisible = false;
                    //  await Shell.Current.DisplayAlert("Alert", "It is first photo", "Ok", "cancel");
                    //  i = i + 1;

                    //var Nextbutton = editorPage.Content.FindByName("btnNext") as ImageButton;
                    //Nextbutton.IsEnabled = false;
                }
            }
            if (imageData.FormType == "P")
            {
                if (i > 0)
                {
                    i--;
                    //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                    SelectedImage = ProjectCommonLocationImagesItems[i].ImageUrl;
                }
                else
                {
                    // var prvBtn = editorPage.Content.FindByName("prvBtn") as ImageButton;
                    //  prvBtn.IsVisible = false;
                    //  await Shell.Current.DisplayAlert("Alert", "It is first photo", "Ok", "cancel");
                    //  i = i + 1;

                    //var Nextbutton = editorPage.Content.FindByName("btnNext") as ImageButton;
                    //Nextbutton.IsEnabled = false;
                }
            }
            else if (imageData.FormType == "B")
            {
                if (i > 0)
                {
                    i--;
                    //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                    SelectedImage = BuildingCommonLocationImagesItems[i].ImageUrl;
                }
                else
                {
                    // var prvBtn = editorPage.Content.FindByName("prvBtn") as ImageButton;
                    //  prvBtn.IsVisible = false;
                    //  await Shell.Current.DisplayAlert("Alert", "It is first photo", "Ok", "cancel");
                    //  i = i + 1;

                    //var Nextbutton = editorPage.Content.FindByName("btnNext") as ImageButton;
                    //Nextbutton.IsEnabled = false;
                }
            }
            else if (imageData.FormType == "A")
            {
                if (i > 0)
                {
                    i--;
                    //imageData.VisualProjectLocationPhoto = VisualProjectLocationPhotoItems[i];
                    SelectedImage = BuildingApartmentImagesItems[i].ImageUrl;
                }
                else
                {
                    var prvBtn = editorPage.Content.FindByName("prvBtn") as ImageButton;
                    //   prvBtn.IsVisible = false;
                    //  await Shell.Current.DisplayAlert("Alert", "It is first photo", "Ok", "cancel");
                    //  i = i + 1;

                    //var Nextbutton = editorPage.Content.FindByName("btnNext") as ImageButton;
                    //Nextbutton.IsEnabled = false;
                }
            }
            await Task.FromResult(true);
        }
        public ICommand DeleteCommand => new Command<object>(async (object obj) => await Delete(obj));


        private async Task Delete(object obj)
        {

            var editorPage = obj as ImageEditorPageWithoutDetail;
            var result = await Shell.Current.DisplayAlert(
               "Alert",
               "Are you sure you want to remove?",
               "Yes", "No");

            if (result)
            {


                if (imageData.FormType == "VP")
                {
                    if (App.IsInvasive == false)
                        await VisualProjectLocationPhotoDataStore.DeleteItemAsync(VisualProjectLocationPhotoItems[i], imageData.IsEditVisual);
                    else
                        await InvasiveVisualProjectLocationPhotoDataStore.DeleteItemAsync(VisualProjectLocationPhotoItems[i], imageData.IsEditVisual);

                    if (App.IsInvasive == false)
                        VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualProjectLocationPhoto.VisualLocationId, false));
                    else
                        VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await InvasiveVisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualProjectLocationPhoto.VisualLocationId, false));
                    i--;
                    int Count = VisualProjectLocationPhotoItems.Count;
                    if (Count == 0)
                    {
                        await App.Current.MainPage.Navigation.PopAsync();

                    }
                    if (Count == 1)
                    {
                        i = 1;
                        await Prv(obj);

                    }


                    if (i == Count - 1)
                    {
                        //i = 0;
                        await Prv(obj);
                    }
                    else
                    {
                        await Next(obj);

                    }
                    //int Count = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualProjectLocationPhoto.VisualID)).Count;
                    //if (Count != 0)
                    //    await Next(obj);
                    //else
                    //    await App.Current.MainPage.Navigation.PopAsync();

                }
                else if (imageData.FormType == "VB")
                {
                    if (App.IsInvasive == false)
                        await VisualBuildingLocationPhotoDataStore.DeleteItemAsync(VisualBuildingLocationPhotoItems[i]);
                    else
                        await InvasiveVisualBuildingLocationPhotoDataStore.DeleteItemAsync(VisualBuildingLocationPhotoItems[i]);


                    if (App.IsInvasive == false)
                        VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualBuildingLocationPhoto.VisualBuildingId, false));
                    else
                        VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await InvasiveVisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualBuildingLocationPhoto.VisualBuildingId, false));
                    i--;

                    //int Count = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualBuildingLocationPhoto.VisualID)).Count;
                    //if (Count != 0)
                    //    await Next(obj);
                    //else
                    //    await App.Current.MainPage.Navigation.PopAsync();
                    int Count = VisualBuildingLocationPhotoItems.Count;
                    if (Count == 0)
                    {
                        await App.Current.MainPage.Navigation.PopAsync();

                    }
                    if (Count == 1)
                    {
                        i = 1;
                        await Prv(obj);
                    }


                    if (i == Count - 1)

                        await Prv(obj);

                    else

                        await Next(obj);


                }
                else if (imageData.FormType == "VA")
                {
                    if (App.IsInvasive == false)
                        await VisualApartmentLocationPhotoDataStore.DeleteItemAsync(VisualApartmentLocationPhotoItems[i]);
                    else
                        await InvasiveVisualApartmentLocationPhotoDataStore.DeleteItemAsync(VisualApartmentLocationPhotoItems[i]);
                    if (App.IsInvasive == false)
                        VisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualApartmentLocationPhoto.VisualApartmentId, false));
                    else
                        VisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(await InvasiveVisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualApartmentLocationPhoto.VisualApartmentId, false));

                    i--;
                    int Count = VisualApartmentLocationPhotoItems.Count;
                    if (Count == 0)
                    {
                        await App.Current.MainPage.Navigation.PopAsync();

                    }
                    if (Count == 1)
                    {
                        i = 1;
                        await Prv(obj);
                    }


                    if (i == Count - 1)

                        await Prv(obj);

                    else

                        await Next(obj);
                    //int Count = new ObservableCollection<VisualApartmentLocationPhoto>(await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualApartmentLocationPhoto.VisualID)).Count;
                    //if (Count != 0)
                    //    await Next(obj);
                    //else
                    //    await App.Current.MainPage.Navigation.PopAsync();
                }
                if (imageData.FormType == "P")
                {
                    await ProjectCommonLocationImagesDataStore.DeleteItemAsync(ProjectCommonLocationImagesItems[i]);
                    ProjectCommonLocationImagesItems = new ObservableCollection<ProjectCommonLocationImages>(await ProjectCommonLocationImagesDataStore.GetItemsAsyncByProjectLocationId(imageData.ProjectCommonLocationImages.ProjectLocationId));
                    i--;
                    int Count = ProjectCommonLocationImagesItems.Count;
                    if (Count == 0)
                    {
                        await App.Current.MainPage.Navigation.PopAsync();

                    }
                    if (Count == 1)
                    {
                        i = 1;
                        await Prv(obj);
                    }


                    if (i == Count - 1)

                        await Prv(obj);

                    else

                        await Next(obj);
                    //int Count = new ObservableCollection<ProjectCommonLocationImages>(await ProjectCommonLocationImagesDataStore.GetItemsAsyncByProjectLocationId(imageData.ProjectCommonLocationImages.ProjectLocationId)).Count;
                    //if (Count != 0)
                    //    await Next(obj);
                    //else
                    //    await App.Current.MainPage.Navigation.PopAsync();
                }
                else if (imageData.FormType == "B")
                {
                    await BuildingCommonLocationImagesDataStore.DeleteItemAsync(BuildingCommonLocationImagesItems[i]);
                    BuildingCommonLocationImagesItems = new ObservableCollection<BuildingCommonLocationImages>(await BuildingCommonLocationImagesDataStore.GetItemsAsyncByBuildingId(imageData.BuildingCommonLocationImages.BuildingLocationId));
                    i--;
                    //    int Count = new ObservableCollection<BuildingCommonLocationImages>(await BuildingCommonLocationImagesDataStore.GetItemsAsyncByBuildingId(imageData.BuildingCommonLocationImages.BuildingId)).Count;
                    //    if (Count != 0)
                    //        await Next(obj);
                    //    else
                    //        await App.Current.MainPage.Navigation.PopAsync();
                    int Count = BuildingCommonLocationImagesItems.Count;
                    if (Count == 0)
                    {
                        await App.Current.MainPage.Navigation.PopAsync();

                    }
                    if (Count == 1)
                    {
                        i = 1;
                        await Prv(obj);
                    }


                    if (i == Count - 1)

                        await Prv(obj);

                    else

                        await Next(obj);
                }
                else if (imageData.FormType == "A")
                {
                    await BuildingApartmentImagesDataStore.DeleteItemAsync(BuildingApartmentImagesItems[i]);
                    BuildingApartmentImagesItems = new ObservableCollection<BuildingApartmentImages>(await BuildingApartmentImagesDataStore.GetItemsAsyncByApartmentID(imageData.BuildingApartmentImages.BuildingApartmentId));
                    i--;
                    //int Count = new ObservableCollection<BuildingApartmentImages>(await BuildingApartmentImagesDataStore.GetItemsAsyncByApartmentID(imageData.BuildingApartmentImages.ApartmentID)).Count;
                    //if (Count != 0)
                    //    await Next(obj);
                    //else
                    //    await App.Current.MainPage.Navigation.PopAsync();
                    int Count = BuildingApartmentImagesItems.Count;
                    if (Count == 0)
                    {
                        await App.Current.MainPage.Navigation.PopAsync();

                    }
                    if (Count == 1)
                    {
                        i = 1;
                        await Prv(obj);
                    }


                    if (i == Count - 1)

                        await Prv(obj);

                    else

                        await Next(obj);
                }
            }

        }
        public ICommand PrvCommand => new Command<object>(async (object obj) => await Prv(obj));





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

            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage.Navigation.PopAsync();
                });
            });

            //    var selectedOption = await App.Current.MainPage.DisplayAlert("Discard Photo",
            //         "if you discard now,you'll lose your photos and edits.", "Discard", "Keep");
            //    if (selectedOption) await App.Current.MainPage.Navigation.PopAsync();
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
        private bool _isBusyProgress;

        public bool IsBusyProgress
        {
            get { return _isBusyProgress; }
            set { _isBusyProgress = value; OnPropertyChanged("IsBusyProgress"); }
        }

        private async void SaveImageCommandExecute(object obj)
        {
            IsBusyProgress = true;
            var editorPage = obj as ImageEditorPageWithoutDetail;

            //var signaturepad = editorPage.Content.FindByName("signaturepad") as SignaturePadView;
            //signaturepad.ClearLabel.IsVisible = false;

            // var gritoolbar = editorPage.Content.FindByName("gridtoolbar") as Grid;
            //  gritoolbar.IsVisible = false;

            var savebutton = editorPage.Content.FindByName("GridOperation") as Grid;
            savebutton.IsVisible = false;
            //var detailGrid = editorPage.Content.FindByName("detailGrid") as Grid;
            //detailGrid.IsVisible = false;

            // var imgcolors = editorPage.Content.FindByName("imgcolors") as Image;
            //  imgcolors.IsVisible = false;

            // var commentslider = editorPage.Content.FindByName("commentcolorslider") as Slider;
            // commentslider.IsVisible = false;

            // var txtName = editorPage.Content.FindByName("txtName") as BorderlessEntry;
            //  commentslider.IsVisible = false;
            //   var txtDescription = editorPage.Content.FindByName("txtDescription") as XEditor;
            //commentslider.IsVisible = false;

            //var scratchslider = editorPage.Content.FindByName("scratchcolorslider") as Slider;
            //scratchslider.IsVisible = false;


            //  string path = await CrossScreenshot.Current.CaptureAndSaveAsync();
            byte[] resizedImage = DependencyService.Get<IImageService>().ResizeTheImage(await CrossScreenshot.Current.CaptureAsync(), 2000, 1800);
            string path = await DependencyService.Get<ISaveFile>().SaveFiles(Guid.NewGuid().ToString(), resizedImage);
            // imageData.Path = filepath;
            //     await App.Current.MainPage.Navigation.PopAsync();
            //ImageData d = new ImageData();
            imageData.Path = path;
            if (imageData.FormType == "VP")
            {
                VisualProjectLocationPhotoItems[i].ImageUrl = path;
                if (App.IsInvasive == false)
                    await VisualProjectLocationPhotoDataStore.UpdateItemAsync(VisualProjectLocationPhotoItems[i], imageData.IsEditVisual);
                else
                    await InvasiveVisualProjectLocationPhotoDataStore.UpdateItemAsync(VisualProjectLocationPhotoItems[i], imageData.IsEditVisual);
            }
            else if (imageData.FormType == "VB")
            {
                VisualBuildingLocationPhotoItems[i].ImageUrl = path;
                // imageData.VisualBuildingLocationPhoto.Image = path;
                if (App.IsInvasive == false)
                    await VisualBuildingLocationPhotoDataStore.UpdateItemAsync(VisualBuildingLocationPhotoItems[i]);
                else
                    await InvasiveVisualBuildingLocationPhotoDataStore.UpdateItemAsync(VisualBuildingLocationPhotoItems[i]);
            }
            else if (imageData.FormType == "VA")
            {
                string oldp = VisualApartmentLocationPhotoItems[i].ImageUrl;
                VisualApartmentLocationPhotoItems[i].ImageUrl = path;
                if (App.IsInvasive == false)
                    await VisualApartmentLocationPhotoDataStore.UpdateItemAsync(VisualApartmentLocationPhotoItems[i]);
                else
                    await InvasiveVisualApartmentLocationPhotoDataStore.UpdateItemAsync(VisualApartmentLocationPhotoItems[i]);
            }
            if (imageData.FormType == "P")
            {
                ProjectCommonLocationImagesItems[i].ImageUrl = path;
                await ProjectCommonLocationImagesDataStore.UpdateItemAsync(ProjectCommonLocationImagesItems[i]);
            }
            else if (imageData.FormType == "B")
            {
                BuildingCommonLocationImagesItems[i].ImageUrl = path;
                await BuildingCommonLocationImagesDataStore.UpdateItemAsync(BuildingCommonLocationImagesItems[i]);
            }
            else if (imageData.FormType == "A")
            {
                BuildingApartmentImagesItems[i].ImageUrl = path;

                await BuildingApartmentImagesDataStore.UpdateItemAsync(BuildingApartmentImagesItems[i]);
            }

            // signaturepad.Clear();
            //  gritoolbar.IsVisible = true;
            savebutton.IsVisible = true;
            //commentslider.IsVisible = true;
            //scratchslider.IsVisible = true;
            IsBusyProgress = false;
            var labelcomment = editorPage.Content.FindByName("labelcomment") as Label;
            //  labelcomment.IsVisible = false;
            labelcomment.Text = string.Empty;
            //  VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(imageData.VisualProjectLocationPhoto.VisualID));
            // await Next(obj);

            // imageData.Name = txtName.Text;
            // imageData.Description = txtDescription.Text;
            //  Callback?.Invoke(imageData);

        }
        public async void Save(byte[] arr)
        {
            IsBusyProgress = true;
           
            string path = await DependencyService.Get<ISaveFile>().SaveFiles(Guid.NewGuid().ToString(), arr);

           
            if (imageData.FormType == "VP")
            {
                VisualProjectLocationPhotoItems[i].ImageUrl = path;
                if (App.IsInvasive == false)
                    await VisualProjectLocationPhotoDataStore.UpdateItemAsync(VisualProjectLocationPhotoItems[i], imageData.IsEditVisual);
                else
                    await InvasiveVisualProjectLocationPhotoDataStore.UpdateItemAsync(VisualProjectLocationPhotoItems[i], imageData.IsEditVisual);
            }
            else if (imageData.FormType == "VB")
            {
                VisualBuildingLocationPhotoItems[i].ImageUrl = path;
                // imageData.VisualBuildingLocationPhoto.Image = path;
                if (App.IsInvasive == false)
                    await VisualBuildingLocationPhotoDataStore.UpdateItemAsync(VisualBuildingLocationPhotoItems[i]);
                else
                    await InvasiveVisualBuildingLocationPhotoDataStore.UpdateItemAsync(VisualBuildingLocationPhotoItems[i]);
            }
            else if (imageData.FormType == "VA")
            {
                string oldp = VisualApartmentLocationPhotoItems[i].ImageUrl;
                VisualApartmentLocationPhotoItems[i].ImageUrl = path;
                if (App.IsInvasive == false)
                    await VisualApartmentLocationPhotoDataStore.UpdateItemAsync(VisualApartmentLocationPhotoItems[i]);
                else
                    await InvasiveVisualApartmentLocationPhotoDataStore.UpdateItemAsync(VisualApartmentLocationPhotoItems[i]);
            }
            if (imageData.FormType == "P")
            {
                ProjectCommonLocationImagesItems[i].ImageUrl = path;
                await ProjectCommonLocationImagesDataStore.UpdateItemAsync(ProjectCommonLocationImagesItems[i]);
            }
            else if (imageData.FormType == "B")
            {
                BuildingCommonLocationImagesItems[i].ImageUrl = path;
                await BuildingCommonLocationImagesDataStore.UpdateItemAsync(BuildingCommonLocationImagesItems[i]);
            }
            else if (imageData.FormType == "A")
            {
                BuildingApartmentImagesItems[i].ImageUrl = path;

                await BuildingApartmentImagesDataStore.UpdateItemAsync(BuildingApartmentImagesItems[i]);
            }

           
            IsBusyProgress = false;
            
            await Next(null);

           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImagePath">Send ImagePath that need to Edit</param>
        /// <param name="callbackEventHandler">return imagepath after click save button</param>
        /// <returns>return ImagePath on callback(imagepath)</returns>
        public static async Task EditImage(ImageData data, CallbackEventHandler callbackEventHandler)
        {

            var imgviewmodel = new CurrentWithoutDetail(data);
            imgviewmodel.Callback += callbackEventHandler;
            if (Device.RuntimePlatform == Device.Android)
            {
                var imgpage = new ImageEditorPageWithoutDetail(string.Empty);
                imgpage.BindingContext = imgviewmodel;
                await App.Current.MainPage.Navigation.PushAsync(imgpage, true);
            }
            if (Device.RuntimePlatform == Device.iOS)
            {
                var imgpage = new ImageEditorPageWithoutDetail(string.Empty);
                imgpage.BindingContext = imgviewmodel;
                await App.Current.MainPage.Navigation.PushAsync(imgpage, true);
            }

        }

    }
}

