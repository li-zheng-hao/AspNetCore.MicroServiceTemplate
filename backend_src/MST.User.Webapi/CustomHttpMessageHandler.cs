namespace MST.User.Webapi;
// Handler A
public class HandlerA : DelegatingHandler
{
    private readonly ILogger<HandlerA> _logger;
    public HandlerA(ILogger<HandlerA> logger) { _logger = logger; }
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("This is Handler A Before");
        var res=await base.SendAsync(request, cancellationToken);
        _logger.LogInformation("This is Handler A After");
        return res;
    }
}

// Handler B
public class HandlerB : DelegatingHandler
{
    private readonly ILogger<HandlerB> _logger;
    public HandlerB(ILogger<HandlerB> logger) { _logger = logger; }
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("This is Handler B Before");
        var res=await  base.SendAsync(request, cancellationToken);
        _logger.LogInformation("This is Handler B After");
        return res;
    }
}