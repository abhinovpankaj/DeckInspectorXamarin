using Mobile.Code.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Code.Services.SQLiteLocal
{
    public class BuildingCommonLocationImagesSqLiteDataStore : IBuildingCommonLocationImages
    {
        public Task<bool> AddItemAsync(BuildingCommonLocationImages item)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteItemAsync(BuildingCommonLocationImages item)
        {
            throw new NotImplementedException();
        }

        public Task<BuildingCommonLocationImages> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BuildingCommonLocationImages>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BuildingCommonLocationImages>> GetItemsAsyncByBuildingId(string BuildingId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(BuildingCommonLocationImages item)
        {
            throw new NotImplementedException();
        }
    }
}
