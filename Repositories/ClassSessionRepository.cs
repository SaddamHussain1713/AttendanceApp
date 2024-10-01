using AttendanceSystem.Data;
using AttendanceSystem.Domain;
using AttendanceSystem.Domain.Repositories;
using AttendanceSystem.Error;
using Npgsql;
using System.Data;
using System.Linq.Expressions;

namespace AttendanceApp.Repositories
{
    public interface IClassSessionRepository:IRepository<ClassSession>
    {
        //Add any additional methods here
    }
    public class ClassSessionRepository : IClassSessionRepository
    {
        private readonly AttendanceDbContext _context;

        public ClassSessionRepository(AttendanceDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ClassSession>> AddAsync(ClassSession entity)
        {
            try
            {
                using(var connection = _context.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new NpgsqlCommand("public.addclasssession", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_classid",NpgsqlTypes.NpgsqlDbType.Integer,entity.ClassId);
                        command.Parameters.AddWithValue("p_sessiondate",NpgsqlTypes.NpgsqlDbType.Date,entity.SessionDate);
                        await command.ExecuteNonQueryAsync();
                        return Result<ClassSession>.Success(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Result<ClassSession>.Failure(ex.Message);
            }
        }

        public Task<Result<ClassSession>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<ClassSession>>> FindAsync(Expression<Func<ClassSession, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<ClassSession>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<ClassSession>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> UpdateAsync(ClassSession entity)
        {
            throw new NotImplementedException();
        }
    }
}
