public class StripeGateway
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Stripe Payment Successful : {amount}");
    }
}