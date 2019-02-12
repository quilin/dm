using System;
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

        public static string List(Guid forumId, PagingData paging, bool attached, TopicsSortType sortType)
        {
            var queryFormatBuilder = new StringBuilder(
                @"select
                    topic.""ForumTopicId"" as ""Id"",
                    topic.""CreateDate"",
                    topic.""Title"",
                    topic.""Text"",
                    topic.""Attached"",
                    topic.""Closed"",

                    forum.""ForumId"" as ""ForumId"",
                    forum.""ForumTitle"" as ""ForumTitle"",

                    topicAuthor.""UserId"" as ""Author.UserId"",
                    topicAuthor.""Login"" as ""Author.Login"",
                    topicAuthor.""Role"" as ""Author.Role"",
                    topicAuthor.""AccessPolicy"" as ""Author.AccessPolicy"",
                    topicAuthor.""LastVisitDate"" as ""Author.LastVisitDate"",
                    topicAuthor.""RatingDisabled"" as ""Author.RatingDisabled"",
                    topicAuthor.""QualityRating"" as ""Author.QualityRating"",
                    topicAuthor.""QuantityRating"" as ""Author.QuantityRating"",

                    commentAuthor.""UserId"" as ""LastCommentAuthor.UserId"",
                    commentAuthor.""Login"" as ""LastCommentAuthor.Login"",
                    commentAuthor.""Role"" as ""LastCommentAuthor.Role"",
                    commentAuthor.""AccessPolicy"" as ""LastCommentAuthor.AccessPolicy"",
                    commentAuthor.""LastVisitDate"" as ""LastCommentAuthor.LastVisitDate"",
                    commentAuthor.""RatingDisabled"" as ""LastCommentAuthor.RatingDisabled"",
                    commentAuthor.""QualityRating"" as ""LastCommentAuthor.QualityRating"",
                    commentAuthor.""QuantityRating"" as ""LastCommentAuthor.QuantityRating"",
                    comment.""CreateDate"" as ""LastCommentDate"",

                    count(c.*) over(
                        partition by topic.""ForumTopicId""
                        where comment.""IsRemoved"" = false
                        order by comment.""CreateDate"" asc) as ""TotalCommentsCount"",
                    row_number() over(
                        partition by topic.""ForumTopicId""
                        where comment.""IsRemoved"" = false
                        order by comment.""CreateDate"" desc) as commentRow

                    from ""ForumTopics"" as topic 
                    inner join ""Fora"" as forum on topic.""ForumId"" = forum.""ForumId""

                    left join ""Comments"" as comment on topic.""ForumTopicId"" = comment.""EntityId""

                    inner join ""Users"" as topicAuthor on topic.""UserId"" = topicAuthor.""UserId""
                    inner join ""Users"" as commentAuthor on comment.""UserId"" = commentAuthor.""UserId""

                    inner join (select * from ""Uploads"" where ""IsRemoved"" = false) as topicAuthorPicture
                        on topicAuthor.""UserId"" = topicAuthorPicture.""EntityId""
                    inner join (select * from ""Uploads"" where ""IsRemoved"" = false) as commentAuthorPicture
                        on commentAuthor.""UserId"" = commentAuthorPicture.""EntityId""
                    where
                        topic.""ForumId"" = {0} and topic.""Attached"" = {1} and topic.""IsRemoved"" = false and
                        commentRow = 1
                    order by");

            if (!attached)
            {
                switch (sortType)
                {
                    case TopicsSortType.Default:
                        queryFormatBuilder.Append(@" comment.""CreateDate"" desc, ");
                        break;
                    case TopicsSortType.Errors:
                        queryFormatBuilder.Append(@" topic.""Closed"" desc, comment.""CreateDate"" desc, ");
                        break;
                }
            }

            queryFormatBuilder.Append(@"topic.""CreateDate""");

            if (paging != null)
            {
                queryFormatBuilder.AppendLine(@"limit {2} offset {3}");
            }

            return queryFormatBuilder.ToString();
        }
    }
}