using AttendanceSystem.Data;
using AttendanceSystem.Domain;
using AttendanceSystem.Domain.Repositories;
using AttendanceSystem.Error;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
    }
    public class StudentRepository : IStudentRepository
    {
        private readonly AttendanceDbContext _context;

        public StudentRepository(AttendanceDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Student>> AddAsync(Student entity)
        {
            try
            {
                using (var connection = _context.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new NpgsqlCommand("public.addstudent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("firstname", NpgsqlTypes.NpgsqlDbType.Varchar, entity.FirstName);
                        command.Parameters.AddWithValue("lastname", NpgsqlTypes.NpgsqlDbType.Varchar, entity.LastName);
                        command.Parameters.AddWithValue("email", NpgsqlTypes.NpgsqlDbType.Varchar, entity.Email);
                        command.Parameters.AddWithValue("phone", NpgsqlTypes.NpgsqlDbType.Varchar, entity.Phone);
                        command.Parameters.AddWithValue("classid", NpgsqlTypes.NpgsqlDbType.Integer, entity.ClassId);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return Result<Student>.Success(entity);
            }
            catch (Exception ex)
            {
                return Result<Student>.Failure(ex.Message);
            }
        }



        Task<Result<IEnumerable<Student>>> IRepository<Student>.FindAsync(Expression<Func<Student, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IEnumerable<Student>>> GetAllAsync()
        {
            try
            {
                string studentCursor;
                using (var connection = _context.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var transaction = connection.BeginTransaction())
                    {
                        using (var command = new NpgsqlCommand("public.getallstudents", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            var refCursorParameter = new NpgsqlParameter("refcursor", NpgsqlTypes.NpgsqlDbType.Refcursor)
                            {
                                Direction = ParameterDirection.InputOutput,
                                Value = "refcursor"
                            };
                            command.Parameters.Add(refCursorParameter);
                            studentCursor = (string)command.Parameters["refcursor"].Value;
                            await command.ExecuteNonQueryAsync();
                            using (var fetchCommand = new NpgsqlCommand($"FETCH ALL IN \"{studentCursor}\";", connection, transaction))
                            {
                                using (var reader = await fetchCommand.ExecuteReaderAsync())
                                {
                                    var students = new List<Student>();
                                    while (await reader.ReadAsync())
                                    {
                                        var student = new Student
                                        {
                                            StudentId = reader.GetInt32(0),
                                            FirstName = reader.GetString(1),
                                            LastName = reader.GetString(2),
                                            Email = reader.GetString(3),
                                            Phone = reader.GetString(4),
                                            ClassId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5)
                                        };
                                        students.Add(student);
                                    }
                                    return Result<IEnumerable<Student>>.Success(students);
                                }
                            }
                        }
                    }
                }

            }


            catch (Exception ex)
            {
                return Result<IEnumerable<Student>>.Failure(ex.Message);
            }
        }

        public async Task<Result<Student>> GetByIdAsync(int id)
        {
            try
            {
                string studentCursor;
                using (var connection = _context.GetConnection())
                {
                    await connection.OpenAsync();
                    using var transaction = connection.BeginTransaction();
                    // Step 1: Call the stored procedure to open the cursor
                    using (var command = new NpgsqlCommand("public.getstudentbyid", connection, transaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameter for the student ID
                        command.Parameters.AddWithValue("p_studentid", NpgsqlTypes.NpgsqlDbType.Integer, id);

                        // Add output parameter for the refcursor
                        var refCursorParameter = new NpgsqlParameter("studentcursor", NpgsqlTypes.NpgsqlDbType.Refcursor)
                        {
                            Direction = ParameterDirection.InputOutput,
                            Value = "studentcursor"
                        };
                        command.Parameters.Add(refCursorParameter);
                        studentCursor = (string)command.Parameters["studentcursor"].Value;
                        // Execute the command to open the cursor
                        await command.ExecuteNonQueryAsync();
                    }

                    // Step 2: Fetch the data from the cursor
                    using (var fetchCommand = new NpgsqlCommand($"FETCH ALL IN \"{studentCursor}\";", connection, transaction))
                    {
                        using (var reader = await fetchCommand.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Populate and return the student object
                                var student = new Student
                                {
                                    StudentId = reader.GetInt32(0),
                                    FirstName = reader.GetString(1),
                                    LastName = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Phone = reader.GetString(4),
                                    ClassId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5)
                                };
                                return Result<Student>.Success(student);
                            }
                            else
                            {
                                return Result<Student>.Failure("Student not found");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return Result<Student>.Failure(ex.Message);
            }
        }

        public async Task<Result<bool>> UpdateAsync(Student entity)
        {
            try
            {
                using (var connection = _context.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new NpgsqlCommand("public.updatestudent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_studentid", NpgsqlTypes.NpgsqlDbType.Integer, entity.StudentId);
                        command.Parameters.AddWithValue("p_firstname", NpgsqlTypes.NpgsqlDbType.Varchar, entity.FirstName);
                        command.Parameters.AddWithValue("p_lastname", NpgsqlTypes.NpgsqlDbType.Varchar, entity.LastName);
                        command.Parameters.AddWithValue("p_email", NpgsqlTypes.NpgsqlDbType.Varchar, entity.Email);
                        command.Parameters.AddWithValue("p_phone", NpgsqlTypes.NpgsqlDbType.Varchar, entity.Phone);
                        command.Parameters.AddWithValue("p_classid", NpgsqlTypes.NpgsqlDbType.Integer, entity.ClassId);
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

        public async Task<Result<Student>> DeleteAsync(int id)
        {
            try
            {
                using (var connection = _context.GetConnection())
                {
                    await connection.OpenAsync();
                    var student = await GetByIdAsync(id);
                    if (!student.IsSuccess)
                    {
                        return Result<Student>.Failure("Student not found");
                    }
                    using (var command = new NpgsqlCommand("public.deletestudent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_studentid", NpgsqlTypes.NpgsqlDbType.Integer, id);
                        await command.ExecuteNonQueryAsync();
                    }
                    return Result<Student>.Success(student.Output);
                }
            }
            catch (Exception ex)
            {
                return Result<Student>.Failure(ex.Message);
            }
        }
    }
}
