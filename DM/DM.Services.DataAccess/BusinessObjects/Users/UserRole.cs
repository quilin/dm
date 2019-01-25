namespace DM.Services.DataAccess.BusinessObjects.Users
{
    public enum UserRole
    {
        Guest = 0,

        Player = 1 << 0,

        Administrator = 1 << 1,

        NurseModerator = 1 << 2,

        RegularModerator = 1 << 3,

        SeniorModerator = 1 << 4
    }
}