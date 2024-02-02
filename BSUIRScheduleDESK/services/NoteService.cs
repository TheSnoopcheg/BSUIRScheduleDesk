using BSUIRScheduleDESK.classes;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.services
{
    public static class NoteService
    {
        private const string PATH = @"\data\";
        private const string SUFFIX = "_notes";
        public static async Task<ObservableCollection<Note>> LoadNotes(string? path)
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
        public static async Task SaveNotes(ObservableCollection<Note> notes, string? path)
        {
            using(FileStream createStream = File.Create($"{Directory.GetCurrentDirectory() + PATH + path + SUFFIX}.json"))
            {
                await JsonSerializer.SerializeAsync<ObservableCollection<Note>>(createStream, notes);
                await createStream.DisposeAsync();
            }
        }
    }
}
