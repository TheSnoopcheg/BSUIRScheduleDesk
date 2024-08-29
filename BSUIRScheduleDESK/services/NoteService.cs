using BSUIRScheduleDESK.classes;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.services
{
    public class NoteService : INoteService
    {
        private const string PATH = @"\data\";
        private const string SUFFIX = "_notes";
        public async Task<ObservableCollection<Note>> LoadNotesAsync(string? path)
        {
            ObservableCollection<Note>? notes = new ObservableCollection<Note>();
            try
            {
                using(FileStream openStream = File.OpenRead($"{Directory.GetCurrentDirectory() + PATH + path + SUFFIX}.json"))
                {
                    notes = await JsonSerializer.DeserializeAsync<ObservableCollection<Note>>(openStream);
                    await openStream.DisposeAsync();
                }
            }
            catch(System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return notes!;
        }
        public async Task SaveNotesAsync(ObservableCollection<Note> notes, string? path)
        {
            using(FileStream createStream = File.Create($"{Directory.GetCurrentDirectory() + PATH + path + SUFFIX}.json"))
            {
                await JsonSerializer.SerializeAsync<ObservableCollection<Note>>(createStream, notes);
                await createStream.DisposeAsync();
            }
        }
    }
}
