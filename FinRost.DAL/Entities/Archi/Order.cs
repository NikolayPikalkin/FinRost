using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinRost.DAL.Entities.Archi
{
    [Table("ORDERS")]
    public class Order
    {
        [Key]
        public int ID { get; set; }
        public short ENABLED { get; set; }
        public string NUMBER { get; set; }
        public double? DAYSQUANT { get; set; }
        public string FULLNAME { get; set; }
        public int USERID { get; set; }
        public int CLIENTID { get; set; }
        public short ORDERSTATUS { get; set; }

        [Column("PUTDATETIME")]
        public double? _PutDateTime { get; set; } //Дата выдачи займа

        [NotMapped]
        public DateTime PutDateTime { get => DateTime.FromOADate(_PutDateTime ?? 0); set => _PutDateTime = (double)value.ToOADate(); }

        [Column("NEXTPAYDATE")]
        public int? _NextPayDate { get; set; }

        [NotMapped]
        public DateTime NextPayDate { get => DateTime.FromOADate(_NextPayDate ?? 0); set => _NextPayDate = (int)value.ToOADate(); }

        [Column("CREATIONDATETIME")]
        public double _CreationDateTime { get; set; } //Дата создания ордера

        [NotMapped]
        public DateTime CreationDateTime { get => DateTime.FromOADate(_CreationDateTime); set => _CreationDateTime = (double)value.ToOADate(); }

        [Column("RETURNDATETIME")]
        public int? _ReturnDateTime { get; set; } //Дата возврата займа

        [NotMapped]
        public DateTime ReturnDateTime { get => DateTime.FromOADate(_ReturnDateTime ?? 0); set => _ReturnDateTime = (int)value.ToOADate(); }

        [Column("CLOSEDATETIME")]
        public double? _CloseDateTime { get; set; } //Дата возврата займа

        [NotMapped]
        public DateTime CloseDateTime { get => DateTime.FromOADate(_CloseDateTime ?? 0); set => _CloseDateTime = value.ToOADate(); }

        [Required]
        public decimal LOANCOSTALL { get; set; }  //Изначальная сумма займа
        public decimal LOANRESTCOSTALL { get; set; } // Остаток по займу
        public double? MAINPERCENT { get; set; } //Основной процент по займу
        public double? PENICOSTALL { get; set; } //Начисленный штраф
        public double? PERCENTCOSTALL { get; set; } //Размер основного процента
        public int RELISEPLACEID { get; set; }//офис где завелся документ
        public int ENTERRELISEPLACEID { get; set; }//ОФИС ВЫДАЧИ ЗАЙМА
        public int ACCOUNTID { get; set; } // НАЛИЧКА ИЛИ КАРТА
        public double? STATETAXCOSTNEED { get; set; } // ГОСПОШЛИНА сколько нужно
        public double? STATETAXCOST { get; set; } // ГОСПОШЛИНА сколько было оплачено
        public int DAYSTRAFQUANT { get; set; } //Количество дней просрочки
        public int POID { get; set; } //ID ПРАВИЛО РАСЧЕТА
        public short? ISFROST { get; set; }// ЗАМОРОЖЕН ИЛИ НЕТ

        [Column("STOPISSUD")]
        public short CourtStatus { get; set; }

        [Column("MONTHINCOME")]
        public double? MonthIncome { get; set; }

        [Column("LOANAIM")]
        public string LoanAim { get; set; }

        #region ----Работа----
        [Column("WORKNAME")]
        public string WorkName { get; set; }
        [Column("WORKHEADMEN")]
        public string WorkHead { get; set; }

        [Column("WORKADDRESS")]
        public string WorkAddress { get; set; }

        [Column("WORKPROF")]
        public string WorkProf { get; set; }

        [Column("WORKSTAZH")]
        public string WorkStazh { get; set; }

        [Column("PREVIOSWORKNAME")]
        public string PreviousWork { get; set; }

        #endregion

        #region ----------Дополнительно---------------
        [Column("PUTMONEYQUANT")]
        public string PutMoneyQuant { get; set; }

        [Column("LASTPUTMONEYDATE")]
        public string LastPutMoneyDate { get; set; }

        [Column("ADDITIONALINCOME")]
        public string AdditionalIncome { get; set; }

        [Column("MONTHEXPENSE")]
        public string MonthExpense { get; set; }

        [Column("CHILDRENBEFORE21")]
        public string ChildrenBefore21 { get; set; }

        [Column("IZHDIVENECLIST")]
        public string IzhdivenecList { get; set; }

        [Column("SUPRUGINFO")]
        public string SuprugInfo { get; set; }

        [Column("INFORMATIONSOURCE")]
        public int InformationSourceId { get; set; }
        [Column("ADULTINFO")]
        public string ParentInfo { get; set; }

        [Column("INFO")]
        public string ManagerInfo { get; set; }

        #endregion

        [Column("OLDCREDITS")]
        public string OldCredits { get; set; }

        [Column("SOURCEINCOMELOAN")]
        public string SourceIncome { get; set; }

    }
}
