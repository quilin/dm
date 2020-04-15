using System;

namespace DM.Web.Classic.Views.Polls
{
    public class PollOptionViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool Voted { get; set; }
        public int VotesCount { get; set; }
    }
}