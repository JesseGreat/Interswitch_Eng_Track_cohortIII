using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Models
{
    public class Result<T>
    {
        public Result()
        {
        }
        public Result(bool success)
        {
            Success = success;
        }
        public Result(bool success, int status) : this(success) { Status = status; }
        public Result(bool success, string message) : this(success) { Message = message; }
        public Result(bool success, string message, int status) : this(success, status) { Message = message; }
        public Result(bool success, T content) : this(success) { Content = content; }
        public Result(bool success, string message, T content) : this(success, message) { Content = content; }
        public Result(bool success, string message, T content, int status) : this(success, message, status) { Content = content; }
        public bool Success { get; set; } = false; public string Message { get; set; }
        public int Status { get; set; }
        public T Content { get; set; }
    }
    public class Result : Result<object>
    {
        public Result() : base()
        {
        }
        public Result(bool success) : base(success)
        {
        }
        public Result(bool success, string message) : base(success, message) { }
        public Result(bool success, string message, int status) : base(success, message, status) { }
        public Result(bool success, object content) : base(success, string.Empty, content) { }
        public Result(bool success, string message, object content) : base(success, message, content) { }
        public Result(bool success, string message, object content, int status) : this(success, message, status) { }
    }
}
