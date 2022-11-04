using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace MST.Infra.Configuration;
[Description("ElasticSearch")]
[RegisterService(ServiceLifetime.Singleton)]
public class ElasticSearchOptions
{
    [InjectConfiguration("ElasticSearch:Url")]
    public string Url { get; set; }
}