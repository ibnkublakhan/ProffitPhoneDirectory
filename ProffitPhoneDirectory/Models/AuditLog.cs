using System;
using System.Collections.Generic;
using System.Net;

namespace ProffitPhoneDirectory.Models;

public partial class AuditLog
{
    public int Id { get; set; }

    public DateTime TimeStamp { get; set; }

    public short Source { get; set; }

    public IPAddress Ip { get; set; } = null!;

    public short Action { get; set; }

    public short ObjectType { get; set; }

    public string UserName { get; set; } = null!;

    public string? ObjectName { get; set; }

    public string? PrevData { get; set; }

    public string? NewData { get; set; }
}
public partial class _AuditLog
{
    public int Id { get; set; }

    public DateTime TimeStamp { get; set; }

    //public short Source { get; set; }

    //public IPAddress Ip { get; set; } = null!;

    public short Action { get; set; }

    //public short ObjectType { get; set; }

    //public string UserName { get; set; } = null!;

    public string? ObjectName { get; set; }

    //public string? PrevData { get; set; }

    //public string? NewData { get; set; }
}
