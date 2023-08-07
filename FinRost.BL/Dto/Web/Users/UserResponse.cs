using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinRost.BL.Dto.Web.Users
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string FullName { get => LastName + " " + FirstName + " " + Patronymic; }
        public string? Position { get; set; }
        public int ArchiId { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Blocked { get; set; }
    }
}
