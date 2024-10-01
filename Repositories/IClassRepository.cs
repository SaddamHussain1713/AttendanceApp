using AttendanceSystem.Data;
using AttendanceSystem.Domain;
using AttendanceSystem.Domain.Repositories;
using AttendanceSystem.Error;
using Npgsql;
using System.Data;
using System.Linq.Expressions;

namespace AttendanceApp.Repositories
{
    public interface IClassEntityRepository : IRepository<ClassEntity>
    {

    }
    public class ClassEntityRepository : IClassEntityRepository
    {
        private readonly AttendanceDbContext _context;

        public ClassEntityRepository(AttendanceDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ClassEntity>> AddAsync(ClassEntity entity)
        {
            try
            {
                using (var connection = _context.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new NpgsqlCommand("public.addclass", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_classname", NpgsqlTypes.NpgsqlDbType.Varchar, entity.Name);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                var isAdded = await GetByIdAsync(entity.Id);
                if (isAdded == null)
                {
                    return Result<ClassEntity>.Failure("Failed to add class");
                }
                return Result<ClassEntity>.Success(isAdded.Output);
            }
            catch (Exception ex)
            {
                return Result<ClassEntity>.Failure(ex.Message);
            }
        }


        public Task<Result<IEnumerable<ClassEntity>>> FindAsync(Expression<Func<ClassEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public async Task<Result<bool>> UpdateAsync(ClassEntity entity)
        {
            try
            {
                using (var connection = _context.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new NpgsqlCommand("public.updateclass", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_classid", NpgsqlTypes.NpgsqlDbType.Integer, entity.Id);
                        command.Parameters.AddWithValue("@p_classname", NpgsqlTypes.NpgsqlDbType.Varchar, entity.Name);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }

        public async Task<Result<ClassEntity>> DeleteAsync(int id)
        {
            try
            {
                using (var connection = _context.GetConnection())
                {
                    await connection.OpenAsync();
                    var classEntity = await GetByIdAsync(id);
                    if (!classEntity.IsSuccess)
                    {
                        return Result<ClassEntity>.Failure("Class not found");
                    }
                    using (var command = new NpgsqlCommand("public.deleteclass", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_classid", NpgsqlTypes.NpgsqlDbType.Integer, id);
                        await command.ExecuteNonQueryAsync();
                    }
                    return Result<ClassEntity>.Success(classEntity.Output);

                }
            }
            catch (Exception ex)
            {
                return Result<ClassEntity>.Failure(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<ClassEntity>>> GetAllAsync()
        {
            try
            {
                using (var connection = _context.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var entities = new List<ClassEntity>();
                        var classCursor = "";
                        using (var command = new NpgsqlCommand("public.getallclasses", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            var classParam = new NpgsqlParameter("@refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor)
                            {
                                Direction = ParameterDirection.InputOutput,
                                Value = "refcursor"
                            };
                            command.Parameters.Add(classParam);
                            classCursor = (string)command.Parameters["@refcursor"].Value;
                            await command.ExecuteNonQueryAsync();
                            using (var fetchcommand = new NpgsqlCommand($"FETCH ALL IN {classCursor}", connection, transaction))
                            {
                                using (var reader = await fetchcommand.ExecuteReaderAsync())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            entities.Add(new ClassEntity
                                            {
                                                Id = reader.GetInt32(0),
                                                Name = reader.GetString(1)
                                            });
                                        }
                                    }
                                }
                            }
                        }
                        return Result<IEnumerable<ClassEntity>>.Success(entities);
                    }
                }

            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ClassEntity>>.Failure(ex.Message);
            }
        }

        public async Task<Result<ClassEntity>> GetByIdAsync(int id)
        {
            try
            {
                string classCursor;
                using (var connection = _context.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var transaction = connection.BeginTransaction())
                    {
                        using (var command = new NpgsqlCommand("public.getclassbyid", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@p_classid", NpgsqlTypes.NpgsqlDbType.Integer, id);
                            var classParam = new NpgsqlParameter("@refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor)
                            {
                                Direction = ParameterDirection.InputOutput,
                                Value = "refcursor"
                            };
                            command.Parameters.Add(classParam);
                            classCursor = (string)command.Parameters["@refcursor"].Value;
                            await command.ExecuteNonQueryAsync();
                        }
                        using (var fetchcommand = new NpgsqlCommand($"FETCH ALL IN {classCursor}", connection, transaction))
                        {
                            using (var reader = await fetchcommand.ExecuteReaderAsync())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        return Result<ClassEntity>.Success(new ClassEntity
                                        {
                                            Id = reader.GetInt32(0),
                                            Name = reader.GetString(1)
                                        });
                                    }
                                }
                            }
                        }

                    }
                }
                return Result<ClassEntity>.Failure("Class not found");
            }
            catch (Exception ex)
            {
                return Result<ClassEntity>.Failure(ex.Message);
            }
        }
    }
}
