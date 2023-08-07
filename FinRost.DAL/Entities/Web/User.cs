using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinRost.DAL.Entities.Web
{
    [Table("WebUsers")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }

        [NotMapped]
        public string FullName { get => LastName + " " + FirstName + " " + Patronymic; }
        public string? NumberPhone { get; set; }
        public DateTime? Birthdate { get; set; }
        public bool Enabled { get; set; }
        public bool Blocked { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string Password { get; set; }
        public byte[]? UserPhoto { get; set; }
        public string? Position { get; set; }
        public int ArchiId { get; set; }

        public long? TelegramChatId { get; set; }

        [NotMapped]
        public List<string> Connections { get; set; }
    }
}
