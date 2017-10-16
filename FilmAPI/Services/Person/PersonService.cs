﻿using FilmAPI.Interfaces.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.DTOs.Person;
using FilmAPI.Core.Interfaces;
using FilmAPI.Interfaces;

namespace FilmAPI.Services.Person
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;
        private readonly IPersonMapper _mapper;
        private readonly IKeyService _keyService;
        public PersonService(IPersonRepository repo, IPersonMapper mapper)
        {
            _repository = repo;
            _mapper = mapper;
            _keyService = new KeyService();
        }

        public KeyedPersonDto Add(BasePersonDto m)
        {
            var personToAdd = _mapper.MapBack(m);
            var savedPerson = _repository.Add(personToAdd);
            var key = _keyService.ConstructPersonSurrogateKey(m.LastName, m.Birthdate);
            return new KeyedPersonDto(m.LastName, m.Birthdate, key, m.FirstMidName);
        }

        public async Task<KeyedPersonDto> AddAsync(BasePersonDto m)
        {
            return await Task.Run(() => Add(m));
        }

        public async Task DeeteAsync(string key)
        {
            await Task.Run(() => Delete(key));
        }

        public void Delete(string key)
        {
            var modelToDelete = GetBySurrogateKey(key);
            var personToDelete = _mapper.MapBack(modelToDelete);
            _repository.Delete(personToDelete);
        }

        public List<KeyedPersonDto> GetAll()
        {
            var people = _repository.List();
            var baseList = _mapper.MapList(people);
            var result = new List<KeyedPersonDto>();
            foreach (var item in baseList)
            {
                var key  = _keyService.ConstructPersonSurrogateKey(item.LastName, item.Birthdate);
                var keyedItem = new KeyedPersonDto(item.LastName, item.Birthdate, key, item.FirstMidName);                
                result.Add(keyedItem);
            }
            return result;
        }

        public async Task<List<KeyedPersonDto>> GetAllAsync()
        {
            return await Task.Run(() => GetAll());
            
        }

        public KeyedPersonDto GetBySurrogateKey(string key)
        {
            var data = _keyService.DeconstructPesonSurrogateKey(key);
            var p = _repository.GetByLastNameAndBirthdate(data.lastName, data.birthdate);
            var result = new KeyedPersonDto(data.lastName, data.birthdate, key, p.FirstMidName);
            return result;
        }

        public async Task<KeyedPersonDto> GetBySurrogateKeyAsync(string key)
        {
            return await Task.Run(() => GetBySurrogateKey(key));
        }

        public async Task UpdateAsync(BasePersonDto m)
        {
            await Task.Run(() => Update(m));
        }

        public void Update(BasePersonDto m)
        {
            var personToUpdate = _mapper.MapBack(m);
            _repository.Update(personToUpdate);
        }
    }
}
