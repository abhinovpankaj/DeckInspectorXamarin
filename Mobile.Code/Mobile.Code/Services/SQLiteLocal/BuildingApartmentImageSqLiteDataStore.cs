using Mobile.Code.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Code.Services.SQLiteLocal
{
    public class BuildingApartmentImagesSqLiteDataStore : IBuildingApartmentImages
    {
        public Task<bool> AddItemAsync(BuildingApartmentImages item)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteItemAsync(BuildingApartmentImages item)
        {
            throw new NotImplementedException();
        }

        public Task<BuildingApartmentImages> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BuildingApartmentImages>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BuildingApartmentImages>> GetItemsAsyncByApartmentID(string ApartmentID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(BuildingApartmentImages item)
        {
            throw new NotImplementedException();
        }
    }
}
