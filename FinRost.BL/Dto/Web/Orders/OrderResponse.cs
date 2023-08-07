namespace FinRost.BL.Dto.Web.Orders
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public string RelisePlace { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public bool IsLot { get; set; } = false;
        public int CountFeedbacks { get; set; }
    }
}
