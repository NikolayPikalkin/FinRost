using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinRost.BL.Dto.Web.Notifications
{
    public class NotifyGroupRequest
    {
        public int Id { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string KeyWord { get; set; } = string.Empty;
        public List<int> Users { get; set; } = new List<int>();
    }
}
