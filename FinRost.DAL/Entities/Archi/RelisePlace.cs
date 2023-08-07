using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinRost.DAL.Entities.Archi
{
    [Table("RELISE_PLACES")]
    public class RelisePlace
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public short Enabled { get; set; }
        public int OrgId { get; set; }
    }
}
