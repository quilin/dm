using System.Text;
using DM.Services.Core.Dto;

namespace DM.Services.Forum.Repositories
{
    public static class TopicQueries
    {
        public enum TopicsSortType
        {
            Default = 0,
            News = 1,
            Errors = 2,
        }

        public static string List(PagingData paging, bool attached, TopicsSortType sortType)
        {
            var queryFormatBuilder = new StringBuilder(@"select
	res.""Id"",
	res.""CreateDate"",
	res.""Title"",
	res.""Text"",
	res.""Attached"",
	res.""Closed"",

	res.""ForumId"",
	res.""ForumTitle"",

	res.""AuthorUserId"",
	res.""AuthorLogin"",
	res.""AuthorRole"",
	res.""AuthorAccessPolicy"",
	res.""AuthorLastVisitDate"",
	res.""AuthorRatingDisabled"",
	res.""AuthorQualityRating"",
	res.""AuthorQuantityRating"",
	res.""AuthorProfilePictureUrl"",

	0 as ""UnreadCommentsCount"",
	res.""TotalCommentsCount"",
	res.""LastCommentDate"",

	res.""LastCommentAuthorUserId"",
	res.""LastCommentAuthorLogin"",
	res.""LastCommentAuthorRole"",
	res.""LastCommentAuthorAccessPolicy"",
	res.""LastCommentAuthorLastVisitDate"",
	res.""LastCommentAuthorRatingDisabled"",
	res.""LastCommentAuthorQualityRating"",
	res.""LastCommentAuthorQuantityRating"",
	res.""LastCommentAuthorProfilePictureUrl""
from (
select
	topic.""ForumTopicId"" as ""Id"",
	topic.""CreateDate"",
	topic.""Title"",
	topic.""Text"",
	topic.""Attached"",
	topic.""Closed"",

	forum.""ForumId"",
	forum.""Title"" as ""ForumTitle"",

	topicAuthor.""UserId"" as ""AuthorUserId"",
	topicAuthor.""Login"" as ""AuthorLogin"",
	topicAuthor.""Role"" as ""AuthorRole"",
	topicAuthor.""AccessPolicy"" as ""AuthorAccessPolicy"",
	topicAuthor.""LastVisitDate"" as ""AuthorLastVisitDate"",
	topicAuthor.""RatingDisabled"" as ""AuthorRatingDisabled"",
	topicAuthor.""QualityRating"" as ""AuthorQualityRating"",
	topicAuthor.""QuantityRating"" as ""AuthorQuantityRating"",
	topicAuthorPicture.""VirtualPath"" as ""AuthorProfilePictureUrl"",

	count(commentary.*) over(
	  partition by topic.""ForumTopicId""
	  order by commentary.""CreateDate"" asc) as ""TotalCommentsCount"",
	row_number() over(
	  partition by topic.""ForumTopicId""
	  order by commentary.""CreateDate"" desc) as ""RowNumber"",

	commentary.""CreateDate"" as ""LastCommentDate"",
	commentaryAuthor.""UserId"" as ""LastCommentAuthorUserId"",
	commentaryAuthor.""Login"" as ""LastCommentAuthorLogin"",
	commentaryAuthor.""Role"" as ""LastCommentAuthorRole"",
	commentaryAuthor.""AccessPolicy"" as ""LastCommentAuthorAccessPolicy"",
	commentaryAuthor.""LastVisitDate"" as ""LastCommentAuthorLastVisitDate"",
	commentaryAuthor.""RatingDisabled"" as ""LastCommentAuthorRatingDisabled"",
	commentaryAuthor.""QualityRating"" as ""LastCommentAuthorQualityRating"",
	commentaryAuthor.""QuantityRating"" as ""LastCommentAuthorQuantityRating"",
	commentaryAuthorPicture.""VirtualPath"" as ""LastCommentAuthorProfilePictureUrl""
from
	""ForumTopics"" as topic
	inner join ""Fora"" as forum on topic.""ForumId"" = forum.""ForumId""
	
	inner join ""Users"" as topicAuthor on topic.""UserId"" = topicAuthor.""UserId""
	inner join (select u.""VirtualPath"", u.""EntityId"" from ""Uploads"" as u where u.""IsRemoved"" = false) as topicAuthorPicture
		on topicAuthor.""UserId"" = topicAuthorPicture.""EntityId""
	
	left join ""Comments"" as commentary on topic.""ForumTopicId"" = commentary.""EntityId""
	inner join ""Users"" as commentaryAuthor on commentary.""UserId"" = commentaryAuthor.""UserId""
	inner join (select u.""VirtualPath"", u.""EntityId"" from ""Uploads"" as u where u.""IsRemoved"" = false) as commentaryAuthorPicture
		on commentaryAuthor.""UserId"" = commentaryAuthorPicture.""EntityId""
	where
		topic.""ForumId"" = @ForumId and
		topic.""Attached"" = @Attached and topic.""IsRemoved"" = false
	order by");

            if (!attached)
            {
                switch (sortType)
                {
                    case TopicsSortType.Default:
                        queryFormatBuilder.Append(@" commentary.""CreateDate"" desc,");
                        break;
                    case TopicsSortType.Errors:
                        queryFormatBuilder.Append(@" topic.""Closed"" desc, commentary.""CreateDate"" desc,");
                        break;
                }
            }

            queryFormatBuilder.Append(@" topic.""CreateDate"") as res where res.""RowNumber"" = 1");

            if (paging != null)
            {
                queryFormatBuilder.AppendLine($" limit @Page_Size offset @Page_From");
            }

            return queryFormatBuilder.ToString();
        }
    }
}