using System;
using System.Linq;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;

namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public class CommentariesViewModelBuilder : ICommentariesViewModelBuilder
    {
        private readonly ICommentaryReadingService commentaryReadingService;
        private readonly ICommentaryViewModelBuilder commentaryViewModelBuilder;
        private readonly IIntentionManager intentionsManager;
        private readonly ICreateCommentaryFormBuilder createCommentaryFormBuilder;
        private readonly IIdentity identity;

        public CommentariesViewModelBuilder(
            ICommentaryReadingService commentaryReadingService,
            ICommentaryViewModelBuilder commentaryViewModelBuilder,
            IIntentionManager intentionsManager,
            ICreateCommentaryFormBuilder createCommentaryFormBuilder,
            IIdentityProvider identityProvider)
        {
            this.commentaryReadingService = commentaryReadingService;
            this.commentaryViewModelBuilder = commentaryViewModelBuilder;
            this.intentionsManager = intentionsManager;
            this.createCommentaryFormBuilder = createCommentaryFormBuilder;
            identity = identityProvider.Current;
        }

        public CommentariesViewModel Build(Guid entityId, int? entityNumber)
        {
            var (comments, paging) = commentaryReadingService.GetCommentsList(entityId, new PagingQuery
            {
                Number = entityNumber,
                Size = identity.Settings.CommentsPerPage
            }).Result;
            return new CommentariesViewModel
                       {
                           TotalPagesCount = paging.TotalPagesCount,
                           CurrentPage = paging.CurrentPage,
                           PageSize = paging.PageSize,
                           EntityNumber = paging.EntityNumber,
                           Commentaries = comments.Select(commentaryViewModelBuilder.Build).ToArray(),
                           CanCreate = intentionsManager.IsAllowed(TopicIntention.CreateComment, paging).Result,
                           CreateCommentaryForm = createCommentaryFormBuilder.Build(entityId)
                       };
        }

        public CommentaryViewModel[] BuildList(Guid entityId, int? entityNumber)
        {
            var (comments, _) = commentaryReadingService.GetCommentsList(entityId, new PagingQuery
            {
                Number = entityNumber,
                Size = identity.Settings.CommentsPerPage,
            }).Result;
            return comments.Select(commentaryViewModelBuilder.Build).ToArray();
        }
    }
}