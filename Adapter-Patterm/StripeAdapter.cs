public class StripeAdapter : IPaymentGateway
{
    private readonly StripeGateway _stripe;

    public void Pay(decimal amount)
    {
        _stripe.ProcessPayment(amount);
    }
}