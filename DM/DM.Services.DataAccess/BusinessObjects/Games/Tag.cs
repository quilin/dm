using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DM.Services.DataAccess.BusinessObjects.Games
{
    [Table("Tags")]
    public class Tag
    {
        [Key]
        public Guid TagId { get; set; }

        public Guid TagGroupId { get; set; }

        public string Title { get; set; }

        [ForeignKey(nameof(TagGroupId))]
        public TagGroup TagGroup { get; set; }

        [InverseProperty(nameof(GameTag.Tag))]
        public ICollection<GameTag> GameTags { get; set; }
    }
}