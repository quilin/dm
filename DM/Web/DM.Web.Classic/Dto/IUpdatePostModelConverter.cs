using DM.Web.Classic.Views.Room.EditPost;

namespace DM.Web.Classic.Dto
{
    public interface IUpdatePostModelConverter
    {
        UpdatePostModel Convert(EditPostForm form);
    }
}