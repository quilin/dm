using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Administration
{
    [Table("Reports")]
    public class Report
    {
        [Key] public Guid ReportId { get; set; }

        public Guid UserId { get; set; }
        public Guid TargetId { get; set; }

        public DateTime CreateDate { get; set; }
        public string ReportText { get; set; }
        public string Comment { get; set; }

        public Guid? AnswerAuthorId { get; set; }
        public string Answer { get; set; }

        [ForeignKey(nameof(UserId))] public virtual User Author { get; set; }

        [ForeignKey(nameof(TargetId))] public virtual User Target { get; set; }

        [ForeignKey(nameof(AnswerAuthorId))] public virtual User AnswerAuthor { get; set; }
    }
}