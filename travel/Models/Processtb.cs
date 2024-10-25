using System;
using System.Collections.Generic;

namespace travel.Models;

public partial class Processtb
{
    public int ProcessId { get; set; }

    public string? ProcessName { get; set; }

    public string? ProcessDescription { get; set; }

    public string? ProcessImage { get; set; }
}
