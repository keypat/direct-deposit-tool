using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace DirectDepositTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IEnumerable<EmployeeBankingInfo> employeeBankingInfo;
        private IEnumerable<Payroll> payrollItems;
        private IEnumerable<Credit> credits;
        
        public MainWindow()
        {
            InitializeComponent();
            ParseFiles();
            MergeData();
            int i = 5;
            string j = i.ToString("D4");
        }

        private void MergeData()
        {
            IEnumerable<Credit> credits = payrollItems.Join(employeeBankingInfo.Where(e=>e.accountNum>0&&e.routingNum>0),
                payroll => payroll.name,
                employee => employee.name,
                (payrollItem, employee) => new Credit()
                {
                    name = payrollItem.name,
                    date = payrollItem.date,
                    accountNum = employee.accountNum.Value,
                    routingNum = employee.routingNum.Value,
                    amount = payrollItem.amount
                }).ToList();
        }

        private void ParseFiles()
        {
            using (var reader = new StreamReader("testEmployees.csv"))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<EmployeeBankingInfoMap>();
                employeeBankingInfo = csv.GetRecords<EmployeeBankingInfo>().ToList();
            }
            using (var reader = new StreamReader("testPayroll.csv"))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<PayrollMap>();
                payrollItems = csv.GetRecords<Payroll>().ToList();
            }
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
