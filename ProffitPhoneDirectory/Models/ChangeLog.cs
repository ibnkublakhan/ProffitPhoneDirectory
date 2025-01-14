namespace ProffitPhoneDirectory.Models;

public class LogInternalBase
{
    public DateTime ChangeDate { get; set; }
    public string EntityName { get; set; }
    public string Action { get; set; }
    public string PrevData { get; set; }
    public string NewData { get; set; }
    //public DateTime ChangeDate { get; set; }
    public string User { get; set; }
    public string Sourse { get; set; }
}

public class ChangeLog 
{
    public Guid Id { get; set; }
    public DateTime ChangeDate { get; set; }
    public string EntityName { get; set; }
    public string Action { get; set; }
    public string PrevData { get; set; }
    public string NewData { get; set; }
    //public DateTime ChangeDate { get; set; }
    public string User { get; set; }
}

public partial class ChangeLogInternal : LogInternalBase
{
    public Guid Id { get; set; }

    public ChangeLogInternal(  )
    {
        Sourse = "proffit";
    }

    public ChangeLogInternal( ChangeLog auditLog ) : this()
    {
        Id = auditLog.Id;
        EntityName = auditLog.EntityName ?? "";
        Action = auditLog.Action.ToString();
        PrevData = auditLog.PrevData ?? "";
        NewData = auditLog.NewData ?? "";
        ChangeDate = auditLog.ChangeDate.ToLocalTime();
        User = auditLog.User ?? "";
    }
}
