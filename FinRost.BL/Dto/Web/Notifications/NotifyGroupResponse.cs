namespace FinRost.BL.Dto.Web.Notifications
{
    public class NotifyGroupResponse
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public List<UserNotifyGroupDto> Users { get; set; } = new List<UserNotifyGroupDto>();
        public string KeyWord { get; set; }
    }
}
