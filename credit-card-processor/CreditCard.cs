namespace credit_card_processor
{
    public class CreditCard
    {
        //card number
        public string? CardNumber { get; set; }
        //card type
        public string? CardType { get; set;}

        //card expiry
        public string? CardExpiry { get; set; }

        //card cvv
        public string? CardCVV { get; set; }

        //authorisation amount
        public string? Amount { get; set; }

    }
}