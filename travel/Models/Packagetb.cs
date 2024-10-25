using System;
using System.Collections.Generic;

namespace travel.Models;

public partial class Packagetb
{
    public int PackageId { get; set; }

    public string? PackageCountry { get; set; }

    public int? PackagePerson { get; set; }

    public int? PackageDays { get; set; }

    public int? PackagePrice { get; set; }

    public string? PackageImage { get; set; }

    public DateTime? PackageDate1 { get; set; }

    public DateTime? PackageDate2 { get; set; }

    public DateTime? PackageDate3 { get; set; }

    public string? PackageGuide { get; set; }

    public string? PackageDescription { get; set; }

    public bool? PopularPackage { get; set; }

    public bool? ActivePackage { get; set; }
}
