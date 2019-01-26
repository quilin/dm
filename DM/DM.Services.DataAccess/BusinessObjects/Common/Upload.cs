using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DM.Services.DataAccess.BusinessObjects.Common
{
    [Table("Uploads")]
    public class Upload
    {
        [Key]
        public Guid UploadId { get; set; }
        
        // todo: complete uploads
    }
}