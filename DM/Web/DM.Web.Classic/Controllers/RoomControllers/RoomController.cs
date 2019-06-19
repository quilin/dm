using System;
using DM.Web.Classic.Views.Room.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.RoomControllers
{
    public class RoomController : DmControllerBase
    {
        private readonly IRoomService roomService;
        private readonly IPostWaitNotificationService postWaitNotificationService;
        private readonly IUserService userService;
        private readonly IUserProvider userProvider;
        private readonly IPostWaitNotificationViewModelBuilder notificationBuilder;

        public RoomController(
            IRoomService roomService,
            IPostWaitNotificationService postWaitNotificationService,
            IUserService userService,
            IUserProvider userProvider,
            IPostWaitNotificationViewModelBuilder notificationBuilder)
        {
            this.roomService = roomService;
            this.postWaitNotificationService = postWaitNotificationService;
            this.userService = userService;
            this.userProvider = userProvider;
            this.notificationBuilder = notificationBuilder;
        }

        public void Remove(Guid roomId)
        {
            roomService.Remove(roomId);
        }

        [HttpPost]
        public void InsertFirst(Guid movedRoomId)
        {
            roomService.InsertFirst(movedRoomId);
        }

        [HttpPost]
        public void InsertAfter(Guid movedRoomId, Guid roomId)
        {
            roomService.InsertAfter(movedRoomId, roomId);
        }

        public ActionResult GetNotifications(Guid locationId)
        {
            var notificationViewModel = notificationBuilder.Build(locationId, userProvider.Current.UserId, true);
            return View("~/Views/Room/NotificationsList.cshtml", notificationViewModel);
        }

        [HttpPost]
        public ActionResult CancelNotification(string login, Guid roomId)
        {
            var user = userService.Find(login);
            postWaitNotificationService.Remove(user.UserId, roomId);
            return GetNotifications(roomId);
        }
    }
}