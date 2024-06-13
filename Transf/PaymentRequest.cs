namespace Transf;

public class PaymentRequest
{
    public string CustomerIp { get; set; }
    public string MerchantPosId { get; set; }
    public string Description { get; set; }
    public string CurrencyCode { get; set; }
    public int TotalAmount { get; set; }
    public Buyer Buyer { get; set; }
    public Product[] Products { get; set; }
}

public class Buyer
{
    public string Email { get; set; }
    public string Phone { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class Product
{
    public string Name { get; set; }
    public string UnitPrice { get; set; }
    public int Quantity { get; set; }
}
