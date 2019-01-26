using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DM.Services.DataAccess.BusinessObjects.Games.Characters
{
    [Table("CharacterAttributes")]
    public class CharacterAttribute
    {
        [Key]
        public Guid CharacterAttributeId { get; set; }

        public Guid AttributeId { get; set; }
        public Guid CharacterId { get; set; }
        public string Value { get; set; }

        [ForeignKey(nameof(CharacterId))]
        public virtual Character Character { get; set; }
    }
}