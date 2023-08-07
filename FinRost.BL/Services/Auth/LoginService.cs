using FinRost.DAL;
using FinRost.DAL.Dto.Login;
using FinRost.DAL.Services;

namespace FinRost.BL.Services.Auth
{
    public class LoginService
    {
        private readonly AesCriptoService _cripto;
        private readonly ApplicationDbContext _db;
        private readonly UserService _userService;

        public LoginService(AesCriptoService cripto, ApplicationDbContext db, UserService userService)
        {
            _db = db;
            _cripto = cripto;
            _userService = userService;
        }

        public async Task<LoginResponse> Login(LoginRequest request, string ipAddress)
        {
            var user = await _userService.FindUserByLoginAndPassword(request.Login, request.Password);
            if (user is null)
                return new LoginResponse { Message = "Неверный логин и/или пароль!" };

            if (user.Blocked || !user.Enabled)
                return new LoginResponse { Message = "Учетная запись заблокирована!" };

            string stringToEncrypt = user.Id.ToString() + "/" + user.Login + "/" + ipAddress;
            byte[] encryptedString = _cripto.EncryptStringToBytes_Aes(stringToEncrypt);

            var token = Convert.ToBase64String(encryptedString);
            return new LoginResponse { Token = token };
        }
    }
}
