using System;
using System.Collections.Generic;

namespace ProffitPhoneDirectory._3cxModels;

public partial class Ringgroup
{
    public int Fkiddn { get; set; }

    public int? Fknoanswerdn { get; set; }

    public int Noanswertype { get; set; }

    public string Noanswertoout { get; set; } = null!;

    public string? Name { get; set; }

    public int Ringstrategy { get; set; }

    public int? Ringtime { get; set; }

    public string? Cidprefix { get; set; }

    public virtual Dn FkiddnNavigation { get; set; } = null!;

    public virtual Dn? FknoanswerdnNavigation { get; set; }

    public virtual ICollection<Ringgroup2dn> Ringgroup2dns { get; set; } = new List<Ringgroup2dn>();
}
