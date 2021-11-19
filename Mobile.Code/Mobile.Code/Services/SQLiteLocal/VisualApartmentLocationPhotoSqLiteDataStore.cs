using Mobile.Code.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Code.Services.SQLiteLocal
{
    public class VisualApartmentLocationPhotoSqLiteDataStore : IVisualApartmentLocationPhotoDataStore
    {
        public Task<bool> AddItemAsync(VisualApartmentLocationPhoto item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(VisualApartmentLocationPhoto item)
        {
            throw new NotImplementedException();
        }

        public Task<VisualApartmentLocationPhoto> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VisualApartmentLocationPhoto>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VisualApartmentLocationPhoto>> GetItemsAsyncByProjectIDSqLite(string aptId, bool loadLocally)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VisualApartmentLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(VisualApartmentLocationPhoto item)
        {
            throw new NotImplementedException();
        }
    }
}
