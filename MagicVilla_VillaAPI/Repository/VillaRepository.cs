using System;
using System.Linq.Expressions;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Repository
{
	public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext _db;
		public VillaRepository(ApplicationDbContext db)
		{
            _db = db;
		}

        public async Task CreateAsysnc(Villa entity)
        {
            await _db.Villas.AddAsync(entity);
            await SaveAsysnc();
        }

        public async Task<Villa> GetAsysnc(Expression<Func<Villa,bool>> filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = _db.Villas;
            if (!tracked)
                query = query.AsNoTracking();
            if (filter != null)
                query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Villa>> GetAllAsysnc(Expression<Func<Villa,bool>> filter = null)
        {
            IQueryable<Villa> query = _db.Villas;
            if (filter != null)
                query = query.Where(filter);
            return await query.ToListAsync();
        }

        public async Task RemoveAsysnc(Villa entity)
        {
            _db.Villas.Remove(entity);
            await SaveAsysnc();
        }

        public async Task SaveAsysnc()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdteAsysnc(Villa entity)
        {
            _db.Villas.Update(entity);
            await SaveAsysnc();
        }
    }
}

