namespace Altairis.ShirtShop.Web {
    public class ShopConfig {

    public PaymentConfig Payment { get; set; }

    public MailingConfig Mailing { get; set; }

    public class PaymentConfig {

        public string AccountNumber { get; set; }

        public string VarSymbolFormat { get; set; }

    }

    public class MailingConfig {

        public string PickupFolder { get; set; }

        public string SenderAddress { get; set; }

        public string OrderRecipientAddress { get; set; }

    }

}
}
