namespace Mobile.Code.Services
{
    //public class AzureDataStore : IDataStore<Project>
    //{
    //    HttpClient client;
    //    IEnumerable<Project> items;

    //    public AzureDataStore()
    //    {
    //        client = new HttpClient();
    //        client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");

    //        items = new List<Project>();
    //    }

    //    bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
    //    public async Task<IEnumerable<Project>> GetItemsAsync(bool forceRefresh = false)
    //    {
    //        if (forceRefresh && IsConnected)
    //        {
    //            var json = await client.GetStringAsync($"api/item");
    //            items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Project>>(json));
    //        }

    //        return items;
    //    }

    //    public async Task<Project> GetItemAsync(string id)
    //    {
    //        if (id != null && IsConnected)
    //        {
    //            var json = await client.GetStringAsync($"api/item/{id}");
    //            return await Task.Run(() => JsonConvert.DeserializeObject<Project>(json));
    //        }

    //        return null;
    //    }

    //    public async Task<bool> AddItemAsync(Project item)
    //    {
    //        if (item == null || !IsConnected)
    //            return false;

    //        var serializedItem = JsonConvert.SerializeObject(item);

    //        var response = await client.PostAsync($"api/item", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

    //        return response.IsSuccessStatusCode;
    //    }

    //    public async Task<bool> UpdateItemAsync(Project item)
    //    {
    //        if (item == null || item.Id == null || !IsConnected)
    //            return false;

    //        var serializedItem = JsonConvert.SerializeObject(item);
    //        var buffer = Encoding.UTF8.GetBytes(serializedItem);
    //        var byteContent = new ByteArrayContent(buffer);

    //        var response = await client.PutAsync(new Uri($"api/item/{item.Id}"), byteContent);

    //        return response.IsSuccessStatusCode;
    //    }

    //    public async Task<bool> DeleteItemAsync(string id)
    //    {
    //        if (string.IsNullOrEmpty(id) && !IsConnected)
    //            return false;

    //        var response = await client.DeleteAsync($"api/item/{id}");

    //        return response.IsSuccessStatusCode;
    //    }
    //}
}
