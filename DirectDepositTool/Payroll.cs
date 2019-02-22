using CsvHelper.Configuration;
using System;

namespace DirectDepositTool
{
    internal class Payroll
    {
        public string name;
        public int? transNum;
        public DateTime date;
        public decimal amount;
        public string account;
        public string memo;
    }

    internal sealed class PayrollMap: ClassMap<Payroll>
    {
        public PayrollMap()
        {
            Map(m => m.name).Name("EMPLOYEE");
            Map(m => m.transNum).Name("NUM").Default(0);
            Map(m => m.date).Name("DATE");
            Map(m => m.amount).Name("AMOUNT");
            Map(m => m.account).Name("ACCOUNT");
            Map(m => m.memo).Name("MEMO");
        }
        private int TryParse(string strVal)
        {
            int.TryParse(strVal, out int result);
            return result;
        }
    }
}