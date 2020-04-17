using System;

namespace DM.Web.Classic.Views.GameActions
{
    public class GameActionsViewModel
    {
        public Guid GameId { get; set; }
        public string GameTitle { get; set; }

        public bool CanReadCommentaries { get; set; }
        public bool CanCreateCharacter { get; set; }
        public bool CanCreateNpc { get; set; }
        public bool CanObserve { get; set; }
        public bool CanStopObserving { get; set; }
        public bool CanChangeStatus { get; set; }
        public bool CanTakeOnPremoderation { get; set; }
        public bool CanEditInfo { get; set; }

        public int UnreadCommentariesCount { get; set; }
        public int UnreadCharactersCount { get; set; }

        public PageType PageType { get; set; }
        public Guid? PageId { get; set; }
    }
}