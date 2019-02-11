using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.Core.Extensions.EnumExtensions
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

        public static IEnumerable<string> GetUserRoleDescription(this UserRole value)
        {
            return Enum.GetValues(typeof(UserRole))
                .Cast<UserRole>()
                .Where(role => role != UserRole.Guest && role != UserRole.Player)
                .Where(role => value.HasFlag(role))
                .Select(role => role.GetDescription());
        }
    }
}