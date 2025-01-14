using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _3cx.Models;

public partial class User
{
    [Key]
    public int Iduser { get; set; }

    public int Fkidextension { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Fwduser { get; set; }

    public string? Sipid { get; set; }

    public string? Selfidprompt { get; set; }

    //public virtual Extension FkidextensionNavigation { get; set; } = null!;

    //public virtual Voicemail? Voicemail { get; set; }
}
