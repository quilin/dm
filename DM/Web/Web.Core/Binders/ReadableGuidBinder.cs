using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.Core.Extensions.TypeExtensions;
using Web.Core.Helpers;

namespace Web.Core.Binders
{
    public class ReadableGuidBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var key = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(key);
            var attemptedValue = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(attemptedValue) && bindingContext.ModelType.AllowsNullValue())
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            if (string.IsNullOrEmpty(attemptedValue))
            {
                bindingContext.ModelState.AddModelError(key, $"Field {key} should contain value");
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            if (Guid.TryParse(attemptedValue, out var parsedValue) ||
                attemptedValue.TryDecodeFromReadableGuid(out parsedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(parsedValue);
                return Task.CompletedTask;
            }
            
            bindingContext.ModelState.AddModelError(key, $"Invalid value for field {key}");
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }
    }
}