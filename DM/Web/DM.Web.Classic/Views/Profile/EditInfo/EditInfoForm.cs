using System;
using BBCodeParser;

namespace DM.Web.Classic.Views.Profile.EditInfo
{
    public class EditInfoForm
    {
        public Guid UserId { get; set; }

        public string Info { get; set; }

        public IBbParser Parser { get; set; }
    }
}