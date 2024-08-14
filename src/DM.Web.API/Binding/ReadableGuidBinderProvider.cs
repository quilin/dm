using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DM.Web.API.Binding;

/// <inheritdoc />
internal class ReadableGuidBinderProvider : IModelBinderProvider
{
    /// <inheritdoc />
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(Guid) ||
            context.Metadata.ModelType == typeof(Guid?))
        {
            return new ReadableGuidBinder();
        }

        return null;
    }
}