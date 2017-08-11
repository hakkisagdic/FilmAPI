﻿using FilmAPI.Core.Interfaces;
using FilmAPI.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FilmAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FilmAPI.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly FilmContext _context;
        protected readonly DbSet<T> _set;
        public Repository(FilmContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }
        public T Add(T t)
        {
            _set.Add(t);
            Save();
            return t;
        }

        private void Save()
        {
            _context.SaveChanges();
        }

        public async Task<T> AddAsync(T t)
        {
            await _set.AddAsync(t);
            await SaveAsync();
            return t;
        }

        private async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Delete(T t)
        {
            _set.Remove(t);
            Save();
        }

        public async Task DeleteAsync(T t)
        {
            _set.Remove(t);
            await SaveAsync();
        }

        public T GetById(int id)
        {
            return _set.Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _set.FindAsync(id);
        }

        public T GetBySurrogateKey(string key)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetBySurrogateKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> List()
        {
            return _set;
        }

        public IEnumerable<T> List(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate).AsEnumerable();
        }

        public IEnumerable<T> List(ISpecification<T> specification)
        {
            return List(specification.Predicate);
        }

        public async Task<IEnumerable<T>> ListAsync()
        {
            return await _set.ToListAsync();
        }

        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate)
        {
            return await _set.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> ListAsync(ISpecification<T> specification)
        {
            return await ListAsync(specification.Predicate);
        }

        public void Update(T t)
        {
            Save();
        }

        public async Task UpdateAsync(T t)
        {
            _context.Entry(t).State = EntityState.Modified;
            await SaveAsync();
        }
    }
}
