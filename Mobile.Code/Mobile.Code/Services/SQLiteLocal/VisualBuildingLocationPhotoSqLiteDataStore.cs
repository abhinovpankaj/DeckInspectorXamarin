using Mobile.Code.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Code.Services.SQLiteLocal
{
    public class VisualBuildingLocationPhotoSqLiteDataStore : IVisualBuildingLocationPhotoDataStore
    {
        public Task<bool> AddItemAsync(VisualBuildingLocationPhoto item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(VisualBuildingLocationPhoto item)
        {
            throw new NotImplementedException();
        }

        public Task<VisualBuildingLocationPhoto> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VisualBuildingLocationPhoto>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VisualBuildingLocationPhoto>> GetItemsAsyncByProjectIDSqLite(string buildingId, bool loadLocally)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VisualBuildingLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(VisualBuildingLocationPhoto item)
        {
            throw new NotImplementedException();
        }
    }
}
