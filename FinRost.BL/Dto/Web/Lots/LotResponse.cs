namespace FinRost.BL.Dto.Web.Lots
{
    public class LotResponse
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int InvestorId { get; set; } = 0;
        public string PledgeType { get; set; }
        public double Square { get; set; }
        public string Address { get; set; } = string.Empty;
        public double Price { get; set; }
    }
}
