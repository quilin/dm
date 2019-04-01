using System;

namespace DM.Web.Core.Extensions.TypeExtensions
{
    /// <summary>
    /// Extensions for type
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Check if the type is nullable
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Is nullable</returns>
        public static bool AllowsNullValue(this Type type)
        {
            return !type.IsValueType || IsNullableValueType(type);
        }

        private static bool IsNullableValueType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}