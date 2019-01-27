namespace DM.Services.Authentication.Implementation
{
    public interface ISaltFactory
    {
        string Create(int saltLength);
    }
}