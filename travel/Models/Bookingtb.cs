using System;
using System.Collections.Generic;

namespace travel.Models;

public partial class Bookingtb
{
    public int BookId { get; set; }

   

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public int? PhoneNumber { get; set; }

    public string? Destination { get; set; }

    public DateTime? DateTime { get; set; }

    public int? TotalPrice { get; set; }

    public string? Guider { get; set; }

    public int? Days { get; set; }

    public int? Person { get; set; }

    public string? Message { get; set; }

    public string? PaymentMethod { get; set; }

    public int? CreditCardNumber { get; set; }

    public string? Status { get; set; }

    public string UserId { get; set;}
}
