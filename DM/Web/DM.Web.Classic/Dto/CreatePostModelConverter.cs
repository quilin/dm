using System;
using DM.Web.Classic.Views.Room.CreatePost;

namespace DM.Web.Classic.Dto
{
    public class CreatePostModelConverter : ICreatePostModelConverter
    {
        public CreatePostModel Convert(CreatePostForm form)
        {
            return new CreatePostModel
            {
                RoomId = form.RoomId,
                CharacterId = form.CharacterId,
                Text = form.Text,
                Comment = form.Commentary,
                MasterMessage = form.MasterMessage,
                DiceIds = form.DiceIds ?? new Guid[0],
                WaitingForUsersIds = form.NotifyUserIds ?? new Guid[0]
            };
        }
    }
}