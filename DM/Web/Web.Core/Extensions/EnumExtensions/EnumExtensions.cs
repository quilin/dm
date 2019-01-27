using System;
using System.ComponentModel;
using System.Linq;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace Web.Core.Extensions.EnumExtensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }
            var field = type.GetField(name);
            if (field == null)
            {
                return null;
            }
            var attr = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attr?.Description;
        }

        public static string GetUserRoleDescription(this UserRole value)
        {
            return string.Join(", ",
                Enum.GetValues(typeof(UserRole))
                    .Cast<UserRole>()
                    .Where(role => role != UserRole.Guest && role != UserRole.Player)
                    .Where(role => value.HasFlag(role))
                    .Select(role => role.GetDescription()));
        }
    }
}