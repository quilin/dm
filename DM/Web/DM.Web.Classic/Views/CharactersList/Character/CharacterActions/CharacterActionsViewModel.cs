using System;

namespace DM.Web.Classic.Views.CharactersList.Character.CharacterActions
{
    public class CharacterActionsViewModel
    {
        public Guid CharacterId { get; set; }

        public bool CanAccept { get; set; }
        public bool CanDecline { get; set; }
        public bool CanKill { get; set; }
        public bool CanResurrect { get; set; }
        public bool CanRemove { get; set; }
        public bool CanEdit { get; set; }
        public bool CanLeave { get; set; }
        public bool CanReturn { get; set; }
        public bool HasMasterAccess { get; set; }
    }
}