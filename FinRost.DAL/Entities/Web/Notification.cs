using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinRost.DAL.Entities.Web
{
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool Enabled { get; set; }
        public bool Checked { get; set; }

        //[NotMapped]
        //public string Class { get; set; }
    }
}
