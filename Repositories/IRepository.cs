using AttendanceSystem.Error;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AttendanceSystem.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<Result<T>> AddAsync(T entity);
        Task<Result<T>> GetByIdAsync(int id);
        Task<Result<IEnumerable<T>>> GetAllAsync(); 
        Task<Result<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> predicate); 
        Task<Result<bool>> UpdateAsync(T entity); 
        Task<Result<T>> DeleteAsync(int id);
    }

}
