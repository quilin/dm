namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public class CommentariesViewModel
    {
        public CommentaryViewModel[] Commentaries { get; set; }

        public int TotalPagesCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int EntityNumber { get; set; }

        public bool CanCreate { get; set; }
        public CreateCommentaryForm CreateCommentaryForm { get; set; }
    }
}