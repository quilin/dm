using System;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.GameRemove
{
    public class GameRemoveViewModel
    {
        public Guid GameId { get; set; }
        public UserViewModel Master { get; set; }
        public bool CanRemove { get; set; }
    }
}