using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace DirectDepositTool
{
    public partial class MainWindow
    {
        private IEnumerable<Credit> _credits;
        private string _payrollFileName = @"testPayroll.csv";
        private string _employeeBankingFileName = @"testEmployees.csv";
        private readonly Helper _helper = new Helper();
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow()
        {
            InitializeComponent();
            Helper.PopulateSavedFields(this);
        }


        private void BtnPayrollFile_Click(object sender, RoutedEventArgs e)
        {
            Helper.GetFileName(out _payrollFileName);
        }

        private void BtnEmployeeFile_Click(object sender, RoutedEventArgs e)
        {
            Helper.GetFileName(out _employeeBankingFileName);
        }

        private void BtnCreateFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _helper.ProcessInputs(out _credits, _employeeBankingFileName, _payrollFileName);

                var scotiaFileWriter = new Scotia105Helper();
                scotiaFileWriter.CreateFile(_credits, this);
                Helper.SaveChangedFields(this);
                Helper.ShowLogFileAlert();
            }
            catch(Exception ex)
            {
                Log.Error(ex.StackTrace);
                Helper.ShowLogFileAlert("Error in program. Open log file?", "ERROR");
            }

        }

    }
}
