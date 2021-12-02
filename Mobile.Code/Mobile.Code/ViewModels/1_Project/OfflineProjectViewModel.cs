using Mobile.Code.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
     public class OfflineProjectViewModel :BaseViewModel
    {
        public OfflineProjectViewModel()
        {
            SyncProjectCommand = new Command<Project>(async (Project project) => await PushProjectToServer(project));
        }
        public Command SyncProjectCommand { get; set; }
        private ObservableCollection<Project> _offlineProjects;
        public ObservableCollection<Project> OfflineProjects
        {
            get { return _offlineProjects; }
            set { _offlineProjects = value; OnPropertyChanged("OfflineProjects"); }
        }
        private bool _Isbusyprog;
        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }
        private async Task<bool> Running()
        {
            IsBusyProgress = true;
            
            OfflineProjects = new ObservableCollection<Project>(await ProjectSQLiteDataStore.GetItemsAsync(true));
           
            return await Task.FromResult(true);


        }
        public async Task LoadData()
        {
            // AllProjects = new ObservableCollection<Project>();
            bool complete = await Task.Run(Running);
            if (complete)
            {

                IsBusyProgress = false;

            }
        }

        //Sync Project Data
        private async Task PushProjectToServer(Project project)
        {
            bool syncedSuccessfully = true;
            IsBusyProgress = true;
            
            var res = await Task.Run(async() =>
            {
                Response response = new Response();
                var localProject = await ProjectSQLiteDataStore.GetItemAsync(project.Id);

                //insert in db
                localProject.Id = null;
                var result = await ProjectDataStore.AddItemAsync(localProject);
                var uploadedProject = JsonConvert.DeserializeObject<Project>(result.Data.ToString());
                if (result.Status == ApiResult.Success)
                {
                    response.Message = response.Message + "\n" + project.Name + "added successfully.";
                    //get project common loc and Building details
                    var ProjectLocationItems = new ObservableCollection<ProjectLocation>(await ProjectLocationSqLiteDataStore
                        .GetItemsAsyncByProjectID(project.Id));

                    foreach (var item in ProjectLocationItems)
                    {
                        //insert projectlocations
                        item.ProjectId = uploadedProject.Id;
                        string localId = item.Id;
                        item.Id = null;
                        var resultProjLocation = await ProjectLocationDataStore.AddItemAsync(item);
                        var uploadedProjectLocation = JsonConvert.DeserializeObject<ProjectLocation>(resultProjLocation.Data.ToString());
                        if (resultProjLocation.Status == ApiResult.Success)
                        {
                            response.Message = response.Message + "\n" + item.Name + "added successully, locations will be added now.";
                            var VisualFormProjectLocationItems = new ObservableCollection<ProjectLocation_Visual>
                                (await VisualFormProjectLocationSqLiteDataStore
                                .GetItemsAsyncByProjectLocationId(localId));

                            foreach (var formLocationItem in VisualFormProjectLocationItems)
                            {
                                //add lowest level  location data
                                var images = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore
                                    .GetItemsAsyncByLoacationIDSqLite(formLocationItem.Id, false));
                                List<string> imageList = images.Select(c => c.ImageUrl).ToList();
                                formLocationItem.ProjectLocationId = uploadedProjectLocation.Id;
                                formLocationItem.Id = null;
                                var locationResult = await VisualFormProjectLocationDataStore.AddItemAsync(formLocationItem, imageList);
                                if (response.Status == ApiResult.Success)
                                {
                                    response.Message = response.Message + "\n" + formLocationItem.Name + "added successully.";
                                }
                                else
                                {
                                    syncedSuccessfully = false;
                                    response.Message = response.Message + "\n" + formLocationItem.Name + "failed to added.";
                                }

                            }
                        }
                        else
                        {
                            syncedSuccessfully = false;
                            response.Message = response.Message + "\n" + item.Name + "failed to sync, skipping the children";
                        }
                    }

                    var ProjectBuildingItems = new ObservableCollection<ProjectBuilding>(await ProjectBuildingSqLiteDataStore
                        .GetItemsAsyncByProjectID(project.Id));
                    foreach (var item in ProjectBuildingItems)
                    {
                        //insert buildinglocations
                        item.ProjectId = uploadedProject.Id;
                        string localId = item.Id;
                        item.Id = null;
                        var resultBuilding = await ProjectBuildingDataStore.AddItemAsync(item);
                        if (resultBuilding.Status == ApiResult.Success)
                        {
                            response.Message = response.Message + "\n" + item.Name + "added successully.";
                            var BuildingLocations = new ObservableCollection<BuildingLocation>(await BuildingLocationSqLiteDataStore
                            .GetItemsAsyncByBuildingId(localId));
                            foreach (var buildingLoc in BuildingLocations)
                            {
                                buildingLoc.BuildingId = resultBuilding.ID;
                                string localBuildId = buildingLoc.Id;
                                buildingLoc.Id = null;
                                result = await BuildingLocationDataStore.AddItemAsync(buildingLoc);
                                if (result.Status == ApiResult.Success)
                                {
                                    response.Message = response.Message + "\n" + buildingLoc.Name + "added successully. Locations will be added";
                                    var VisualFormBuildingLocationItems = new ObservableCollection<BuildingLocation_Visual>(await VisualFormBuildingLocationSqLiteDataStore
                                    .GetItemsAsyncByBuildingLocationId(localBuildId));

                                    foreach (var buildLocForm in VisualFormBuildingLocationItems)
                                    {
                                        var images = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore
                                    .GetItemsAsyncByProjectIDSqLite(localBuildId, false));
                                        List<string> imageList = images.Select(c => c.ImageUrl).ToList();
                                        buildLocForm.Id = null;
                                        buildLocForm.BuildingLocationId = result.ID;
                                        var locationResult = await VisualFormBuildingLocationDataStore.AddItemAsync(buildLocForm, imageList);
                                        if (response.Status == ApiResult.Success)
                                        {
                                            response.Message = response.Message + "\n" + buildLocForm.Name + "added successully.";
                                        }
                                        else
                                        {
                                            syncedSuccessfully = false;
                                            response.Message = response.Message + "\n" + buildLocForm.Name + "failed to added.";
                                        }
                                    }
                                }

                            }
                            var BuildingApartments = new ObservableCollection<BuildingApartment>(await BuildingApartmentSqLiteDataStore
                                .GetItemsAsyncByBuildingId(localId));
                            foreach (var apartment in BuildingApartments)
                            {
                                apartment.BuildingId = resultBuilding.ID;
                                string localaptId = apartment.Id;
                                apartment.Id = null;
                                result = await BuildingApartmentDataStore.AddItemAsync(apartment);
                                if (result.Status == ApiResult.Success)
                                {
                                    response.Message = response.Message + "\n" + apartment.Name + "added successully. Locations will be added";

                                    var VisualFormApartmentLocationItems = new ObservableCollection<Apartment_Visual>
                                        (await VisualFormApartmentSqLiteDataStore.GetItemsAsyncByApartmentId(localaptId));
                                    foreach (var aptLoc in VisualFormApartmentLocationItems)
                                    {
                                        var images = new ObservableCollection<VisualApartmentLocationPhoto>
                                            (await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectIDSqLite(aptLoc.Id, false));
                                        List<string> imageList = images.Select(c => c.ImageUrl).ToList();
                                        aptLoc.Id = null;
                                        aptLoc.BuildingApartmentId = result.ID;
                                        var locationResult = await VisualFormApartmentDataStore.AddItemAsync(aptLoc, imageList);
                                        if (response.Status == ApiResult.Success)
                                        {
                                            response.Message = response.Message + "\n" + aptLoc.Name + "added successully.";
                                        }
                                        else
                                        {
                                            syncedSuccessfully = false;
                                            response.Message = response.Message + "\n" + aptLoc.Name + "failed to added.";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            response.Message = response.Message + item.Name + "failed to be added.";
                        }
                    }
                }
                else
                {
                    syncedSuccessfully = false;
                    response.Message = response.Message + "\n" + project.Name + "failed to add.";
                }
                return response;
            });
            project.IsSynced = syncedSuccessfully;
            await ProjectSQLiteDataStore.UpdateItemAsync(project);
            IsBusyProgress = false;
        }
    }
}
