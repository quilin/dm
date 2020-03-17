using System;

namespace DM.Web.Classic.Views.Profile.EditInfo
{
    public interface IEditInfoFormFactory
    {
        EditInfoForm Create(Guid userId, string info);
    }
}