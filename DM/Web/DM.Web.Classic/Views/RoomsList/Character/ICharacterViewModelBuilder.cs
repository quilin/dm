namespace DM.Web.Classic.Views.RoomsList.Character
{
    public interface ICharacterViewModelBuilder
    {
        CharacterViewModel Build(Services.Gaming.Dto.Output.Character character);
    }
}