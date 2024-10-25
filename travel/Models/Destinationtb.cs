using System;
using System.Collections.Generic;

namespace travel.Models;

public partial class Destinationtb
{
    public int DestinationId { get; set; }



    public string? DestinationCountry { get; set; }

    public int? DestinationPrice { get; set; }

    public string? DestinationImage { get; set; }

    public string? DestinationGuider { get; set; }

    public DateTime? DestinationDate1 { get; set; }

    public DateTime? DestinationDate2 { get; set; }

    public DateTime? DestinationDate3 { get; set; }

    public bool? ActiveDestination { get; set; }

    public bool? PopularDestination { get; set; }
}
