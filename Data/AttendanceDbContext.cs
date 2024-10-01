using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Data
{
    public class AttendanceDbContext
    {
        private readonly string _connectionString;

        public AttendanceDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
