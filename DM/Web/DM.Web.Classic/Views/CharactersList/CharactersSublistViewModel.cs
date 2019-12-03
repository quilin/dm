using System.Linq;
using DM.Web.Classic.Views.CharactersList.Character;

namespace DM.Web.Classic.Views.CharactersList
{
    public class CharactersSublistViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public CharacterViewModel[] Characters { get; set; }
        public bool IsDefault { get; set; }

        public bool IsEmpty => !Characters.Any();
    }
}