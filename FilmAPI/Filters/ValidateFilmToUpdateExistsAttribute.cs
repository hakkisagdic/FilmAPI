﻿using FilmAPI.Core.Interfaces;
using FilmAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmAPI.Filters
{
    public class ValidateFilmToUpdateExistsAttribute : TypeFilterAttribute
    {
        public ValidateFilmToUpdateExistsAttribute() : base(typeof(ValidateFilmToUpdateExistsFilterImpl))
        {
        }
        private class ValidateFilmToUpdateExistsFilterImpl : IAsyncActionFilter
        {
            private readonly IFilmRepository _repoitory;
            public ValidateFilmToUpdateExistsFilterImpl(IFilmRepository repo)
            {
                _repoitory = repo;
            }
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("model"))
                {
                    var model = (FilmViewModel)context.ActionArguments["model"];
                    var f = _repoitory.GetByTitleAndYear(model.Title, model.Year);
                    if (f == null)
                    {
                        context.Result = new NotFoundObjectResult(model.Title);
                        return;
                    }
                }
                await next();
            }
        }
    }
}
