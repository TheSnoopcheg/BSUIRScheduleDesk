using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Services;

public interface INetworkService
{
    Task<T?> GetAsync<T>(string? url);
}
