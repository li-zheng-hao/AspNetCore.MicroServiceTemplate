using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace MST.Infra.Configuration;
[Description("RabbitMQ")]
[RegisterService(ServiceLifetime.Singleton)]
public class RabbitMQOptions
{
    [InjectConfiguration("RabbitMQ:Host")]
    public string Host { get; set; }
    [InjectConfiguration("RabbitMQ:Port")]
    public int Port { get; set; }
    [InjectConfiguration("RabbitMQ:UserName")]
    public string UserName { get; set; }
    [InjectConfiguration("RabbitMQ:Password")]
    public string Password{ get; set; }
    [InjectConfiguration("RabbitMQ:VirtualHost")]
    public string VirtualHost{ get; set; }
}