using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinRost.DAL.Entities.Web
{
    [Table("Investors")]
    public class Investor
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public long? ChatId { get; set; }
        public string? BirthPlace { get; set; }
        public string? Passport { get; set; } // Серия и номер
        public DateTime? IssueDate { get; set; }
        public string? IssueName { get; set; }
        public string? IssueCode { get; set; }

        [NotMapped]
        public string FullName { get => $"{LastName} {FirstName} {Patronymic}"; }
        public string PhoneNumber { get; set; } = string.Empty;
        public double MinLoanSum { get; set; } // КРИТЕРИЙ Сумма минимальная для отправки лота
        
        //Банковские
        public string? CurrentAccount { get; set; } 
        public string? CorespondeAccount { get; set; }
        public string? BankName { get; set; }
        public string? INN{ get; set; }
        public string? BIK { get; set; }
    }
}
