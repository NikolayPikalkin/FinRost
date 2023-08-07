using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinRost.DAL.Entities.Archi
{

    [Table("CLIENTS")]
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Column("FULLNAME")]
        public string FullName { get; set; }

        [Column("NAME1")]
        public string FirstName { get; set; }

        [Column("NAME")]
        public string LastName { get; set; }

        [Column("NAME2")]
        public string Patronymic { get; set; }

        [Column("OLDFIO")]
        public string PreviousFullName { get; set; }

        [Column("MOBILEPHONE")]
        public string MobilePhone { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("RELISEPLACEID")]
        public int RelisePlaceId { get; set; }

        [Column("DOCNUM")]
        public string NumberPassport { get; set; }

        [Column("DOCS")]
        public string SeriesPassport { get; set; }

        [Column("Birthdate")]
        public int _Birthdate { get; set; }

        [NotMapped]
        public DateTime Birthdate { get => DateTime.FromOADate(_Birthdate); set => _Birthdate = (int)value.ToOADate(); }

        [Column("ENABLED")]
        public short Enabled { get; set; }

        [Column("PHOTO")]
        public byte[] Photo { get; set; }

        [Column("DOCBEGINDATE")]
        public int? _DocBeginDate { get; set; }

        [NotMapped]
        public DateTime DocBeginDate
        {
            get => DateTime.FromOADate(_DocBeginDate ?? 0);
            set => _DocBeginDate = (int)value.ToOADate();
        }

        [Column("DOCCONTENT")]
        public string DocContent { get; set; }

        [Column("BIRTHPLACE")]
        public string BirthPlace { get; set; }

        [Column("DOCCODE")]
        public string DocCode { get; set; }

        [Column("CONSENTDATE")]
        public int? _ConsentDate { get; set; }

        [NotMapped]
        public DateTime ConsentDate { get => DateTime.FromOADate((int)_ConsentDate); set => _ConsentDate = (int)value.ToOADate(); }

        [Column("CONSENTENDDATE")]
        public int? _ConsentEndDate { get; set; }

        [NotMapped]
        public DateTime ConsentEndDate { get => DateTime.FromOADate((int)_ConsentEndDate); set => _ConsentEndDate = (int)value.ToOADate(); }

        [Column("OKB_CREDITHISTORY")]
        public string OKBCreditHistory { get; set; }

        [Column("OKB_CREDITHISTORYDATE")]
        public int? _OKBCreditHistoryDate { get; set; }
        [NotMapped]
        public DateTime OKBCreditHistoryDate { get => _OKBCreditHistoryDate is null ? DateTime.Today : DateTime.FromOADate((int)_OKBCreditHistoryDate); set => _OKBCreditHistoryDate = (int)value.ToOADate(); }

        [Column("ADDRESS_REG")]
        public string AddressReg { get; set; }

        [Column("ADDRESS_FACT")]
        public string AddressFact { get; set; }

        [Column("FAMILYQUANT")]
        public short? FamilyQuant { get; set; }

        [Column("FAMILYSTATUS")]
        public int FamilyStatus { get; set; }

        [Column("DATEADDPHOTO")]
        public double? _DateAddPhoto { get; set; }
        [NotMapped]
        public DateTime? DateAddPhoto { get => DateTime.FromOADate(_DateAddPhoto ?? 0); set => _DateAddPhoto = value?.ToOADate(); }

        [Column("ADDRESSEQUAL")]
        public short AddressEqual { get; set; }

        [Column("CARDNUMBER")]
        public string CardNumber { get; set; }

        [Column("KORACCT")]
        public string KorAcct { get; set; } // Кореспондентский счет

        [Column("BANK")]
        public string Bank { get; set; }

        [Column("INN")]
        public string INN { get; set; } // ИНН банка

        [Column("KPP")]
        public string KPP { get; set; }

        [Column("OKATO")]
        public string OKATO { get; set; }

        [Column("BIK")]
        public string BIK { get; set; }

        [Column("USERID")]
        public int UserId { get; set; }
    }
}
