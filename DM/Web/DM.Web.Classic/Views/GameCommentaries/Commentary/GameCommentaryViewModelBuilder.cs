using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.Dto;
using DM.Services.Core.Parsing;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.GameCommentaries.Commentary
{
    public class GameCommentaryViewModelBuilder : IGameCommentaryViewModelBuilder
    {
        private readonly IBbParserProvider bbParserProvider;
        private readonly IIntentionManager intentionManager;
        private readonly IUserViewModelBuilder userViewModelBuilder;
        private readonly IIdentity identity;

        public GameCommentaryViewModelBuilder(
            IBbParserProvider bbParserProvider,
            IIntentionManager intentionManager,
            IUserViewModelBuilder userViewModelBuilder,
            IIdentityProvider identityProvider)
        {
            this.bbParserProvider = bbParserProvider;
            this.intentionManager = intentionManager;
            this.userViewModelBuilder = userViewModelBuilder;
            identity = identityProvider.Current;
        }

        public async Task<GameCommentaryViewModel> Build(Comment comment, Game game,
            IDictionary<Guid, IEnumerable<string>> characterNames)
        {
            return new GameCommentaryViewModel
            {
                CommentaryId = comment.Id,
                Author = userViewModelBuilder.Build(comment.Author),
                CharacterNames = characterNames.TryGetValue(comment.Author.UserId, out var names)
                    ? names
                    : new string[0],
                CreateDate = comment.CreateDate.DateTime,
                LastUpdateDate = comment.LastUpdateDate?.DateTime,
                Text = bbParserProvider.CurrentCommon.Parse(comment.Text).ToHtml(),
                LikesCount = comment.Likes.Count(),

                CanEdit = intentionManager.IsAllowed(CommentIntention.Edit, (comment, game)),
                CanRemove = intentionManager.IsAllowed(CommentIntention.Delete, (comment, game)),

                CanLike = intentionManager.IsAllowed(CommentIntention.Like, comment),
                HasLiked = comment.Likes.Any(l => l.UserId == identity.User.UserId),

                CanWarn = intentionManager.IsAllowed(GameIntention.CreateComment, game)
            };
        }
    }
}