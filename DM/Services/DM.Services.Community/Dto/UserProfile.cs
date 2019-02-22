using System;

namespace DM.Services.Community.Dto
{
    public class UserProfile
    {
        public Guid UserId { get; set; }

        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Icq { get; set; }
        public string Skype { get; set; }
        public string Email { get; set; }
        public string Info { get; set; }
    }
}