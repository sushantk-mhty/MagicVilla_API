using System;
using System.Linq.Expressions;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
	public interface IVillaRepository
	{
        Task<List<Villa>> GetAllAsysnc(Expression<Func<Villa,bool>> filter = null);
        Task<Villa> GetAsysnc(Expression<Func<Villa,bool>> filter = null, bool tracked=true);
        Task CreateAsysnc(Villa entity);
        Task UpdteAsysnc(Villa entity);
        Task RemoveAsysnc(Villa entity);
		Task SaveAsysnc();
	}
}

