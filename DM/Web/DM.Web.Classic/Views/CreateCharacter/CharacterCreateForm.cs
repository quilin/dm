using System;
using System.ComponentModel.DataAnnotations;
using BBCodeParser;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.Classic.Views.CreateCharacter
{
    public class CharacterCreateForm
    {
        public Guid GameId { get; set; }

        [Required(ErrorMessage = "Введите имя персонажа")]
        [StringLength(50, ErrorMessage = "Имя не должно быть длиннее 50 символов")]
        public string Name { get; set; }

        [StringLength(30, ErrorMessage = "Название расы не должно быть длиннее 30 символов")]
        public string Race { get; set; }

        [StringLength(30, ErrorMessage = "Название класса не должно быть длиннее 30 символов")]
        public string Class { get; set; }

        public Alignment Alignment { get; set; }

        public string Appearance { get; set; }

        public string Temper { get; set; }

        public string Skills { get; set; }

        public string Inventory { get; set; }

        public string Story { get; set; }

        // TODO: attributes
        public bool IsNpc { get; set; }
        public bool MasterEditAllowed { get; set; }
        public bool MasterPostsEditAllowed { get; set; }
        public bool HasMasterAccess { get; set; }
        public bool DisplayAlignment { get; set; }
        public Guid? PictureUploadRootId { get; set; }

        public bool TemperHidden { get; set; }
        public bool StoryHidden { get; set; }
        public bool SkillsHidden { get; set; }
        public bool InventoryHidden { get; set; }

        public IBbParser Parser { get; set; }
    }
}