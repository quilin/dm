using System;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.Post.SinglePost;
using DM.Web.Classic.Views.Room;
using DM.Web.Classic.Views.RoomChat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.PostControllers
{
    public class PostController : DmControllerBase
    {
        private readonly IPostService postService;
        private readonly IRoomService roomService;
        private readonly IModuleService moduleService;
        private readonly IIntentionsManager intentionsManager;
        private readonly IRoomViewModelBuilder roomViewModelBuilder;
        private readonly IChatRoomViewModelBuilder chatRoomViewModelBuilder;
        private readonly ISinglePostViewModelBuilder singlePostViewModelBuilder;
        private readonly IUserProvider userProvider;
        private readonly IUnreadCounterService unreadCounterService;

        public PostController(
            IPostService postService,
            IRoomService roomService,
            IModuleService moduleService,
            IIntentionsManager intentionsManager,
            IRoomViewModelBuilder roomViewModelBuilder,
            IChatRoomViewModelBuilder chatRoomViewModelBuilder,
            ISinglePostViewModelBuilder singlePostViewModelBuilder,
            IUserProvider userProvider,
            IUnreadCounterService unreadCounterService
        )
        {
            this.postService = postService;
            this.roomService = roomService;
            this.moduleService = moduleService;
            this.intentionsManager = intentionsManager;
            this.roomViewModelBuilder = roomViewModelBuilder;
            this.chatRoomViewModelBuilder = chatRoomViewModelBuilder;
            this.singlePostViewModelBuilder = singlePostViewModelBuilder;
            this.userProvider = userProvider;
            this.unreadCounterService = unreadCounterService;
        }

        public ActionResult LastUnread(string roomIdEncoded)
        {
            roomIdEncoded.TryDecodeFromReadableGuid(out var roomId);
            var room = roomService.ReadRoom(roomId);

            var entityNumber = 1;
            if (room.RoomType == RoomType.Default)
            {
                var totalCount = postService.Count(roomId);
                if (totalCount > 0)
                {
                    var unreadCount = unreadCounterService.GetCounter(roomId, EntryType.Message);
                    entityNumber = Math.Min(totalCount - unreadCount + 1, totalCount);
                }
            }

            return RedirectToAction("Index", new RouteValueDictionary
            {
                {"roomId", roomIdEncoded},
                {"entityNumber", entityNumber}
            });
        }

        public ActionResult Index(Guid roomId, int entityNumber)
        {
            var room = roomService.ReadRoom(roomId);
            if (room.RoomType == RoomType.Chat)
            {
                var chatRoomViewModel = chatRoomViewModelBuilder.Build(room);
                return View("~/Views/RoomChat/RoomChat.cshtml", chatRoomViewModel);
            }

            if (!Request.IsAjaxRequest())
            {
                var roomViewModel = roomViewModelBuilder.Build(room, entityNumber);
                return View("~/Views/Room/Room.cshtml", roomViewModel);
            }

            var posts = roomViewModelBuilder.BuildList(room, entityNumber);
            return PartialView("~/Views/Room/PostsList.cshtml", posts);
        }

        public ActionResult OlderChatEntries(Guid roomId, int startIndex)
        {
            var room = roomService.ReadRoom(roomId);
            var chatRoomViewModel = chatRoomViewModelBuilder.Build(room, startIndex, 20);
            if (chatRoomViewModel.Posts.Length == 0)
            {
                return new EmptyResult();
            }

            return View("~/Views/RoomChat/RoomChatMessages.cshtml", chatRoomViewModel);
        }

        public ActionResult NewestChatEntries(Guid roomId, long fromDate = 0)
        {
            var room = roomService.ReadRoom(roomId);
            var chatRoomViewModel = chatRoomViewModelBuilder.Build(room, fromDate);
            if (chatRoomViewModel.Posts.Length == 0)
            {
                return new EmptyResult();
            }

            return View("~/Views/RoomChat/RoomChatMessages.cshtml", chatRoomViewModel);
        }

        public ActionResult SinglePost(Guid postId)
        {
            var post = postService.Read(postId);
            var room = roomService.ReadRoom(post.RoomId);
            var module = moduleService.Read(room.ModuleId);
            if (!intentionsManager.IsAllowed(RoomIntention.ViewPosts, room, module))
            {
                return View("SinglePost/SinglePost", singlePostViewModelBuilder.Build(post, module));
            }

            return RedirectToAction("Index", new RouteValueDictionary
            {
                {"roomId", post.RoomId.EncodeToReadable(room.Title)},
                {"entityNumber", postService.GetPostNumber(post) + 1}
            });
        }

        public int Delete(Guid postId)
        {
            var post = postService.Remove(postId);
            return PagingHelper.GetTotalPages(postService.Count(post.RoomId),
                userProvider.CurrentSettings.PostsPerPage);
        }

        public ActionResult GetFirstUnreadPostInModule(Guid moduleId)
        {
            var rooms = roomService.SelectVisibleRooms(moduleId);
            if (!rooms.Any())
            {
                var module = moduleService.Read(moduleId);
                return RedirectToAction("Index", "Module", new RouteValueDictionary
                {
                    {"moduleId", moduleId.EncodeToReadable(module.Title)}
                });
            }

            var firstUnreadData = unreadCounterService
                .GetFirstUnreadData(rooms.Select(r => r.RoomId), EntryType.Message);
            if (firstUnreadData.TryGetValue(moduleId, out var unreadData))
            {
                var room = rooms.First(r => r.RoomId == unreadData.EntityId);
                return RedirectToAction("LastUnread", new RouteValueDictionary
                {
                    {"roomIdEncoded", room.RoomId.EncodeToReadable(room.Title)}
                });
            }

            var firstRoom = rooms.First();
            return RedirectToAction("LastUnread", new RouteValueDictionary
            {
                {"roomIdEncoded", firstRoom.RoomId.EncodeToReadable(firstRoom.Title)}
            });
        }
    }
}