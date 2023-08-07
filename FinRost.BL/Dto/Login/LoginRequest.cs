using System.ComponentModel.DataAnnotations;

namespace FinRost.DAL.Dto.Login
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
