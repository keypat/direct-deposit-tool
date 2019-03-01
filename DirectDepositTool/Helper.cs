using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using CsvHelper;
using DirectDepositTool.Properties;
using Microsoft.Win32;

namespace DirectDepositTool
{
    internal class Helper
    {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string _outputFileName = @"testOutput.txt";

        public void ProcessInputs(out IEnumerable<Credit> credits, string employeeBankingFileName=null, string payrollFileName=null)
        {
            ParseFiles(out var employeeBankingInfo, out var payrollItems, employeeBankingFileName, payrollFileName);
            MergeData(employeeBankingInfo, payrollItems, out credits);
        }

        public void ParseFiles(out IEnumerable<EmployeeBankingInfo> employeeBankingInfo,
            out IEnumerable<Payroll> payrollItems, string employeeBankingFileName = null, string payrollFileName = null)
        {
            using (var reader = new StreamReader(employeeBankingFileName ?? throw new ArgumentNullException(nameof(employeeBankingFileName))))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<EmployeeBankingInfoMap>();
                employeeBankingInfo = csv.GetRecords<EmployeeBankingInfo>().ToList();
            }
            using (var reader = new StreamReader(payrollFileName ?? throw new ArgumentNullException(nameof(payrollFileName))))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<PayrollMap>();
                payrollItems = csv.GetRecords<Payroll>().ToList();
            }
        }

        public IEnumerable<Credit> MergeData(IEnumerable<EmployeeBankingInfo> employeeBankingInfo, IEnumerable<Payroll> payrollItems, out IEnumerable<Credit> credits)
        {
            credits = payrollItems.Join(employeeBankingInfo,
                    payroll => payroll.Name,
                    employee => employee.name,
                    (payrollItem, employee) => new Credit
                    {
                        name = payrollItem.Name,
                        date = payrollItem.Date,
                        accountNum = employee.accountNum.GetValueOrDefault(),
                        routingNum = employee.routingNum.GetValueOrDefault(),
                        amount = payrollItem.Amount,
                        transNum = payrollItem.TransNum.GetValueOrDefault()
                    })
                .Where(x =>
                {
                    if (x.routingNum >= 0 && x.accountNum >= 0) return true;
                    Log.Warn($"Excluding Transaction {x.transNum} for {x.name} because of missing or invalid banking info.");
                    return false;

                }).ToList();
            return credits;
        }

        public bool ValidateInputs(MainWindow mw)
        {
            return
                int.TryParse(mw.CustomerNumber.Text, out var customerNumber) &&
                int.TryParse(mw.ReturnBranchTransitNum.Text, out var ReturnTransitNumber) &&
                int.TryParse(mw.ReturnAccountNum.Text, out var ReturnAccountNumber);
        }

        public static void SelectSaveFileName(out string fileNameVar)
        {
            var openFileDialog = new SaveFileDialog
            {
                Filter = "Text File | *.txt",
                FileName = $"{Settings.Default.FileCreationNumber}.txt"
            };

            fileNameVar = !openFileDialog.ShowDialog().Value ? $"{Settings.Default.FileCreationNumber}.txt" : openFileDialog.FileName;
        }

        public static void GetFileName(out string fileNameVar)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Comma Separated Value | *.csv"
            };

            fileNameVar = !openFileDialog.ShowDialog().Value ? string.Empty : openFileDialog.FileName;
        }

        public void WriteFile(List<string> rows)
        {
            SelectSaveFileName(out _outputFileName);
            File.WriteAllLines(_outputFileName, rows, Encoding.ASCII);
        }

        public static string GetJulianDateString(DateTime date)
        {
            return "0" + $"{date:yy}" + date.DayOfYear.ToString("D3");
        }

        public static void ShowLogFileAlert(string message = "Open log file?", string title = "Success!")
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Process.Start(@"directdeposittool_log.txt");
                    Environment.Exit(0);
                    break;
                case MessageBoxResult.No:
                    Environment.Exit(0);
                    break;
            }
        }

        public static string TruncateLeft(string str, int maxLength)
        {
            return str.Substring(Math.Max(0, str.Length - maxLength));
        }

        public static void PopulateSavedFields(MainWindow mw)
        {
            var x = (Settings.Default.FileCreationNumber + 1).ToString("D4");
            mw.FileCreationNum.Text = TruncateLeft(x,4);
            mw.ReturnBranchTransitNum.Text = TruncateLeft(Settings.Default.ReturnTransitNumber, 5);
            mw.ReturnAccountNum.Text = TruncateLeft(Settings.Default.ReturnAccountNumber, 12);
            mw.CustomerNumber.Text = TruncateLeft(Settings.Default.CustomerNumber, 10);
            mw.OrigShortName.Text = TruncateLeft(Settings.Default.OrigShortName, 15);
            mw.OrigLongName.Text = TruncateLeft(Settings.Default.OrigLongName, 30);
        }

        public static void SaveChangedFields(MainWindow mw)
        {
            Settings.Default.CustomerNumber = mw.CustomerNumber.Text;
            Settings.Default.OrigShortName = mw.OrigShortName.Text;
            Settings.Default.OrigLongName = mw.OrigLongName.Text;
            Settings.Default.ReturnTransitNumber = mw.ReturnBranchTransitNum.Text;
            Settings.Default.ReturnAccountNumber = mw.ReturnAccountNum.Text;
            Settings.Default.Save();
        }
    }
}
