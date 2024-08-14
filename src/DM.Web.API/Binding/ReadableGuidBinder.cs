using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DM.Web.API.Binding;

/// <inheritdoc />
internal class ReadableGuidBinder : IModelBinder
{
    /// <inheritdoc />
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var key = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(key);
        var attemptedValue = valueProviderResult.FirstValue;

        if (bindingContext.ModelType == typeof(Guid?) && string.IsNullOrEmpty(attemptedValue))
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        if (bindingContext.ModelType == typeof(Optional<>))
        {
            if (attemptedValue == null)
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            if (string.IsNullOrEmpty(attemptedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(Optional<Guid>.WithValue(null));
                return Task.CompletedTask;
            }
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