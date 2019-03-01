using CsvHelper.Configuration;
using System;

namespace DirectDepositTool
{
    public class Payroll
    {
        public string Name;
        public int? TransNum;
        public DateTime Date;
        public decimal Amount;
        public string Account;
        public string Memo;
    }

    public class PayrollMap: ClassMap<Payroll>
    {
        public PayrollMap()
        {
            Map(m => m.Name).Name("EMPLOYEE").Default(string.Empty);
            Map(m => m.TransNum).Name("NUM").Default(-1);
            Map(m => m.Date).Name("DATE").Default(DateTime.Today);
            Map(m => m.Amount).Name("AMOUNT").Default(0);
            Map(m => m.Account).Name("ACCOUNT").Default(string.Empty);
            Map(m => m.Memo).Name("MEMO").Default(string.Empty);
        }
    }
}