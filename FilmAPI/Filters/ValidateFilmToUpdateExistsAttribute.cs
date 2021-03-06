﻿using FilmAPI.Common.DTOs;
using FilmAPI.Core.Interfaces;
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
        public ValidateFilmToUpdateExistsAttribute(): base(typeof(ValidateFilmToUpdateExistsFilterImpl))
       {
        }
        private class ValidateFilmToUpdateExistsFilterImpl : IAsyncActionFilter
        {
            private readonly IFilmRepository _repository;
            public ValidateFilmToUpdateExistsFilterImpl(IFilmRepository repo)
            {
                _repository = repo;
            }
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("model"))
                {
                    var model = (BaseFilmDto)context.ActionArguments["model"];
                    var f = _repository.GetByTitleAndYear(model.Title, model.Year).value;
                    if (f == null)
                    {
                        context.Result = new NotFoundObjectResult(model);
                        return;
                    }
                }
                await next();
            }
        }
    }
}
