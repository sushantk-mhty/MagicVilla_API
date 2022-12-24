using System;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository
{
	public class Repository<T>: IRepository<T> where T: class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task CreateAsysnc(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsysnc();
        }

        public async Task<T> GetAsysnc(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
                query = query.AsNoTracking();
            if (filter != null)
                query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsysnc(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
                query = query.Where(filter);
            return await query.ToListAsync();
        }

        public async Task RemoveAsysnc(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsysnc();
        }

        public async Task SaveAsysnc()
        {
            await _db.SaveChangesAsync();
        }

        //public async Task UpdteAsysnc(T entity)
        //{
        //    dbSet.Update(entity);
        //    await SaveAsysnc();
        //}
    }
}

