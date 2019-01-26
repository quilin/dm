using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Games.Links;

namespace DM.Services.DataAccess.BusinessObjects.Common
{
    [Table("Tags")]
    public class Tag
    {
        [Key]
        public Guid TagId { get; set; }

        public Guid TagGroupId { get; set; }

        public string Title { get; set; }

        [ForeignKey(nameof(TagGroupId))]
        public virtual TagGroup TagGroup { get; set; }

        [InverseProperty(nameof(GameTag.Tag))]
        public virtual ICollection<GameTag> GameTags { get; set; }
    }
}