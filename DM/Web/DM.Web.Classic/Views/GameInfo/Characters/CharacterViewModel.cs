using System;
using DM.Services.Core.Dto.Enums;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.GameInfo.Characters
{
    public class CharacterViewModel
    {
        public Guid CharacterId { get; set; }
        public UserViewModel User { get; set; }

        public string Name { get; set; }
        public string Class { get; set; }
        public string Race { get; set; }

        public CharacterStatus Status { get; set; }
        public int Number { get; set; }
    }
}