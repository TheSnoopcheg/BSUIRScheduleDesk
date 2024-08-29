using System.Threading.Tasks;

namespace BSUIRScheduleDESK.services
{
    public interface INetworkService
    {
        Task<T> GetAsync<T>(string? url);
    }
}
