using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DM.Web.Core.Binders
{
    public class ReadableGuidBinderProvider : IModelBinderProvider
    {
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
}