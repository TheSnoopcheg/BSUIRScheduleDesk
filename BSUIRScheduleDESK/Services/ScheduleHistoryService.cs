using BSUIRScheduleDESK.Classes;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Services;

public class ScheduleHistoryService : IScheduleHistoryService
{
    private const string PATH = @"\data\";
    private const string SUFFIX = "_history";

    public async Task<ObservableCollection<HistoryNote>?> LoadHistoryAsync(string? path)
    {
        ObservableCollection<HistoryNote>? notes = new ObservableCollection<HistoryNote>();
        try
        {
            using (FileStream openStream = File.OpenRead($"{Directory.GetCurrentDirectory() + PATH + path + SUFFIX}.json"))
            {
                notes = await JsonSerializer.DeserializeAsync<ObservableCollection<HistoryNote>>(openStream);
                await openStream.DisposeAsync();
            }
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }
        return notes;
    }
    public async Task SaveHistoryAsync(ObservableCollection<HistoryNote> history, string? path)
    {
        using (FileStream createStream = File.Create($"{Directory.GetCurrentDirectory() + PATH + path + SUFFIX}.json"))
        {
            await JsonSerializer.SerializeAsync<ObservableCollection<HistoryNote>>(createStream, history);
            await createStream.DisposeAsync();
        }
    }
}
