using Mobile.Code.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Code.Services.SQLiteLocal
{
    class VisualFormProjectLocationSqLiteDataStore : IVisualFormProjectLocationDataStore
    {
        public Task<Response> AddItemAsync(ProjectLocation_Visual item, IEnumerable<string> ImageList = null)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteItemAsync(ProjectLocation_Visual item)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectLocation_Visual> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectLocation_Visual>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectLocation_Visual>> GetItemsAsyncByProjectLocationId(string projectLocationId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateItemAsync(ProjectLocation_Visual item, List<MultiImage> finelList, string imgType = "TRUE")
        {
            throw new NotImplementedException();
        }
    }
}
