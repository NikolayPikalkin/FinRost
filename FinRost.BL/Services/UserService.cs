using AutoMapper;
using FinRost.BL.Dto.Web.Users;
using FinRost.BL.Extensions;
using FinRost.DAL.Entities.Archi;
using FinRost.DAL.Entities.Web;
using Microsoft.EntityFrameworkCore;

namespace FinRost.DAL.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public UserService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        public async Task<User?> FindUserByLoginAndPassword(string login, string password)
        {
            return await _db.Users.FirstOrDefaultAsync(it => it.Login == login && it.Password == password);
        }
        public async Task<User?> FindUserByIdAndLogin(int Id, string login)
        {
            return await _db.Users.FirstOrDefaultAsync(it => it.Id == Id && it.Login == login);
        }
        public async Task<List<User>> GetUsersAsync()
        {
            return await _db.Users.Where(it => it.Enabled).ToListAsync();

        }
        public async Task<User> GetUserByIdAsync(int Id)
        {
            var user = new User();

            if (Id != 0)
                user = await _db.Users.FindAsync(Id);

            return user;
        }
        public async Task<User> CreateUser(UserRequest userRequest)
        {
            var repeatLogin = await _db.Users.FirstOrDefaultAsync(it => it.Login == userRequest.Login) != null;
            if (repeatLogin)
                throw new CustomException("Такой логин уже занят!");

            var newUser = _mapper.Map<User>(userRequest);

            newUser.Enabled = true;
            newUser.Blocked = false;
            newUser.FirstName = newUser.FirstName.Replace(" ", "");
            newUser.LastName = newUser.LastName.Replace(" ", "");
            newUser.Patronymic = newUser.Patronymic.Replace(" ", "");

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            return newUser;
        }
        public async Task<User> EditUser(UserRequest userRequest)
        {
            var userDb = await _db.Users.FindAsync(userRequest.Id);
            if (userDb is null)
                throw new CustomException("Пользователь не найден!");

            var repeatLogin = await _db.Users.FirstOrDefaultAsync(it => it.Login == userRequest.Login && it.Id != userRequest.Id) != null;
            if (repeatLogin)
                throw new CustomException("Такой логин уже занят!");

            userDb.Login = userRequest.Login;
            userDb.Password = userRequest.Password;
            userDb.FirstName = userRequest.FirstName.Replace(" ", "");
            userDb.LastName = userRequest.LastName.Replace(" ", "");
            userDb.Patronymic = userRequest.Patronymic?.Replace(" ", "");
            userDb.Birthdate = userRequest.Birthdate;
            userDb.NumberPhone = userRequest.NumberPhone;
            userDb.Email = userRequest.Email?.Replace(" ", "");
            userDb.Position = userRequest.Position;
            userDb.ArchiId = userRequest.ArchiId;
            userDb.Blocked = userRequest.Blocked;


            _db.Entry(userDb).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return userDb;

        }
        public async Task DeleteUser(int Id)
        {
            var user = await _db.Users.FindAsync(Id);
            if (user is null)
                throw new CustomException("Пользователь не найден!");

            user.Enabled = false;
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
        public async Task<List<ArchiUser>> GetArchiUsersAsync()
        {
            return await _db.ArchiUsers.Where(u => u.Enabled == 1 && u.Locked == 0).OrderBy(u => u.Name).ToListAsync();
        }
    }
}
