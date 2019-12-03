using System;
using BBCodeParser;

namespace DM.Web.Classic.Views.GameNotepad
{
    public class GameNotepadForm
    {
        public Guid ModuleId { get; set; }

        public string NotepadHtml { get; set; }

        public string Notepad { get; set; }

        public IBbParser Parser { get; set; }
    }
}