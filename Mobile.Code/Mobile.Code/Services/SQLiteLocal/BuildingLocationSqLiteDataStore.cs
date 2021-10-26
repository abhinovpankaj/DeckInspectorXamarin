using Mobile.Code.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Code.Services.SQLiteLocal
{
    public class BuildingLocationSqLiteDataStore : IBuildingLocation
    {
        public Task<Response> AddItemAsync(BuildingLocation item)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteItemAsync(BuildingLocation item)
        {
            throw new NotImplementedException();
        }

        public Task<BuildingLocation> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BuildingLocation>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BuildingLocation>> GetItemsAsyncByBuildingId(string BuildingId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(BuildingLocation item)
        {
            throw new NotImplementedException();
        }
    }
}
