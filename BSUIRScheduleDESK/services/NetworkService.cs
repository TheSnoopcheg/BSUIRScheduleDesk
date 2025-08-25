using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Services;

public class NetworkService : INetworkService
{
    private readonly HttpClient httpClient;
    private readonly HttpClientHandler httpHandler;

    public NetworkService()
    {
        httpHandler = new HttpClientHandler();
        httpHandler.CheckCertificateRevocationList = true;
        httpClient = new HttpClient(httpHandler);
    }
    public async Task<T?> GetAsync<T>(string? url)
    {
        try
        {
            Stream content = await httpClient.GetStreamAsync(url);
            T? obj = await JsonSerializer.DeserializeAsync<T>(content);
            return obj!;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return default;
    }
}
