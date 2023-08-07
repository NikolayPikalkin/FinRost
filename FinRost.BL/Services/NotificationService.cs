using FinRost.BL.Dto.Web.Notifications;
using FinRost.BL.Extensions;
using FinRost.DAL;
using FinRost.DAL.Entities.Web;
using FinRost.DAL.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FinRost.BL.Services
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserService _userService;

        public NotificationService(ApplicationDbContext db, UserService userService)
        {
            _db = db;
            _userService = userService;
        }

        public async Task<List<NotificationGroup>> GetNotifyGroupsAsync()
        {
            return await _db.NotificationGroups.Where(it => it.Enabled).ToListAsync();
        }

        public async Task<NotifyGroupResponse> GetNotifyGroupByIdAsync(int Id)
        {
            var notifyGroup = await _db.NotificationGroups.FindAsync(Id);
            if (notifyGroup is null)
                throw new CustomException("Группа уведомлений не найдена!");

            var users = await _userService.GetUsersAsync();
            var usersInNotifyGroup = notifyGroup.ListUsersId.Split(',').Select(it => Convert.ToInt32(it)).ToList();

            var response = new NotifyGroupResponse
            {
                GroupName = notifyGroup.Name,
                Id = notifyGroup.Id,
                KeyWord = notifyGroup.KeyWord,
                Users = users.Select(it => new UserNotifyGroupDto 
                {
                    Id = it.Id,
                    Member = usersInNotifyGroup.Contains(it.Id),
                    Position = it.Position ?? string.Empty,
                    FullName = it.FullName,
                }).ToList()
            };

            return response;
        }

        public async Task EditNotifyGroupAsync(NotifyGroupRequest request)
        {
            var notifyGroup = await _db.NotificationGroups.FindAsync(request.Id);
            if (notifyGroup is null) 
                throw new CustomException("Группа уведомления не найдена!");

            notifyGroup.Name = request.GroupName;
            notifyGroup.KeyWord = request.KeyWord;
            notifyGroup.ListUsersId = string.Join(',',request.Users);
            
            _db.Entry(notifyGroup).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<List<int>> GetUsersNotificationAsync(string notifyKeyWord)
        {
            var listUsersId = new List<int>();
            var notificationGroup = await _db.NotificationGroups.FirstOrDefaultAsync(it => it.KeyWord == notifyKeyWord);
            if (notificationGroup is null)
                return new List<int>();

            return notificationGroup.ListUsersId.Split(',').Select(it => Convert.ToInt32(it)).ToList();
        }

        public async Task SendNotificationAsync(int fromUserId, int toUserId, string desc, string title)
        {
            var newNotify = new Notification
            {
                Checked = false,
                CreationDate = DateTime.Now,
                Description = desc,
                Title = title,
                Enabled = true,
                FromUserId = fromUserId,
                ToUserId = toUserId,
            };

            await _db.AddAsync(newNotify);
            await _db.SaveChangesAsync();
        }

        public async Task SendNotificationFromRobotAsync(int toUserId, string desc, string title)
        {
            var robot = await _db.Users.FirstOrDefaultAsync(it => it.Login == "robot" && it.Enabled);
            if (robot is null)
                return;

            var newNotify = new Notification
            {
                Checked = false,
                CreationDate = DateTime.Now,
                Description = desc,
                Title = title,
                Enabled = true,
                FromUserId = robot.Id,
                ToUserId = toUserId,
            };

            await _db.AddAsync(newNotify);
            await _db.SaveChangesAsync();
        }

        public async Task<NotificationGroup> CreateNotifyGroupAsync(NotifyGroupRequest request)
        {
            var newGroup = new NotificationGroup
            {
                Enabled = true,
                KeyWord = request.KeyWord,
                ListUsersId = string.Join(',', request.Users),
                Name = request.GroupName
            };

            await _db.NotificationGroups.AddAsync(newGroup);
            await _db.SaveChangesAsync();

            return newGroup;
        }

        public async Task DeleteNotifyGroupAsync(int id)
        {
            var group = await _db.NotificationGroups.FindAsync(id);
            if (group is null)
                throw new CustomException("Группа уведомлений с ID " + id + " не найдена!");

            group.Enabled = false;
            await _db.SaveChangesAsync();
        }
         
        public async Task<List<Notification>> GetProfileNoticesAsync(int userId)
        {
            var notices = await _db.Notifications.Where(it => it.Enabled && it.ToUserId == userId)
                                                  .OrderByDescending(it => it.CreationDate)
                                                  .Take(30)
                                                  .ToListAsync();
            return notices;
        }

        public async Task CheckAllProfileNoticesAsync(int userId)
        {
            var notices = await _db.Notifications.Where(it => it.Enabled && it.ToUserId == userId && !it.Checked).ToListAsync();

            foreach(var it in notices)
            {
                it.Checked = true;
                _db.Entry(it).State = EntityState.Modified;
            }
            await _db.SaveChangesAsync();
        }
    }
}
