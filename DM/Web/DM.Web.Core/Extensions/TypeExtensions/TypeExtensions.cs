using System;

namespace DM.Web.Core.Extensions.TypeExtensions
{
    public static class TypeExtensions
    {
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