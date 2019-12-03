using System;

namespace DM.Web.Classic.Views.CharactersList
{
    public class CharactersListViewModel
    {
        public Guid GameId { get; set; }
        public Guid? CharacterId { get; set; }
        public string GameTitle { get; set; }
        public CharactersSublistViewModel[] CharacterLists { get; set; }
    }
}