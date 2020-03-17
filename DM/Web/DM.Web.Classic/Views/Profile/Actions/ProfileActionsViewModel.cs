using System;

namespace DM.Web.Classic.Views.Profile.Actions
{
    public class ProfileActionsViewModel
    {
        public string Login { get; set; }

        public bool CanWriteMessages { get; set; }
        public bool CanReportUser { get; set; }
        public bool CanReport { get; set; }
        public bool CanViewStatistics { get; set; }
        public bool CanEdit { get; set; }
        public bool CanModerate { get; set; }
        public bool CanLogin { get; set; }
        public bool CanInitiateMerge { get; set; }
        public bool CanCompleteMerge { get; set; }
        public Guid UserId { get; set; }

        public string AdministrativeUrl { get; set; }
    }
}