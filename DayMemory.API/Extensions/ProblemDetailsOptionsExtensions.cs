﻿using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace DayMemory.Web.Components.Extensions
{
    public static class ProblemDetailsOptionsExtensions
    {
        public static void MapFluentValidationException(this Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options)
        {
            options.Map<ValidationException>((ctx, ex) =>
            {
                var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                var errors = ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(x => x.ErrorMessage).ToArray());

                return factory.CreateValidationProblemDetails(ctx, errors);
            });
        }
    }
}