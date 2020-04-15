using System.Collections.Generic;
using DM.Services.Core.Dto;

namespace DM.Web.Classic.Views.Polls
{
    public class PollsViewModel
    {
        public IEnumerable<PollViewModel> Polls { get; set; }
        public PagingResult Paging { get; set; }
        public bool CanCreate { get; set; }
    }
}