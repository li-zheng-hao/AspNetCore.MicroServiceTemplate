using System.ComponentModel;

namespace MST.Infra.Shared.Configuration;
[Description("ElasticSearch")]
public class ElasticSearchOptions
{
    public string Url { get; set; }
}