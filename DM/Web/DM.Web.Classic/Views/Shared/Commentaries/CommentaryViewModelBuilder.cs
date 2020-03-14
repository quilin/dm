using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.Dto;
using DM.Services.Core.Parsing;
using DM.Services.Forum.Authorization;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public class CommentaryViewModelBuilder : ICommentaryViewModelBuilder
    {
        private readonly IIntentionManager intentionsManager;
        private readonly IBbParserProvider bbParserProvider;
        private readonly IUserViewModelBuilder userViewModelBuilder;
        private readonly IIdentityProvider identityProvider;

        public CommentaryViewModelBuilder(
            IIntentionManager intentionsManager,
            IBbParserProvider bbParserProvider,
            IUserViewModelBuilder userViewModelBuilder,
            IIdentityProvider identityProvider)
        {
            this.intentionsManager = intentionsManager;
            this.bbParserProvider = bbParserProvider;
            this.userViewModelBuilder = userViewModelBuilder;
            this.identityProvider = identityProvider;
        }

        public async Task<CommentaryViewModel> Build(Comment comment)
        {
            return new CommentaryViewModel
            {
                CommentaryId = comment.Id,
                EntityId = comment.EntityId,
                CreateDate = comment.CreateDate,
                LastUpdateDate = comment.LastUpdateDate,
                Author = userViewModelBuilder.Build(comment.Author),
                Text = bbParserProvider.CurrentCommon.Parse(comment.Text).ToHtml(),
                LikesCount = comment.Likes.Count(),

                CanEdit = intentionsManager.IsAllowed(CommentIntention.Edit, comment),
                CanRemove = intentionsManager.IsAllowed(CommentIntention.Delete, comment),

                CanLike = intentionsManager.IsAllowed(CommentIntention.Like, comment),
                HasLiked = comment.Likes.Any(l => l.UserId == identityProvider.Current.User.UserId),

//                           CanWarn = intentionsManager.IsAllowed(WarningIntention.Create, userService.Read(comment.UserId)),
//                           WarningsList = warningsListViewModelBuilder.Build(comment.CommentId)
            };
        }
    }
}