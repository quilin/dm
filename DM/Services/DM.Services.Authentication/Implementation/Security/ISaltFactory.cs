namespace DM.Services.Authentication.Implementation.Security
{
    public interface ISaltFactory
    {
        string Create(int saltLength);
    }
}