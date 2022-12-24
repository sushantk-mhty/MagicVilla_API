using System;
using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
        Task<List<T>> GetAllAsysnc(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsysnc(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task CreateAsysnc(T entity);
        Task RemoveAsysnc(T entity);
        Task SaveAsysnc();
    }
}

