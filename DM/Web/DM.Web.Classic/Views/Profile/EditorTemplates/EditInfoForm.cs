using BBCodeParser;

namespace DM.Web.Classic.Views.Profile.EditorTemplates
{
    public class EditInfoForm
    {
        public string Login { get; set; }

        public string Info { get; set; }

        public IBbParser Parser { get; set; }
    }
}