using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.ViewModels;

public class EmployeeInfoViewModel
{
    private readonly EmployeeInfoModel _model;

    public EmployeeFullInfo? EmployeeInfo => _model.EmployeeFullInfo;
    public bool[] IsShowAddInfo { get; set; } = new bool[6];
    public ObservableCollection<Contact> Contacts { get; set; } = new();
    public EmployeeInfoViewModel(EmployeeInfoModel model)
    {
        _model = model;
    }

    public async Task<bool> TryLoadEmployeeInfo(string? urlId)
    {
        var res = await _model.TryLoadEmployeeInfo(urlId);

        if (res)
        {
            IsShowAddInfo = EmployeeInfo!.additionalInformation!.Select(i => i.content != null).ToArray();
            Contacts.Clear();
            EmployeeInfo.jobPositions!.ForEach(p => p.contacts!.ForEach(c => Contacts.Add(c)));
        }
        
        return res;
    }
}
