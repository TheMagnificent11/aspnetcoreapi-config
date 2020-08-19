﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreApi.Infrastructure.Mediation;
using FluentValidation;
using FluentValidation.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCoreApi.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception Filter
    /// </summary>
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        /// <summary>
        /// Handles exceptions thrown in API
        /// </summary>
        /// <param name="context">Exception contect</param>
        /// <returns>Asynchronous task</returns>
        public virtual Task OnExceptionAsync(ExceptionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context?.Exception == null) return Task.CompletedTask;

            switch (context.Exception)
            {
                case ValidationException validationException:
                    context.Result = HandleValidationException(validationException);
                    break;

                case UnauthorizedAccessException _:
                    context.Result = new ForbidResult();
                    break;
            }

            return Task.CompletedTask;
        }

        private static IActionResult HandleValidationException(ValidationException exception)
        {
            var errors = exception.Errors
                .ToList()
                .GetErrors();

            var operationResult = OperationResult.Fail(errors);

            return new BadRequestObjectResult(operationResult.ToProblemDetails());
        }
    }
}
