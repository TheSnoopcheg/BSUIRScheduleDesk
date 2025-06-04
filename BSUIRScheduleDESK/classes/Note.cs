using System;

namespace BSUIRScheduleDESK.Classes
{
    public class Note
    {
        public Note(DateTime? Date, string? Content)
        {
            if(Date == null)
            {
                this.Date = DateTime.MinValue;
            }
            else
            {
                this.Date = Date;
            }
            this.Content = Content;
        }
        public DateTime? Date { get; set; }
        public string? Content { get; set; }
    }
}