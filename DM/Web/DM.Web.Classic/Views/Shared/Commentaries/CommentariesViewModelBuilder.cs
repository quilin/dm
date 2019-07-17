using System;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<CommentariesViewModel> Build(Guid entityId, int? entityNumber)
        {
            var (comments, paging) = commentaryReadingService.Get(entityId, new PagingQuery
            {
                Number = entityNumber,
                Size = identity.Settings.CommentsPerPage
            }).Result;
            var commentaryTasks = comments.Select(commentaryViewModelBuilder.Build).ToArray();
            await Task.WhenAll(commentaryTasks);
            return new CommentariesViewModel
            {
                TotalPagesCount = paging.TotalPagesCount,
                CurrentPage = paging.CurrentPage,
                PageSize = paging.PageSize,
                EntityNumber = paging.EntityNumber,
                Commentaries = commentaryTasks.Select(c => c.Result).ToArray(),
                CanCreate = intentionsManager.IsAllowed(TopicIntention.CreateComment, paging).Result,
                CreateCommentaryForm = createCommentaryFormBuilder.Build(entityId)
            };
        }

        public async Task<CommentaryViewModel[]> BuildList(Guid entityId, int? entityNumber)
        {
            var (comments, _) = await commentaryReadingService.Get(entityId, new PagingQuery
            {
                Number = entityNumber,
                Size = identity.Settings.CommentsPerPage,
            });
            var commentaryTasks = comments.Select(commentaryViewModelBuilder.Build).ToArray();
            await Task.WhenAll(commentaryTasks);
            return commentaryTasks.Select(c => c.Result).ToArray();
        }
    }
}