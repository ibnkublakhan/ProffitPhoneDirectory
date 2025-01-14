using ProffitPhoneDirectory._3cxModels;
using System.ComponentModel.DataAnnotations;

namespace ProffitPhoneDirectory.Models;

public partial class Employee
{
    //public Employee( User user )
    //{
    //    Id = user.Iduser;
    //    extentionKey = user.Fkidextension;
    //    firstname = user.Firstname;
    //    lastname = user.Lastname;
    //    //number = user.

    //}

    public int Id { get; set; }
    public int extentionKey { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
    public string? number { get; set; }
    //public string? groupname { get; set; } = "";
    //public string? groupnumber { get; set; }
}


public partial class EmployeeGroup
{
    //[Key]
    public int  Id { get; set; }
    public string? groupname { get; set; }   
}

//public partial class Employee
//{
//    public int Id { get; set; }
//    public string? firstname { get; set; }
//    public string? lastname { get; set; }
//    public string? number { get; set; }
//    public string? groupname { get; set; }
//    public string? groupnumber { get; set; }
//}

public partial class ProffitGroup
{
    public int Id { get; set; }
    public string? groupnumber { get; set; }
    public string? groupname { get; set; }
    public List<PhoneOuter> phones { get; set; }

    public ProffitGroup() { }
    public ProffitGroup( Ringgroup ringgroup) 
    {
        Id = ringgroup.Fkiddn;
        groupname = ringgroup.Name;
        groupnumber = ringgroup.FkiddnNavigation.Value;
    }
}
