using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinRost.BL.Dto.Web.Users
{
    public class ProfileResponse
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
        public string FullName { get => LastName + " " + FirstName + " " + Patronymic; }
        // public byte[]? UserPhoto { get; set; }
        public string? Position { get; set; }
        public int ArchiId { get; set; }
        public string? NumberPhone { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Email { get; set; }
    }
}
