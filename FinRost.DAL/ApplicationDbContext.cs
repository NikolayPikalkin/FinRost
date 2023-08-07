using FinRost.DAL.Entities;
using FinRost.DAL.Entities.Archi;
using FinRost.DAL.Entities.Web;
using Microsoft.EntityFrameworkCore;

namespace FinRost.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ServiceLog> ServiceLogs { get; set; }
        public DbSet<RelisePlace> RelisePlaces { get; set; }
        public DbSet<User> Users { get; set; }  
        public DbSet<ArchiUser> ArchiUsers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Investor> Investors { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<LotFeedback> LotFeedbacks { get; set;}
        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationGroup> NotificationGroups { get; set; }

    }
}
