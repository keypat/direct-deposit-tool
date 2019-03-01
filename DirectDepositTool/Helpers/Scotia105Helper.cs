using System;
using System.Collections.Generic;
using DirectDepositTool.Properties;

namespace DirectDepositTool
{
    internal class Scotia105Helper : Helper
    {
        private int _creditTotalItems;
        private decimal _creditTotalValue;
        private readonly List<string> _rows = new List<string>();

        public override void GenerateFileFromInputs(string employeeBankingFileName, string payrollFileName, MainWindow mw)
        {
            ProcessInputs(out var credits, employeeBankingFileName, payrollFileName);
            CreateFile(credits, mw);
        }

        public void CreateFile(IEnumerable<Credit> credits, MainWindow mw)
        {

            UpdateFcn(mw);
            CreateARecord(mw);
            CreateYRecord(mw);
            foreach (var c in credits)
            {
                try
                {
                    CreateCRecord(c);
                }
                catch
                {
                    Log.Error($"Error for transaction {c.transNum} for {c.name}");
                }

            }
            CreateZRecord(mw);

            WriteFile(_rows);

        }

        public void ValidateAndAddRow(string row)
        {
            if (row.Length == 105)
            {
                _rows.Add(row);
            }
            else
            {
                Log.Error($"Record Length is not 105 for the following record: \n{row}");
                throw new Exception();
            }
        }

        private void UpdateFcn(MainWindow mw)
        {
            var fcn = Settings.Default.FileCreationNumber;
            if (fcn > 9999)
            {
                fcn = 0;
                Settings.Default.FileCreationNumber = 0;
            }
            else
            {
                Settings.Default.FileCreationNumber++;
            }
            if (int.TryParse(mw.FileCreationNum.Text, out var newVal) && newVal != fcn && newVal < 10000 && newVal > 0)
            {
                fcn = newVal;
            }

            mw.FileCreationNum.Text = fcn.ToString("D4");
            Settings.Default.Save();
        }

        private void CreateARecord(MainWindow mw)
        {
            var row = "A000000001" + mw.CustomerNumber.Text.PadLeft(10, '0') +
                      "00220D" + Settings.Default.FileCreationNumber.ToString("D4") + GetJulianDateString(DateTime.Today) +
                      string.Empty.PadRight(69);

            ValidateAndAddRow(row);
        }

        private void CreateYRecord(MainWindow mw)
        {
            var row = "Y" + mw.OrigShortName.Text.PadRight(15) + mw.OrigLongName.Text.PadRight(30) +
                      "002" + mw.ReturnBranchTransitNum.Text + mw.ReturnAccountNum.Text.PadLeft(12, '0') +
                      string.Empty.PadRight(39);

            ValidateAndAddRow(row);
        }

        private void CreateCRecord(Credit c)
        {
            var row = "C200" + $"{c.amount * 100:0}".PadLeft(10, '0') +
                      GetJulianDateString(c.date) + c.routingNum.ToString("D8") +
                      c.accountNum.ToString("D12") + c.name.PadRight(30) +
                      c.transNum.ToString().PadRight(19) + c.transNum.ToString().PadRight(16);

            ValidateAndAddRow(row);
            _creditTotalItems++;
            _creditTotalValue += c.amount;
            Log.Info($"Transaction {c.transNum} for {c.name} in the amount of {c.amount:C} has been written successfully");
        }

        private void CreateZRecord(MainWindow mw)
        {
            var row = "Z".PadRight(10) + mw.CustomerNumber.Text.PadLeft(10, '0') +
                      Settings.Default.FileCreationNumber.ToString("D4") + 0.ToString("D22") +
                      $"{_creditTotalValue:0}".PadLeft(14, '0') +
                      _creditTotalItems.ToString("D8") +
                      string.Empty.PadRight(37);

            ValidateAndAddRow(row);
        }

    }
}
