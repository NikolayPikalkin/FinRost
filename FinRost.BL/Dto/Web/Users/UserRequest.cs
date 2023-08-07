using System.ComponentModel.DataAnnotations;

namespace FinRost.BL.Dto.Web.Users
{
    public class UserRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Логин не может быть пустым!")]
        public string Login { get; set; } = null!;

        [Required(ErrorMessage = "Пароль не может быть пустым!")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Имя не может быть пустым!")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Фамилия не может быть пустая!")]
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }

        [EmailAddress(ErrorMessage = "Не соответствует шаблону pochta@domen.*")]
        public string? Email { get; set; }
        public string? Position { get; set; }

        [Required(ErrorMessage = "ArchiId не выбран!")]
        public int ArchiId { get; set; }

        [Phone]
        public string? NumberPhone { get; set; }

        [Required(ErrorMessage = "Дата рождения не указана!")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
        public bool Blocked { get; set; }
        public bool Enabled { get; set; }
    }
}
