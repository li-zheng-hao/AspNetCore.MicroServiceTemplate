using Microsoft.Extensions.Logging;

namespace MST.Infra.Shared.HttpHandler
{
    public class NacosDiscoverDelegatingHandler : DelegatingHandler
    {
        // private readonly ConsulClient _consulClient;
        private readonly ILogger<NacosDiscoverDelegatingHandler> _logger;

        public NacosDiscoverDelegatingHandler( ILogger<NacosDiscoverDelegatingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var currentUri = request.RequestUri;
            if (currentUri is null)
                throw new NullReferenceException(nameof(request.RequestUri));
            // TODO 这里配置注册发现
            var realRequestUri = new Uri($"{currentUri.Scheme}://localhost:5000{currentUri.PathAndQuery}");
            request.RequestUri = realRequestUri;
            _logger.LogDebug($"RequestUri:{request.RequestUri}");
            // var discoverProvider = new DiscoverProviderBuilder(_consulClient)
            //                                                 .WithCacheSeconds(5)
            //                                                 .WithServiceName(currentUri.Host)
            //                                                 .WithLoadBalancer(TypeLoadBalancer.RandomLoad)
            //                                                 .WithLogger(_logger)
            //                                                 .Build()
            //                                                 ;
            // var baseUri = await discoverProvider.GetSingleHealthServiceAsync();
            // if (baseUri.IsNullOrWhiteSpace())
                // throw new NullReferenceException($"{currentUri.Host} does not contain helath service address!");
            // else
            // {
                // var realRequestUri = new Uri($"{currentUri.Scheme}://{baseUri}{currentUri.PathAndQuery}");
                // request.RequestUri = realRequestUri;
                // _logger.LogDebug($"RequestUri:{request.RequestUri}");
            // }

            try
            {
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogDebug(ex, "Exception during SendAsync()");
                throw;
            }
            finally
            {
                request.RequestUri = currentUri;
            }
        }
    }
}