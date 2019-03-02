using CsvHelper.Configuration;
using System;

namespace DirectDepositTool
{
    public class Payroll
    {
        public string Name;
        public long TransNum;
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
            Map(m => m.TransNum).Name("NUM").Default(-1L);
            Map(m => m.Date).Name("DATE").Default(DateTime.Today.AddYears(1));
            Map(m => m.Amount).Name("AMOUNT").Default(0.0m);
            Map(m => m.Account).Name("ACCOUNT").Default(string.Empty);
            Map(m => m.Memo).Name("MEMO").Default(string.Empty);
        }
    }
}