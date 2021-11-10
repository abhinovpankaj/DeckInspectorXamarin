using Mobile.Code.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Code.Services.SQLiteLocal
{
    public class VisualProjectLocationPhotoSqLiteDataStore : IVisualProjectLocationPhotoDataStore
    {
        public Task<bool> AddItemAsync(VisualProjectLocationPhoto item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual)
        {
            throw new NotImplementedException();
        }

        public Task<VisualProjectLocationPhoto> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual)
        {
            throw new NotImplementedException();
        }
    }
}
