using System.ComponentModel;

namespace MST.Infra.Shared.Configuration;
[Description("RabbitMQ")]
public class RabbitMQOptions
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password{ get; set; }
    public string VirtualHost{ get; set; }
}