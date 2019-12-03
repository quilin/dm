namespace DM.Web.Classic.Views.CreateGame
{
    public class CreateGameFormBuilder : ICreateGameFormBuilder
    {
        public CreateGameForm Build()
        {
            return new CreateGameForm
            {
                CreateAsRegistration = true
            };
        }
    }
}