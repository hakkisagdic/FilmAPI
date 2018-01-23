﻿using FilmAPI.Core.Entities;
using FilmAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.Interfaces;
using FilmAPI.Core.Interfaces;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Constants;
using FluentValidation.Results;
using FluentValidation;
using FilmAPI.Common.Services;
using FilmAPI.Validation.Interfaces;
using FilmAPI.Common.Utilities;

namespace FilmAPI.Services
{
    public class FilmService : BaseSevice<Film>, IFilmService
    {
        private readonly IFilmValidator _validator;
        public FilmService(IFilmRepository repo,
                           IFilmMapper mapper,
                           IFilmValidator validator) : base(repo, mapper)
        {
            _validator = validator;
        }
        public IKeyedDto Result()
        {
            return (KeyedFilmDto)result;
        }
        public override OperationStatus Add(IBaseDto dto)
        {
            var retVal = OperationStatus.OK;
            var b = (BaseFilmDto)dto;
            var results = _validator.Validate(b);
            IsValid = results.IsValid;
            
            var entityToAdd = _mapper.MapBack(b);
            var savedEntity = _repository.Add(entityToAdd);
            if (IsValid)
            {
                result = ExtractKeyedDto(b);
                retVal = OperationStatus.OK;
            }
            else
            {
                result = null;
                retVal = OperationStatus.BadRequest;
            }
            return retVal;
        }

        public override OperationStatus Delete(string key)
        {
            var result = OperationStatus.OK;
            var filmToDelete = ((IFilmRepository)_repository).GetByKey(key);
            if (filmToDelete == null)
            {
                result = OperationStatus.NotFound;
            }
            else
            {
                _repository.Delete(filmToDelete);
            }
            return result;
        }

        public override OperationStatus Update(IBaseDto dto)
        {
            var result = OperationStatus.OK;
            var b = (BaseFilmDto)dto;
            if (b == null)
            {
                result = OperationStatus.BadRequest;
            }
            var filmToUpdate = _mapper.MapBack(b);
            var storedFilm = RetrieveStoredEntity(dto);
            if (storedFilm == null)
            {
                result = OperationStatus.NotFound;
            }
            else
            {
                _repository.Update(filmToUpdate);
            }
            return result;
        }

        protected override IKeyedDto ExtractKeyedDto(IBaseDto dto)
        {
            var b = (BaseFilmDto)dto;
            var key = _keyService.ConstructFilmKey(b.Title, b.Year);
            var result = new KeyedFilmDto(b.Title, b.Year, b.Length, key);
            return (IKeyedDto)result;
        }

        protected override Film RetrieveStoredEntity(IBaseDto dto)
        {
            var b = (BaseFilmDto)dto;
            return ((IFilmRepository)_repository).GetByTitleAndYear(b.Title, b.Year);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        private bool LocalEquals(FilmService that)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override OperationStatus GetByKey(string key)
        {
            var data = _keyService.DeconstructFilmKey(key);
            _getResults[key] = new KeyedFilmDto(data.title, data.year);
            _getResults[key].Key = key;
            return OperationStatus.OK;
        }
    }
}
