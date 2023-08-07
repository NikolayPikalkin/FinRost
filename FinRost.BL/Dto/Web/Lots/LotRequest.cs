using System.ComponentModel.DataAnnotations;

namespace FinRost.BL.Dto.Web.Lots
{
    public class LotRequest
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int InvestorId { get; set; } = 0;
        [Required]
        public string PledgeType { get; set; }
        [Required]
        public double Square { get; set; }
        public string Address { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
    }
}
