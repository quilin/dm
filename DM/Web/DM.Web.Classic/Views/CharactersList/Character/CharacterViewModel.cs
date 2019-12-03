using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.CharactersList.Character.CharacterActions;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.CharactersList.Character
{
    public class CharacterViewModel
    {
        public Guid CharacterId { get; set; }
        public UserViewModel Author { get; set; }
        public CharacterStatus Status { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Class { get; set; }
        public Alignment? Alignment { get; set; }

        public IEnumerable<CharacterAttribute> Attributes { get; set; }

        public string Appearance { get; set; }
        public string Temper { get; set; }
        public string Skills { get; set; }
        public string Inventory { get; set; }
        public string Story { get; set; }

        public bool DisplayTemper { get; set; }
        public bool DisplaySkills { get; set; }
        public bool DisplayInventory { get; set; }
        public bool DisplayStory { get; set; }
        public bool DisplayAlignment { get; set; }

        public bool TemperHidden { get; set; }
        public bool SkillsHidden { get; set; }
        public bool InventoryHidden { get; set; }
        public bool StoryHidden { get; set; }

        public string PictureUrl { get; set; }
        public CharacterActionsViewModel CharacterActions { get; set; }
    }
}