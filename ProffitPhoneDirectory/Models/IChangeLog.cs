namespace ProffitPhoneDirectory.Models
{
    public interface IChangeLog
    {
        string Action { get; set; }
        DateTime ChangeDate { get; set; }
        string EntityName { get; set; }
        object Id { get; set; }
        string NewData { get; set; }
        string PrevData { get; set; }
        string User { get; set; }
    }
}