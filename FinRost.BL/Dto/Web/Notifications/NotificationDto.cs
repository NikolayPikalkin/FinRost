namespace FinRost.BL.Dto.Web.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Checked { get; set; }

    }
}
