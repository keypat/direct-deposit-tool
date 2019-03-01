# direct-deposit-tool
## generate direct deposit files from quickbooks payroll and employee reports.
## The current version only supports converting to Scotia Direct EFT 105 Byte Transmission File Layout
![](https://github.com/keypat/direct-deposit-tool/blob/master/GUI.PNG)

|Field|Description|
|-----|-----------|
|File Creation Number|AutoIncremented. You can change it if you feel the need to, but it will not update the global value. (The auto increment will increment from the saved value next time)|
|Customer Number|Positive integer. 10 digits|
|Originator Short Name|Alphanumeric. Max 15 characters|
|Originator Long Name|Alphanumeric. Max 30 characters|
|Return Branch Transit Number|Positive integer. 5 digits|
|Return Branch Account Number|Positive integer. Max 12 digits|
|Payroll File Button|Select .csv file with payroll information [**Refer To Appendix A**](https://github.com/keypat/direct-deposit-tool/blob/master/README.md#appendix-a)|
|Employee File Button|Select .csv file with employee banking information [**Refer To Appendix A**](https://github.com/keypat/direct-deposit-tool/blob/master/README.md#appendix-a)|
|Create File Button|You will get a prompt to save the file to a location of your choosing|
#### You will get a pop up after execution asking whether you would like to view the log file.


## Appendix A
### Payroll .csv file format example
|EMPLOYEE|NUM|DATE|AMOUNT|ACCOUNT|MEMO|
|--------|---|----|------|-------|----|
|Fake Name|1234|2019-02-15|123.45|Bank of Nova Scotia 12345|Test memo|

### Employee banking information .csv file format example
|Employee|Bank Account Number|ABA Routing Number|CHK/SAV|
|--------|-------------------|------------------|-------|
|Fake Name|1234567|032156788|chk|
