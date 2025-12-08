using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Services;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Models;

public class EmployeeInfoModel
{
    private const string baseUrl = "https://iis.bsuir.by/api/v1/employees/details-url?urlId=";

    private readonly INetworkService _networkService;

    private EmployeeFullInfo? _employeeFullInfo;
    public EmployeeFullInfo? EmployeeFullInfo
    {
        get { return _employeeFullInfo; } 
        private set { _employeeFullInfo = value; }
    }

    public EmployeeInfoModel(INetworkService networkService)
    {
        _networkService = networkService; 
    }

    public async Task<bool> TryLoadEmployeeInfo(string? urlId)
    {
        var info = await _networkService.GetAsync<EmployeeFullInfo>($"{baseUrl}{urlId}");
        if (info == null)
            return false;

        EmployeeFullInfo = info;
        return true;
    }
}
