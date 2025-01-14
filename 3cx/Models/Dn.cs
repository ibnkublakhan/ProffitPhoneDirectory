using System;
using System.Collections.Generic;

namespace _3cx.Models;

public partial class Dn
{
    public int Iddn { get; set; }

    public int Status { get; set; }

    public string Value { get; set; } = null!;

    public int Fkidtenant { get; set; }

    public int Fkidcalendar { get; set; }

    //public virtual ICollection<ChattemplateCategory> ChattemplateCategories { get; set; } = new List<ChattemplateCategory>();

    //public virtual Conferenceplaceextension? Conferenceplaceextension { get; set; }

    //public virtual Dngrp? Dngrp { get; set; }

    //public virtual ICollection<Dnprop> Dnprops { get; set; } = new List<Dnprop>();

    //public virtual ICollection<Extblf> Extblves { get; set; } = new List<Extblf>();

    //public virtual Extension? Extension { get; set; }

    //public virtual ICollection<Extensionforward> Extensionforwards { get; set; } = new List<Extensionforward>();

    //public virtual Externalline? ExternallineFkiddnNavigation { get; set; }

    //public virtual ICollection<Externalline> ExternallineFkidfaxdnNavigations { get; set; } = new List<Externalline>();

    //public virtual Faxextension? Faxextension { get; set; }

    //public virtual Calendar FkidcalendarNavigation { get; set; } = null!;

    //public virtual Tenant FkidtenantNavigation { get; set; } = null!;

    //public virtual ICollection<Fwdprofile> FwdprofileFkforwardtodnDontmatch1Navigations { get; set; } = new List<Fwdprofile>();

    //public virtual ICollection<Fwdprofile> FwdprofileFkforwardtodnDontmatch2Navigations { get; set; } = new List<Fwdprofile>();

    //public virtual ICollection<Fwdprofile> FwdprofileFkforwardtodnDontmatch3Navigations { get; set; } = new List<Fwdprofile>();

    //public virtual ICollection<Fwdprofile> FwdprofileFkforwardtodnMatch1Navigations { get; set; } = new List<Fwdprofile>();

    //public virtual ICollection<Fwdprofile> FwdprofileFkforwardtodnMatch2Navigations { get; set; } = new List<Fwdprofile>();

    //public virtual ICollection<Fwdprofile> FwdprofileFkforwardtodnMatch3Navigations { get; set; } = new List<Fwdprofile>();

    //public virtual ICollection<Grp> GrpFkdestinationdnNavigations { get; set; } = new List<Grp>();

    //public virtual Grp? GrpFkiddnNavigation { get; set; }

    //public virtual ICollection<Inboundrule> InboundruleFkforwardtodnNavigations { get; set; } = new List<Inboundrule>();

    //public virtual ICollection<Inboundrule> InboundruleHolidayFkforwardtodnNavigations { get; set; } = new List<Inboundrule>();

    //public virtual ICollection<Inboundrule> InboundruleOutofhoursFkforwardtodnNavigations { get; set; } = new List<Inboundrule>();

    //public virtual ICollection<Ivr> IvrFkforwardtodnNavigations { get; set; } = new List<Ivr>();

    //public virtual Ivr? IvrFkiddnNavigation { get; set; }

    //public virtual ICollection<Ivrmenuitem> Ivrmenuitems { get; set; } = new List<Ivrmenuitem>();

    //public virtual Parkextension? Parkextension { get; set; }

    //public virtual ICollection<Phonebook> Phonebooks { get; set; } = new List<Phonebook>();

    //public virtual ICollection<Queue2dn> Queue2dns { get; set; } = new List<Queue2dn>();

    //public virtual ICollection<Queue2managerdn> Queue2managerdns { get; set; } = new List<Queue2managerdn>();

    //public virtual Queue? QueueFkiddnNavigation { get; set; }

    //public virtual ICollection<Queue> QueueFknoanswerdnNavigations { get; set; } = new List<Queue>();

    public virtual ICollection<Ringgroup2dn> Ringgroup2dns { get; set; } = new List<Ringgroup2dn>();

    public virtual Ringgroup? RinggroupFkiddnNavigation { get; set; }

    public virtual ICollection<Ringgroup> RinggroupFknoanswerdnNavigations { get; set; } = new List<Ringgroup>();

    //public virtual Routepoint? Routepoint { get; set; }

    //public virtual Specialmenu? Specialmenu { get; set; }
}
