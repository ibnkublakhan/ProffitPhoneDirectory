using System;
using System.Collections.Generic;

namespace ProffitPhoneDirectory._3cxModels;

public partial class Dn
{
    public int Iddn { get; set; }

    public int Status { get; set; }

    public string Value { get; set; } = null!;

    public int Fkidtenant { get; set; }

    public int Fkidcalendar { get; set; }

    public virtual ICollection<Ringgroup2dn> Ringgroup2dns { get; set; } = new List<Ringgroup2dn>();

    public virtual Ringgroup? RinggroupFkiddnNavigation { get; set; }

    public virtual ICollection<Ringgroup> RinggroupFknoanswerdnNavigations { get; set; } = new List<Ringgroup>();
}
