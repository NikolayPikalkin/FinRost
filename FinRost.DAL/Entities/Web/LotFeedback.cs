using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinRost.DAL.Entities.Web
{
    [Table("LotFeedbacks")]
    public class LotFeedback
    {
        public int Id { get; set; } 
        public int InvestorId { get; set;}
        public int OrderId { get; set; }
        public int LotId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Checked { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
