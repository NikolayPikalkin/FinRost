using System.ComponentModel.DataAnnotations.Schema;

namespace FinRost.DAL.Entities.Archi
{
    [Table("USERS")]
    public class ArchiUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short Enabled { get; set; }
        public short Locked { get; set; }
    }
}
