using System;
using System.Linq;
using System.Reflection;

namespace DM.Services.Core.Extensions;

/// <summary>
/// Attribute helpers
/// </summary>
public static class AttributeExtensions
{
    /// <summary>
    /// Get single attribute of given type
    /// </summary>
    /// <param name="structValue"></param>
    /// <typeparam name="TStruct"></typeparam>
    /// <typeparam name="TAttr"></typeparam>
    /// <returns></returns>
    /// <exception cref="AmbiguousAttributesException"></exception>
    public static TAttr GetAttribute<TStruct, TAttr>(this TStruct structValue)
        where TStruct : struct
        where TAttr : Attribute
    {
        var attributes = GetAllAttributes<TStruct, TAttr>(structValue);
        if (attributes.Length > 1)
            throw new AmbiguousAttributesException(typeof(TAttr));
        if (attributes.Length == 1)
            return attributes[0];
        throw new AttributeNotFoundException(typeof(TAttr), typeof(TStruct));
    }

    /// <summary>
    /// Get all attributes of given type
    /// </summary>
    /// <param name="structValue"></param>
    /// <typeparam name="TStruct"></typeparam>
    /// <typeparam name="TAttr"></typeparam>
    /// <returns></returns>
    public static TAttr[] GetAllAttributes<TStruct, TAttr>(this TStruct structValue)
        where TStruct : struct
        where TAttr : Attribute
    {
        var type = typeof(TStruct);
        var isFlag = type.GetCustomAttribute<FlagsAttribute>() != null;
        var attributes = new TAttr[0];
        foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
        {
            var fieldValue = field.GetValue(null);
            if (fieldValue.Equals(structValue))
                return (TAttr[])field.GetCustomAttributes(typeof(TAttr), false);
            if (isFlag && type.IsEnum)
            {
                // This feels like a black magic
                var enumValue = (object) structValue;
                if (((int) fieldValue & (int) enumValue) != 0)
                {
                    attributes = attributes.Concat((TAttr[])field.GetCustomAttributes(typeof(TAttr), false)).ToArray();
                }
            }
        }
        return attributes;
    }
}

/// <summary>
/// More than one attribute found
/// </summary>
internal class AmbiguousAttributesException : Exception
{
    /// <inheritdoc />
    public AmbiguousAttributesException(Type attrType)
        : base($"Ambiguous attributes of type {attrType.FullName} found.")
    {
    }
}

/// <summary>
/// No attribute found
/// </summary>
public class AttributeNotFoundException : Exception
{
    /// <inheritdoc />
    public AttributeNotFoundException(Type attrType, Type enumType)
        : base($"Attribute of type {attrType.FullName} not found at enum {enumType.FullName}.")
    {
    }

    /// <inheritdoc />
    public AttributeNotFoundException(Type attrType)
        : base($"Attribute of type {attrType.FullName} not found")
    {
    }
}