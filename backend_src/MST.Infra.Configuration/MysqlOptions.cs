using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace MST.Infra.Configuration;
[Description("Mysql")]
[RegisterService(ServiceLifetime.Singleton)]
public class MysqlOptions
{
    [InjectConfiguration("Mysql:ConnectionString")]
    public string ConnectionString { get; set; }
}