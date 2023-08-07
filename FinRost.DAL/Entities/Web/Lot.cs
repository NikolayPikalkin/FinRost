using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinRost.DAL.Entities.Web
{
    [Table("Lots")]
    public class Lot
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int InvestorId { get; set; } = 0;
        [Required]
        [MaxLength(50)]
        public string PledgeType { get; set; }
        [Required]
        public double Square { get; set; }
        public string Address { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
