namespace ProffitPhoneDirectory._3cxModels;

public partial class User
{
    public int Iduser { get; set; }
    public int Fkidextension { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Fwduser { get; set; }
    public string? Sipid { get; set; }
    public string? Selfidprompt { get; set; }
}
