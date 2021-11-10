using Mobile.Code.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Code.Services.SQLiteLocal
{
    public class VisualFormBuildingLocationSqLiteDataStore : IVisualFormBuildingLocationDataStore
    {
        public Task<Response> AddItemAsync(BuildingLocation_Visual item, IEnumerable<string> ImageList)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteItemAsync(BuildingLocation_Visual item)
        {
            throw new NotImplementedException();
        }

        public Task<BuildingLocation_Visual> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BuildingLocation_Visual>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BuildingLocation_Visual>> GetItemsAsyncByBuildingLocationId(string buildingLocationId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateItemAsync(BuildingLocation_Visual item, List<MultiImage> finelList, string imgType = "TRUE")
        {
            throw new NotImplementedException();
        }
    }
}
