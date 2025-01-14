using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace ProffitPhoneDirectory.Controllers;

[ApiController]
public abstract class RootController : ControllerBase
{
    protected readonly IConfiguration configuration;
    protected readonly ILogger logger;

    protected RootController( IConfiguration configuration )
    {
        this.configuration = configuration;
    }

    protected RootController( IConfiguration configuration, ILogger logger ) : this( configuration ) 
    {
        this.logger = logger;
    }
}
