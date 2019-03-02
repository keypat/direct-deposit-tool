using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using CsvHelper.Configuration;

namespace DirectDepositTool
{
    public class EmployeeBankingInfo
    {
        public string Name;
        public string AccountNum;
        public long RoutingNum;
        public string AccountType;
    }

    internal sealed class EmployeeBankingInfoMap : ClassMap<EmployeeBankingInfo>
    {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public EmployeeBankingInfoMap()
        {
            Map(m => m.Name).Name("Employee").Default(string.Empty);
            Map(m => m.AccountNum).Name("Bank Account Number").Default(string.Empty);
            Map(m => m.RoutingNum).Name("ABA Routing Number").Default(-1L);
            Map(m => m.AccountType).Name("CHK/SAV").Default(string.Empty);
        }
    }
}
