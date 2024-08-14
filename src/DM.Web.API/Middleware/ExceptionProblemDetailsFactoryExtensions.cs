using System;
using DM.Services.Core.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DM.Web.API.Middleware;

internal static class ExceptionProblemDetailsFactoryExtensions
{
    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory,
        HttpException httpException, HttpContext httpContext) =>
        factory.CreateProblemDetails(httpContext, (int)httpException.StatusCode, httpException.Message);

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory,
        HttpBadRequestException httpBadRequestException, HttpContext httpContext)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var (key, value) in httpBadRequestException.ValidationErrors)
        {
            modelStateDictionary.AddModelError(key, value);
        }

        return factory.CreateValidationProblemDetails(httpContext,
            modelStateDictionary, StatusCodes.Status400BadRequest);
    }

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory,
        ValidationException validationException, HttpContext httpContext)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in validationException.Errors)
        {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return factory.CreateValidationProblemDetails(httpContext,
            modelStateDictionary, StatusCodes.Status400BadRequest, "Validation failed");
    }

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory,
        NotImplementedException notImplementedException, HttpContext httpContext) =>
        factory.CreateProblemDetails(httpContext,
            StatusCodes.Status501NotImplemented,
            notImplementedException.Message);

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory,
        Exception exception, HttpContext httpContext, Guid correlationId) =>
        factory.CreateProblemDetails(httpContext, StatusCodes.Status500InternalServerError, exception.Message,
            detail: $"Server error. Address the administration for technical support. Use the following token to help us identify your issue: {correlationId}");
}