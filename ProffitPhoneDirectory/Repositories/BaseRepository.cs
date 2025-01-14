using Microsoft.AspNetCore.Mvc;
using ProffitPhoneDirectory.Context;

namespace ProffitPhoneDirectory;

[Route( "api/_3cx" )]
public class BaseRepository
{
    protected readonly IConfiguration _configuration;
    protected readonly ProffitCorporateInfoContext proffitContext;
    protected readonly DatabaseSingleContext single_context;
    protected readonly ILogger logger;

    public BaseRepository( ProffitCorporateInfoContext proffitContext, IConfiguration _configuration, DatabaseSingleContext single_context )
    {
        this.single_context = single_context;
        this.proffitContext = proffitContext;
        this._configuration = _configuration;
    }
}
