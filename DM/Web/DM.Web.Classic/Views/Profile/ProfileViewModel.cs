using System;
using DM.Services.Core.Dto.Enums;
using DM.Web.Classic.Views.Profile.Actions;
using DM.Web.Classic.Views.Profile.EditInfo;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Profile
{
    public class ProfileViewModel
    {
        public Guid UserId { get; set; }
        public UserViewModel User { get; set; }
        public bool HasRoles { get; set; }
        public UserRole Role { get; set; }

        public DateTimeOffset RegistrationDate { get; set; }
        public DateTimeOffset? LastVisitDate { get; set; }

        public string ProfilePictureUrl { get; set; }
        public string Status { get; set; }
        public int VotesCount { get; set; }

        public int WarningsCount { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }
        public string Icq { get; set; }
        public string Skype { get; set; }

        public bool ShowEmail { get; set; }
        public string Email { get; set; }

        public string Info { get; set; }
        public EditInfoForm EditInfoForm { get; set; }

        public bool CanEdit { get; set; }
        public bool CanEditSettings { get; set; }

        public ProfileActionsViewModel ProfileActions { get; set; }
    }
}