using CsvHelper.Configuration;
using System;
using System.Globalization;
using CsvHelper.TypeConversion;

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
            Map(m => m.Name).Name("Employee").Default(string.Empty);
            Map(m => m.TransNum).Name("Num").Default(-1L);
            Map(m => m.Date).Name("Date").Default(DateTime.Today.AddYears(1));
            Map(m => m.Amount).Name("Amount").Default(0.0m).TypeConverterOption.NumberStyles(NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands);
            Map(m => m.Account).Name("Account").Default(string.Empty);
            Map(m => m.Memo).Name("Memo").Default(string.Empty);
        }
    }
}