using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinRost.DAL.Dto.Login
{
    public class LoginResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; } = null;
    }
}
