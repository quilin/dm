using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.LegacyControllers
{
    public class LegacyController : DmControllerBase
    {
        private readonly ILegacyService legacyService;
        private readonly IModuleService moduleService;
        private readonly IRoomService roomService;
        private readonly IForumService forumService;

        public LegacyController(
            ILegacyService legacyService,
            IModuleService moduleService,
            IRoomService roomService,
            IForumService forumService
            )
        {
            this.legacyService = legacyService;
            this.moduleService = moduleService;
            this.roomService = roomService;
            this.forumService = forumService;
        }

        public IActionResult Home()
        {
            return RedirectToActionPermanent("Index", "Home");
        }

        public IActionResult Module(int module)
        {
            var moduleId = legacyService.GetModernId(module, LegacyEntityType.Modules);
            var moduleTitle = moduleService.Read(moduleId).Title;
            return RedirectToActionPermanent("Index", "Module",
                new RouteValueDictionary {{"moduleId", moduleId.EncodeToReadable(moduleTitle)}});
        }

        public IActionResult Room(int module, int room)
        {
            var roomId = legacyService.GetModernId(room, LegacyEntityType.Rooms);
            var roomTitle = roomService.ReadRoom(roomId).Title;
            return RedirectPermanent($"session/{roomId.EncodeToReadable(roomTitle)}");
        }

        public IActionResult OutOfSession(int module)
        {
            var moduleId = legacyService.GetModernId(module, LegacyEntityType.Modules);
            var moduleTitle = moduleService.Read(moduleId).Title;
            return RedirectToActionPermanent("LastUnread", "ModuleCommentaries",
                new RouteValueDictionary {{"moduleIdEncoded", moduleId.EncodeToReadable(moduleTitle)}});
        }

        public IActionResult Characters(int module)
        {
            var moduleId = legacyService.GetModernId(module, LegacyEntityType.Modules);
            var moduleTitle = moduleService.Read(moduleId).Title;
            return RedirectToActionPermanent("Index", "CharactersList",
                new RouteValueDictionary {{"moduleId", moduleId.EncodeToReadable(moduleTitle)}});
        }

        public IActionResult Comments(int contentId)
        {
            var topicId = legacyService.GetModernId(contentId, LegacyEntityType.News);
            var topicTitle = forumService.ReadTopic(topicId).Title;
            return RedirectToActionPermanent("Index", "Topic",
                new RouteValueDictionary {{"topicIdEncoded", topicId.EncodeToReadable(topicTitle)}});
        }

        public IActionResult Chat()
        {
            return RedirectToActionPermanent("Index", "Chat");
        }

        public IActionResult Cabinet(string user)
        {
            return RedirectToActionPermanent("Index", "Profile", new RouteValueDictionary{{"login", user}});
        }
    }
}