using System;
using System.Collections.Generic;

namespace travel.Models;

public partial class Mailtb
{
    public int MailId { get; set; }

    public string? MailName { get; set; }

    public string? MailEmail { get; set; }

    public string? MailSubject { get; set; }

    public string? MailMessage { get; set; }
}
