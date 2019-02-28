namespace DirectDepositTool
{
    public class PaymentOriginator
    {
        public int CustomerNumber { get; set; }
        public int ReturnTransitNumber { get; set; }
        public int ReturnAccountNumber { get; set; }
        public string OriginatorShortName { get; set; }
        public string OriginatorLongName { get; set; }
    }
}
