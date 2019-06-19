using System;
using DM.Web.Classic.Views.Room.EditPost;

namespace DM.Web.Classic.Dto
{
    public class UpdatePostModelConverter : IUpdatePostModelConverter
    {
        public UpdatePostModel Convert(EditPostForm form)
        {
            return new UpdatePostModel
            {
                PostId = form.PostId,
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