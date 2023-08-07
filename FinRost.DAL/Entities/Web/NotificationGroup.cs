using System.ComponentModel.DataAnnotations;

namespace FinRost.DAL.Entities.Web
{
    public class NotificationGroup
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string KeyWord { get; set; }
        public string ListUsersId { get; set; } = string.Empty;
        public bool Enabled { get; set; }
    }
}
