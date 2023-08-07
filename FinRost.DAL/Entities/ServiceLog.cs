using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinRost.DAL.Entities
{
    [Table("ServiceLog")]
    public class ServiceLog
    {
        [Key]
        public int Id { get; set; }
        public int? EntityId { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Title { get; set; }
        public string LogMessage { get; set; }
    }
}
