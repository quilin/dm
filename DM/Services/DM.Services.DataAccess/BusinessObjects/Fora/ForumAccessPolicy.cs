namespace DM.Services.DataAccess.BusinessObjects.Fora
{
    public enum ForumAccessPolicy
    {
        NoOne = 0,
        Administrators = 1 << 0,
        SeniorModerators = 1 << 1,
        RegularModerators = 1 << 2,
        NurseModerators = 1 << 3,
        ForumModerators = 1 << 4,
        Players = 1 << 5,
        Everyone = 1 << 6
    }
}