namespace FinRost.BL.Dto.Web.Orders
{
    public class OrderDetailResponse
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public double Daysquant { get; set; }
        public string FullName { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public int OrderStatus { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime PutDateTime { get; set; }
        public DateTime? ReturnDateTime { get; set; }
        public DateTime? CloseDateTime { get; set; }
        public decimal LoanCostall { get; set; }  //Изначальная сумма займа
        public decimal LoanRestCostall { get; set; } // Остаток по займу
        public double MainPercent { get; set; } //Основной процент по займу
        public double? PeniCostall { get; set; } //Начисленный штраф
        public double? PercentCostall { get; set; } //Размер основного процента
        public int RelisePlaceId { get; set; }//офис где завелся документ
        public int EnterRelisePlaceId { get; set; }//ОФИС ВЫДАЧИ ЗАЙМА
        //public int ACCOUNTID { get; set; } // НАЛИЧКА ИЛИ КАРТА
        //public double? STATETAXCOSTNEED { get; set; } // ГОСПОШЛИНА сколько нужно
        //public double? STATETAXCOST { get; set; } // ГОСПОШЛИНА сколько было оплачено
        //public int DAYSTRAFQUANT { get; set; } //Количество дней просрочки
        //public int POID { get; set; } //ID ПРАВИЛО РАСЧЕТА
        //public short? ISFROST { get; set; }// ЗАМОРОЖЕН ИЛИ НЕТ

        //[Column("STOPISSUD")]
        //public short CourtStatus { get; set; }

        //[Column("MONTHINCOME")]
        //public double? MonthIncome { get; set; }

        //[Column("LOANAIM")]
        //public string LoanAim { get; set; }

        //#region ----Работа----
        //[Column("WORKNAME")]
        //public string WorkName { get; set; }
        //[Column("WORKHEADMEN")]
        //public string WorkHead { get; set; }

        //[Column("WORKADDRESS")]
        //public string WorkAddress { get; set; }

        //[Column("WORKPROF")]
        //public string WorkProf { get; set; }

        //[Column("WORKSTAZH")]
        //public string WorkStazh { get; set; }

        //[Column("PREVIOSWORKNAME")]
        //public string PreviousWork { get; set; }

        //#endregion

        //#region ----------Дополнительно---------------
        //[Column("PUTMONEYQUANT")]
        //public string PutMoneyQuant { get; set; }

        //[Column("LASTPUTMONEYDATE")]
        //public string LastPutMoneyDate { get; set; }

        //[Column("ADDITIONALINCOME")]
        //public string AdditionalIncome { get; set; }

        //[Column("MONTHEXPENSE")]
        //public string MonthExpense { get; set; }

        //[Column("CHILDRENBEFORE21")]
        //public string ChildrenBefore21 { get; set; }

        //[Column("IZHDIVENECLIST")]
        //public string IzhdivenecList { get; set; }

        //[Column("SUPRUGINFO")]
        //public string SuprugInfo { get; set; }

        //[Column("INFORMATIONSOURCE")]
        //public int InformationSourceId { get; set; }
        //[Column("ADULTINFO")]
        //public string ParentInfo { get; set; }

        //[Column("INFO")]
        //public string ManagerInfo { get; set; }

        //#endregion

        //[Column("OLDCREDITS")]
        //public string OldCredits { get; set; }

        //[Column("SOURCEINCOMELOAN")]
        //public string SourceIncome { get; set; }
    }
}
