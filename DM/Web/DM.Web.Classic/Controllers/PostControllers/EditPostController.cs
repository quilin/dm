using System;
using System.Collections.Generic;
using DM.Web.Classic.Dto;
using DM.Web.Classic.Views.Room.EditPost;
using DM.Web.Classic.Views.Room.Post;
using DM.Web.Classic.Views.Room.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.PostControllers
{
    public class EditPostController : DmControllerBase
    {
        private readonly IPostService postService;
        private readonly IPostViewModelBuilder postViewModelBuilder;
        private readonly IRoomService roomService;
        private readonly IModuleService moduleService;
        private readonly ICharacterService characterService;
        private readonly IIntentionsManager intentionsManager;
        private readonly ICompositePostFormViewModelBuilder compositePostFormViewModelBuilder;
        private readonly IUpdatePostModelConverter updatePostModelConverter;
        private readonly IDiceService diceService;

        public EditPostController(
            IPostService postService,
            IPostViewModelBuilder postViewModelBuilder,
            IRoomService roomService,
            IModuleService moduleService,
            ICharacterService characterService,
            IIntentionsManager intentionsManager,
            ICompositePostFormViewModelBuilder compositePostFormViewModelBuilder,
            IUpdatePostModelConverter updatePostModelConverter,
            IDiceService diceService
            )
        {
            this.postService = postService;
            this.postViewModelBuilder = postViewModelBuilder;
            this.roomService = roomService;
            this.moduleService = moduleService;
            this.characterService = characterService;
            this.intentionsManager = intentionsManager;
            this.compositePostFormViewModelBuilder = compositePostFormViewModelBuilder;
            this.updatePostModelConverter = updatePostModelConverter;
            this.diceService = diceService;
        }

        [HttpGet]
        public ActionResult Edit(Guid postId)
        {
            var post = postService.Read(postId);
            var room = roomService.ReadRoom(post.RoomId);
            var module = moduleService.Read(room.ModuleId);
            if (post.CharacterId == Guid.Empty)
            {
                intentionsManager.ThrowIfForbidden(PostIntention.Edit, post, module);
            }
            else
            {
                var character = characterService.Read(post.CharacterId);
                intentionsManager.ThrowIfForbidden(PostIntention.Edit, post, module, character);
            }

            var roomCharacterIds = roomService.GetRoomCharacters(post.RoomId);
            var characters = characterService.Select(roomCharacterIds)
                                             .Where(c => c.Status == CharacterStatus.Active)
                                             .ToArray();
            var editPostViewModel = compositePostFormViewModelBuilder.BuildForEdit(post, characters, module);

            return View("~/Views/Room/EditPost/EditPost.cshtml", editPostViewModel);
        }

        [HttpPost, ValidationRequired]
        public ActionResult Edit(EditPostForm editForm)
        {
            var number = postService.GetPostNumber(editForm.PostId, out var post);
            var updatePostModel = updatePostModelConverter.Convert(editForm);
            postService.Update(updatePostModel);
            var room = roomService.ReadRoom(post.RoomId);
            var module = moduleService.Read(room.ModuleId);
            var dice = diceService.Select(post, module);

            var postViewModel = postViewModelBuilder.Build(post, room, module, new Dictionary<Guid, DieResult[]> {{post.PostId, dice}}, number + 1);
            return View("~/Views/Room/Post/Post.cshtml", postViewModel);
        }
    }
}