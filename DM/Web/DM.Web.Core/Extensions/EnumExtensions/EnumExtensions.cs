using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.Core.Extensions.EnumExtensions
{
    /// <summary>
    /// Enum values display extensions
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get enum description from its annotations
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns>Annotation description</returns>
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

        /// <summary>
        /// Get list of separate roles from composite role
        /// </summary>
        /// <param name="value">Composite role</param>
        /// <returns>List of roles</returns>
        public static IEnumerable<string> GetUserRoles(this UserRole value)
        {
            return Enum.GetValues(typeof(UserRole))
                .Cast<UserRole>()
                .Where(role => role != UserRole.Guest && role != UserRole.Player)
                .Where(role => value.HasFlag(role))
                .Select(role => role.ToString());
        }
    }
}