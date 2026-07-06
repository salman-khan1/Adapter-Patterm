class Program
{
    static void Main()
    {
        IPaymentGateway payment = new PaymentAdapter();

        payment.Pay(5000);

        IPaymentGateway pay = new StripeAdapter();
        pay.Pay(5090);
        Console.ReadLine();
    }
}