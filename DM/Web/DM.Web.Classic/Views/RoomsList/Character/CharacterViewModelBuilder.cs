namespace DM.Web.Classic.Views.RoomsList.Character
{
    public class CharacterViewModelBuilder : ICharacterViewModelBuilder
    {
        public CharacterViewModel Build(Services.Gaming.Dto.Output.Character character)
        {
            return new CharacterViewModel
                {
                    CharacterId = character.Id,
                    Login = character.Author.Login,
                    Name = character.Name
                };
        }
    }
}