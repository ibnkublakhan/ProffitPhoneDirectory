using ProffitPhoneDirectory._3cxModels;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace ProffitPhoneDirectory.Models;

public class Branch
{
    public int Id { get; set; }                 // 0
    public string? Name { get; set; }           // "ООО ПРОФФИТ КОНСАЛТИНГ"
}
public class BranchOutput
{
    public int Id { get; set; }                 // 0
    public string? Name { get; set; }           // "ООО ПРОФФИТ КОНСАЛТИНГ"
                                                //public List<int> Groups { get; set; }

    public List<int> GroupsId { get; set; } = new List<int>();
}

public class GroupBranch
{
    public Branch? Branch { get; set; }           // "ООО ПРОФФИТ КОНСАЛТИНГ"
    public int GroupsId { get; set; }
}

//public class GroupPhone
//{
//    public int GroupId { get; set; }
//    public List<OuterPhone> OuterPhone { get; set; }
//}

public class PhoneOuterInternal
{

    public int Country { get; set; } // 8
    public int Code { get; set; } // (xxx) | (xxxx)
    public int Value { get; set; } // xxx xx xx | xx xx xx

    public override string? ToString()
    {
        return $"{Country} ({Code}) {Value}";
    }

    public static bool operator ==( PhoneOuterInternal r1, PhoneOuterInternal r2 ) =>
    r1.Country == r2.Country &&
    r1.Code == r2.Code &&
    r1.Value == r2.Value;

    public static bool operator !=( PhoneOuterInternal r1, PhoneOuterInternal r2 ) =>
    !( r1 == r2 );
    public PhoneOuter GetExternal( int groupId ) => new PhoneOuter( groupId, Country, Code, Value );
}

public class PhoneOuter
{
    public PhoneOuter( int groupId, int country, int code, int value )
    {
        GroupId = groupId;
        Country = country;
        Code = code;
        Value = value;
    }

    public Guid Id { get; set; }
    //[JsonIgnore]
    public int GroupId { get; set; }
    public int Country { get; set; } // 8
    public int Code { get; set; } // (xxx) | (xxxx)
    public int Value { get; set; } // xxx xx xx | xx xx xx

    public override string? ToString()
    {
        return $"{Country} ({Code}) {Value}";
    }

    public static bool operator ==( PhoneOuter r1, PhoneOuter r2 ) =>
    r1.Country == r2.Country &&
    r1.Code == r2.Code &&
    r1.Value == r2.Value;

    public static bool operator !=( PhoneOuter r1, PhoneOuter r2 ) =>
    !( r1 == r2 );


}

//public class OuterPhoneInternal
//{
//    public int Country { get; set; } // 8
//    public int Code { get; set; } // (xxx) | (xxxx)
//    public int Value { get; set; } // xxx xx xx | xx xx xx
//}

public class Position
{
    public int Id { get; set; }       // 0
    /// <summary>
    /// Название должности
    /// </summary>
    public string? Name { get; set; } // Главный

    /// <summary>
    /// Цвет в HEX
    /// </summary>
    [DefaultValue( "#ffffff" )]
    public string? Color { get; set; }
}

public class EmployeePosition
{
    public Position Position { get; set; }
    public int EmployeeId { get; set; }
}

public class EmployeeGroupe
{
    public int GroupeId { get; set; }
    public int EmployeeId { get; set; }
}

public class EmployeeOutput
{
    public int Id { get; set; }
    public int ExtentionKey { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
    public string? number { get; set; }
    public string Position { get; set; } = "";
    public string Branch { get; set; } = "0";
    public string groupname { get; set; } = "groupe";
    public string groupnumber { get; set; } = "0";
    public int GroupKey { get; internal set; }

    public EmployeeOutput( User employee )
    {
        Id = employee.Iduser;
        firstname = employee.Firstname;
        lastname = employee.Lastname;
    }
}
