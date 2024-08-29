using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.services
{
    public class NetworkService : INetworkService
    {
        private HttpClient httpClient = new HttpClient();
        public async Task<T> GetAsync<T>(string? url)
        {
            try
            {
                Stream content = await httpClient.GetStreamAsync(url);
                T? obj = await JsonSerializer.DeserializeAsync<T>(content);
                return obj!;
            }
            catch (Exception ex) { }
            return default;
        }
    }
}
