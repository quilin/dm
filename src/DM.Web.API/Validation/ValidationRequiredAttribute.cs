using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DM.Web.API.Validation;

/// <summary>
/// Validation required attribute
/// </summary>
internal class ValidationRequiredAttribute : TypeFilterAttribute
{
    /// <inheritdoc />
    public ValidationRequiredAttribute() : base(typeof(ValidationRequiredFilter))
    {
    }

    private class ValidationRequiredFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;
            var failures = context.ModelState
                .Where(s => s.Value.ValidationState == ModelValidationState.Invalid)
                .SelectMany(s => s.Value.Errors.Select(e => new ValidationFailure(s.Key, e.ErrorMessage)));
            throw new ValidationException(failures);
        }
    }
}