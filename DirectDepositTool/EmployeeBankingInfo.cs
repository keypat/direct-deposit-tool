using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDepositTool
{
    internal class EmployeeBankingInfo
    {
        public string name;
        public int? accountNum;
        public int? routingNum;
        public string accountType;
    }

    internal sealed class EmployeeBankingInfoMap : ClassMap<EmployeeBankingInfo>
    {
        public EmployeeBankingInfoMap()
        {
            Map(m => m.name).Name("Employee");
            Map(m => m.accountNum).Name("Bank Account Number").Default(-1);
            Map(m => m.routingNum).Name("ABA Routing Number").Default(-1);
            Map(m => m.accountType).Name("CHK/SAV").Default(string.Empty);
        }
    }
}
