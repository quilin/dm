using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.DataAccess.BusinessObjects.Fora
{
    public class ForumTopicsListItem
    {
        public Guid Id { get; set; }
        public Guid ForumId { get; set; }
        public string ForumTitle { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }

        public Guid AuthorUserId { get; set; }
        public string AuthorLogin { get; set; }
        public UserRole AuthorRole { get; set; }
        public AccessPolicy AuthorAccessPolicy { get; set; }
        public DateTime? AuthorLastVisitDate { get; set; }
        public string AuthorProfilePictureUrl { get; set; }
        public bool AuthorRatingDisabled { get; set; }
        public int AuthorQualityRating { get; set; }
        public int AuthorQuantityRating { get; set; }

        public bool Attached { get; set; }
        public bool Closed { get; set; }

        public int TotalCommentsCount { get; set; }
        public int UnreadCommentsCount { get; set; }

        public Guid LastCommentAuthorUserId { get; set; }
        public string LastCommentAuthorLogin { get; set; }
        public UserRole LastCommentAuthorRole { get; set; }
        public AccessPolicy LastCommentAuthorAccessPolicy { get; set; }
        public DateTime? LastCommentAuthorLastVisitDate { get; set; }
        public string LastCommentAuthorProfilePictureUrl { get; set; }
        public bool LastCommentAuthorRatingDisabled { get; set; }
        public int LastCommentAuthorQualityRating { get; set; }
        public int LastCommentAuthorQuantityRating { get; set; }
        public DateTime LastCommentDate { get; set; }
    }
}