using System;

namespace DM.Web.Classic.Views.Polls
{
    public class PollViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsClosed { get; set; }
        public bool IsVoted { get; set; }
        
        public int TotalVotesCount { get; set; }
        public int MaxVotesCount { get; set; }
        public PollOptionViewModel[] Options { get; set; }
    }
}