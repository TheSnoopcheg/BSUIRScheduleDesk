using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Services
{
    public enum ConnectionStatus
    {
        NotConnected,
        LimitedAccess,
        Connected
    }
    public interface IInternetService
    {
        ConnectionStatus CheckInternet();
        ConnectionStatus PingServer(string? url);
        Task<ConnectionStatus> CheckServerAccessAsync(string? url);

    }
}
