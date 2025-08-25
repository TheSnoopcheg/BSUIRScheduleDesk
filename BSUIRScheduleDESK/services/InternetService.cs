using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Services;

public class InternetService : IInternetService
{
    private HttpClient _httpClient = new HttpClient();
    
    public ConnectionStatus CheckInternet()
    {
        // Проверить подключение к dns.msftncsi.com
        try
        {
            IPHostEntry entry = Dns.GetHostEntry("dns.msftncsi.com");
            if (entry.AddressList.Length == 0)
            {
                return ConnectionStatus.NotConnected;
            }
            else
            {
                if (!entry.AddressList[0].ToString().Equals("131.107.255.255"))
                {
                    return ConnectionStatus.LimitedAccess;
                }
            }
        }
        catch
        {
            return ConnectionStatus.NotConnected;
        }

        // Проверить загрузку документа ncsi.txt
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://www.msftncsi.com/ncsi.txt");
        try
        {
            HttpResponseMessage responce = _httpClient.Send(request);

            if (responce.StatusCode != HttpStatusCode.OK)
            {
                return ConnectionStatus.LimitedAccess;
            }
            using (StreamReader sr = new StreamReader(responce.Content.ReadAsStream()))
            {
                if (sr.ReadToEnd().Equals("Microsoft NCSI"))
                {
                    return ConnectionStatus.Connected;
                }
                else
                {
                    return ConnectionStatus.LimitedAccess;
                }
            }
        }
        catch
        {
            return ConnectionStatus.NotConnected;
        }

    }
    public ConnectionStatus PingServer(string? url)
    {
        Ping ping = new Ping();
        PingReply pingReply = ping.Send(url!);
        return pingReply.Status == IPStatus.Success ? ConnectionStatus.Connected : ConnectionStatus.NotConnected;
    }
    public async Task<ConnectionStatus> CheckServerAccessAsync(string? url)
    {
        try
        {
            HttpResponseMessage responce = await _httpClient.GetAsync(url!);
            if (responce.StatusCode == HttpStatusCode.OK)
                return ConnectionStatus.Connected;
            else
                return ConnectionStatus.LimitedAccess;
        }
        catch { return ConnectionStatus.NotConnected; }
    }
}
