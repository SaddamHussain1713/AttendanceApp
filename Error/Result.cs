using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Error
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Output { get; set; }
        public string ErrorMessage { get; set; }

        public static Result<T> Success(T output)
        {
            return new Result<T>
            {
                IsSuccess = true,
                Output = output
            };
        }
        public static Result<T> Failure(string errorMessage)
        {
            return new Result<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
