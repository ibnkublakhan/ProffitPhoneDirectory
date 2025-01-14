using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _3cx.Models;

public partial class Phonedirectory
{
    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Number { get; set; }

    public string? Groupname { get; set; }

    public string? Groupnumber { get; set; }
}
