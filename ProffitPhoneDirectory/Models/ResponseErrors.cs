using System.Net;
using System.Text.Json.Serialization;

namespace ProffitPhoneDirectory.Models;

public class ResponseErrors : Exception
{
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; /*private*/ set; }
    [JsonIgnore]
    public bool ToLog { get; set; }
    [JsonIgnore]
    public bool SendToClient { get; internal set; }

    public ResponseErrors( string message, HttpStatusCode statusCode ) : base( message )
    {
        this.StatusCode = statusCode;
    }
}
