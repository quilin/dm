namespace DM.Services.UserServices.Implementation
{
    public interface ISaltFactory
    {
        string Create(int saltLength);
    }
}