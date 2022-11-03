using System.ComponentModel;

namespace MST.Infra.Shared.Configuration;
[Description("Mysql")]
public class MysqlOptions
{
    public string ConnectionString { get; set; }
}