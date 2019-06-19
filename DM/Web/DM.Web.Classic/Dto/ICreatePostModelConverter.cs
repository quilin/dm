using DM.Web.Classic.Views.Room.CreatePost;

namespace DM.Web.Classic.Dto
{
    public interface ICreatePostModelConverter
    {
        CreatePostModel Convert(CreatePostForm form);
    }
}