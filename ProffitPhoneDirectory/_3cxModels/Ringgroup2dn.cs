using System;
using System.Collections.Generic;

namespace ProffitPhoneDirectory._3cxModels;

public partial class Ringgroup2dn
{
    public int Fkidringgroup { get; set; }

    public int Fkiddn { get; set; }

    public int Priority { get; set; }

    public int Idringgroup2dn { get; set; }

    public virtual Dn FkiddnNavigation { get; set; } = null!;

    public virtual Ringgroup FkidringgroupNavigation { get; set; } = null!;
}
