using Mobile.Code.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Code.Services.SQLiteLocal
{
    public class VisualFormApartmentSqLiteDataStore : IVisualFormApartmentDataStore
    {
        public Task<Response> AddItemAsync(Apartment_Visual item, IEnumerable<string> ImageList)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteItemAsync(Apartment_Visual item)
        {
            throw new NotImplementedException();
        }

        public Task<Apartment_Visual> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Apartment_Visual>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Apartment_Visual>> GetItemsAsyncByApartmentId(string ApartmentLocationId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateItemAsync(Apartment_Visual item, List<MultiImage> finelList, string imgType = "TRUE")
        {
            throw new NotImplementedException();
        }
    }
}
