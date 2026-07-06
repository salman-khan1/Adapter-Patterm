public class PaymentAdapter : IPaymentGateway
{
    private readonly LegacyPaymentGateway _legacyGateway;

    public PaymentAdapter()
    {
        _legacyGateway = new LegacyPaymentGateway();
    }

    public void Pay(decimal amount)
    {
        _legacyGateway.MakePayment((double)amount);
    }
}