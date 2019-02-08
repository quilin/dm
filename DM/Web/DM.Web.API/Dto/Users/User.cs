using System;
using System.Collections.Generic;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Web.API.Dto.Users
{
    public class User
    {
        public string Login { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string ProfilePictureUrl { get; set; }
        public Rating Rating { get; set; }
        public DateTime? Online { get; set; }
    }

    public class Rating
    {
        public Rating(IUser user)
        {
            Enabled = !user.RatingDisabled;
            Quality = user.QualityRating;
            Quantity = user.QuantityRating;
        }
        
        public bool Enabled { get; set; }
        public int Quality { get; set; }
        public int Quantity { get; set; }
    }
}