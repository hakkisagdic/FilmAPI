﻿using AutoMapper;
using FilmAPI.Core.Entities;
using FilmAPI.Core.Interfaces;
using FilmAPI.Core.SharedKernel;
using FilmAPI.Interfaces;
using FilmAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmAPI.Services
{
    public class FilmService : EntityService<Film, FilmViewModel>, IFilmService
    {
        public FilmService(IFilmRepository repo, IMapper mapper, IKeyService keyService) : base(mapper, keyService)
        {
            _repository = repo;
        }
        public override Film CreateEntity(string key)
        {
            var data = GetData(key);
            if (data.title == FilmConstants.BADKEY)
            {
                return null;
            }
            return new Film(data.title, data.year);
        }

        private (string title, short year) GetData(string key)
        {
            return _keyService.DeconstructFilmSurrogateKey(key);
        }

        public override FilmViewModel EntityToModel(Film e)
        {
            return new FilmViewModel(e);
        }

        public override Film GetEntity(string key)
        {
            var data = GetData(key);
            if (data.title == FilmConstants.BADKEY)
            {
                return null;
            }
            return ((IFilmRepository)_repository).GetByTitleAndYear(data.title, data.year);
        }

        public override Film ModelToEntity(FilmViewModel m)
        {
            return _mapper.Map<Film>(m);
        }

        public override FilmViewModel AddForce(FilmViewModel m)
        {
            return Add(m);
        }

        public override Film FetchEntity(string key)
        {
            var data = GetData(key);
            var storedFilm = ((IFilmRepository)_repository).GetByTitleAndYear(data.title, data.year);
            return _repository.GetById(storedFilm.Id);
        }

        public override void CopyModelOntoEntity(Film e, FilmViewModel m)
        {
            e.Title = m.Title;
            e.Year = m.Year;
            e.Length = m.Length;
        }
    }
}
