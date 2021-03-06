﻿using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Core.Entities;
using FilmAPI.Core.Interfaces;
using FilmAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmAPI.Mappers
{
    public class FilmPersonMapper : BaseMapper<FilmPerson>, IFilmPersonMapper
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IPersonRepository _personRepository;
        public FilmPersonMapper(IFilmRepository frepo, IPersonRepository prepo)
        {
            _filmRepository = frepo;
            _personRepository = prepo;
        }
        public override IBaseDto Map(FilmPerson t)
        {
            var data = _filmRepository.GetById(t.FilmId);
            var f = data.value;
            if (f == null)
            {
                throw new Exception("Unknown film");
            }
            var pdata = _personRepository.GetById(t.PersonId);
            var p = pdata.value;
            if (p == null)
            {
                throw new Exception("Unknown person");
            }
            return new BaseFilmPersonDto(f.Title, f.Year, p.LastName, p.BirthdateString, t.Role);
        }

        public override FilmPerson MapBack(IBaseDto dto)
        {
            FilmPerson result = null;
            var b = (BaseFilmPersonDto)dto;
            var f = _filmRepository.GetByTitleAndYear(b.Title, b.Year).value;
            if (f == null)
            {
                throw new Exception("Unknown film");
            }
            var p = _personRepository.GetByLastNameAndBirthdate(b.LastName, b.Birthdate).value;
            if (f != null && p != null)
            {
                result = new FilmPerson(f.Id, p.Id, b.Role);
            }
            return result;
        }
    }
}