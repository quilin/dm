using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;
using DM.Web.Classic.Controllers;
using DM.Web.Classic.Controllers.CharacterControllers;
using DM.Web.Classic.Controllers.ChatControllers;
using DM.Web.Classic.Controllers.CommentariesControllers;
using DM.Web.Classic.Controllers.CommunityControllers;
using DM.Web.Classic.Controllers.ConversationControllers;
using DM.Web.Classic.Controllers.ForumControllers;
using DM.Web.Classic.Controllers.LegacyControllers;
using DM.Web.Classic.Controllers.ModuleCommentariesControllers;
using DM.Web.Classic.Controllers.ModuleControllers;
using DM.Web.Classic.Controllers.PollControllers;
using DM.Web.Classic.Controllers.PostControllers;
using DM.Web.Classic.Controllers.ProfileControllers;
using DM.Web.Classic.Controllers.RatingControllers;
using DM.Web.Classic.Controllers.RoomControllers;
using DM.Web.Classic.Views.ModuleSettings;
using DM.Web.Core.Extensions.RouteExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Configuration
{
    public static class RouteConfig
    {
        private static readonly IDictionary<string, object> PagingDefaults = new Dictionary<string, object>
        {
            ["entityNumber"] = 1
        };

        public static void Register(IRouteBuilder routes)
        {
            #region ModuleRoutes
            routes.MapAction<ModuleController>("game/{moduleId}", c => c.Index(Guid.Empty));

            routes.MapAction<CreateModuleController>("creategame", c => c.Create());

            routes.MapAction<ModuleSettingsController>(
                "editgame/{settingsType}/{moduleId}",
                c => c.Index(Guid.Empty, ModuleSettingsType.General),
                new Dictionary<string, object>{{"settingsType", ModuleSettingsType.General}});

            routes.MapAction<ModuleController>("observe/{moduleId}", c => c.Observe(Guid.Empty));
            routes.MapAction<ModuleController>("stopobserve/{moduleId}", c => c.StopObserving(Guid.Empty));

            routes.MapAction<ModuleCommentariesController>("outofsession/{moduleIdEncoded}", c => c.LastUnread(null));
            routes.MapAction<ModuleCommentariesController>("outofsession/{moduleId}/{entityNumber}", c => c.Index(Guid.Empty, 0));

            routes.MapAction<ModulesListController>("tags/{tagId}/{entityNumber}", c => c.Tags(Guid.Empty, 0), PagingDefaults);
            routes.MapAction<ModulesListController>("games/{status}/{entityNumber}", c => c.Index(GameStatus.Active, 0), PagingDefaults);

            #endregion

            #region CharacterRoutes
            routes.MapAction<CharactersListController>("charslist/{moduleId}/{characterId}", c => c.Index(null, null), new Dictionary<string, object>{{"characterId", null}});

            routes.MapAction<CreateCharacterController>("chargen/{moduleId}", c => c.Create(Guid.Empty));

            routes.MapAction<EditCharacterController>("acceptchar/{characterId}",
                c => c.ChangeStatus(Guid.Empty, CharacterIntention.Accept),
                new Dictionary<string, object> {{"characterIntention", CharacterIntention.Accept}});
            routes.MapAction<EditCharacterController>("declinechar/{characterId}",
                c => c.ChangeStatus(Guid.Empty, CharacterIntention.Decline),
                new Dictionary<string, object> {{"characterIntention", CharacterIntention.Decline}});
            routes.MapAction<EditCharacterController>("killchar/{characterId}",
                c => c.ChangeStatus(Guid.Empty, CharacterIntention.Kill),
                new Dictionary<string, object> {{"characterIntention", CharacterIntention.Kill}});
            routes.MapAction<EditCharacterController>("resurrectchar/{characterId}",
                c => c.ChangeStatus(Guid.Empty, CharacterIntention.Resurrect),
                new Dictionary<string, object> {{"characterIntention", CharacterIntention.Resurrect}});
            routes.MapAction<EditCharacterController>("leavegame/{characterId}",
                c => c.ChangeStatus(Guid.Empty, CharacterIntention.Leave),
                new Dictionary<string, object> {{"characterIntention", CharacterIntention.Leave}});
            routes.MapAction<CharacterController>("deletechar/{characterId}", c => c.Remove(Guid.Empty));
            routes.MapAction<EditCharacterController>("editchar/{characterId}", c => c.Edit(Guid.Empty));
            #endregion

            #region RoomRoutes
            routes.MapAction<PostController>("session/{roomIdEncoded}", c => c.LastUnread(null));
            routes.MapAction<PostController>("session/{roomId}/{entityNumber}", c => c.Index(Guid.Empty, 0));
            routes.MapAction<PostController>("singlepost/{postId}", c => c.SinglePost(Guid.Empty));
            routes.MapAction<PostController>("topicalmodulepost/{moduleId}", c => c.GetFirstUnreadPostInModule(Guid.Empty));

            routes.MapAction<RoomController>("cancelpostnotification/{login}/{roomId}", c => c.CancelNotification(null, Guid.Empty));
            #endregion

            #region CommentariesRoutes
            routes.MapAction<CommentariesController>("comments/{entityId}/{entityNumber}", c => c.Index(Guid.Empty, 0));
            #endregion


            #region ProfileRoutes
            routes.MapAction<ProfileController>("profile/{login}", c => c.Index(null), new Dictionary<string, object>{{"login", null}});

            routes.MapAction<ProfileController>("profile/{login}/games", c => c.GetModules(null));
            routes.MapAction<ProfileController>("profile/{login}/characters", c => c.GetCharacters(null));
            routes.MapAction<ProfileController>("profile/{login}/settings", c => c.GetUserSettings());
            routes.MapAction<ProfileController>("profile/closesession/{userId}", c => c.CloseSessions(Guid.Empty));

            routes.MapAction<ProfilePictureUploadController>("upload/profilepicture/{userId}/{uploadId}", c => c.Upload(null, Guid.Empty, Guid.Empty));
            routes.MapAction<ProfilePictureUploadController>("upload/profilepicture/progress", c => c.Progress(Guid.Empty));
            #endregion

            #region MessageRoutes
            routes.MapAction<ConversationsController>("dialogues/{entityNumber}", c => c.List(0), PagingDefaults);
            routes.MapAction<ConversationsController>("messages/{login}", c => c.LastUnread(null));
            routes.MapAction<ConversationsController>("messages/{login}/{entityNumber}", c => c.Index(null, 0));
            #endregion

            #region RatingRoutes
            routes.MapAction<VotesController>("votes/{login}", c => c.UserVotes(null));
            routes.MapAction<VotesController>("rating/{login}", c => c.UserRating(null));
            routes.MapAction<RatingController>("voteform/{postId}/{voteSign}", c => c.Vote(Guid.Empty, VoteSign.Neutral));
            #endregion

            #region ForumRoutes
            routes.MapAction<ForumController>("fora", c => c.List());
            routes.MapAction<ForumController>("forum/{forumTitle}/{entityNumber}", c => c.Index(null, 0, null), new Dictionary<string, object>{{"forumTitle", null}, {"entityNumber", 1}});
            routes.MapAction<ForumController>("readallmessages", c => c.MarkAllAsRead(Guid.Empty));

            routes.MapAction<EditTopicController>("topic/{topicId}/edit", c => c.Edit(Guid.Empty));
            routes.MapAction<TopicController>("topic/{topicIdEncoded}", c => c.LastUnread(null));
            routes.MapAction<TopicController>("topic/{topicId}/{entityNumber}", c => c.Index(Guid.Empty, 0));
            routes.MapAction<TopicController>("closetopic", c => c.CloseTopic(Guid.Empty));
            routes.MapAction<TopicController>("reopentopic", c => c.OpenTopic(Guid.Empty));
            routes.MapAction<TopicController>("attachtopic", c => c.AttachTopic(Guid.Empty));
            routes.MapAction<TopicController>("detachtopic", c => c.DetachTopic(Guid.Empty));
            routes.MapAction<TopicController>("removetopic", c => c.RemoveTopic(Guid.Empty));
            routes.MapAction<TopicController>("movetopic", c => c.MoveTopic(Guid.Empty, Guid.Empty));
            #endregion

            #region CommunityRoutes
            routes.MapAction<CommunityController>("community/{entityNumber}", c => c.Index(0, false), new Dictionary<string, object>{{"entityNumber", 1}, {"withInactive", false}});
            #endregion

            #region AccountRoutes
            routes.MapAction<AccountController>("activateaccount/{tokenId}", c => c.UpdatePassword(Guid.Empty));
            routes.MapAction<AccountController>("restorepassword/{tokenId}", c => c.UpdatePassword(Guid.Empty));
            routes.MapAction<AccountController>("loginas/{userId}", c => c.LogInAs(Guid.Empty));
            #endregion

            routes.MapAction<ChatController>("fm", c => c.Index());

            routes.MapAction<ErrorController>("error/{statusCode}", c => c.Index(0, null));

            routes.MapAction<HomeController>("about", c => c.About());
            routes.MapAction<HomeController>("rules", c => c.Rules());
            routes.MapAction<HomeController>("donate", c => c.Donate());

            routes.MapAction<PollController>("createpoll", c => c.Create());
            routes.MapAction<PollController>("votepoll", c => c.Vote(Guid.Empty, Guid.Empty));

            routes.MapAction<ApiController>("api", c => c.Index());

            #region LegacyRoutes

            routes.MapAction<LegacyController>("Default.aspx", c => c.Home());
            routes.MapAction<LegacyController>("ModuleInfo.aspx", c => c.Module(0));
            routes.MapAction<LegacyController>("Session.aspx", c => c.Room(0, 0));
            routes.MapAction<LegacyController>("OutOfSession.aspx", c => c.OutOfSession(0));
            routes.MapAction<LegacyController>("PlayerProfiles.aspx", c => c.Characters(0));
            routes.MapAction<LegacyController>("Comments.aspx", c => c.Comments(0));
            routes.MapAction<LegacyController>("Chat", c => c.Chat());
            routes.MapAction<LegacyController>("Chat/Default.aspx", c => c.Chat());
            routes.MapAction<LegacyController>("Cabinet", c => c.Cabinet(null));
            routes.MapAction<LegacyController>("Cabinet/Default.aspx", c => c.Cabinet(null));

            #endregion

            routes.MapRoute(
                name: "default_route",
                template: "{controller}/{action}/{id?}",
                defaults: new {controller = "Home", action = "Index"});
        }
    }
}