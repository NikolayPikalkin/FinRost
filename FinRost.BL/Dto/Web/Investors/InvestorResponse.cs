using System.ComponentModel.DataAnnotations;

namespace FinRost.BL.Dto.Web.Investor
{
    public class InvestorResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string FullName { get => $"{LastName} {FirstName} {Patronymic}";}
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public long? ChatId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BirthPlace { get; set; }
        public string? Passport { get; set; } // Серия и номер
        public DateTime? IssueDate { get; set; }
        public string? IssueName { get; set; }
        public string? IssueCode { get; set; }
        public double MinLoanSum { get; set; }
        //Банковские
        public string? CurrentAccount { get; set; }
        public string? CorespondeAccount { get; set; }
        public string? BankName { get; set; }
        public string? INN { get; set; }
        public string? BIK { get; set; }
    }
}
