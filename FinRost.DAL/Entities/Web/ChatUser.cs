namespace FinRost.DAL.Entities.Web
{
    public class ChatUser
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

    }
}
