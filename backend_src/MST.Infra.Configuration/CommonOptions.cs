using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace MST.Infra.Configuration;

/// <summary>
/// 一些未分类的通用配置
/// </summary>
[Description("Common")]
[RegisterService(ServiceLifetime.Singleton)]
public class CommonOptions
{
    /// <summary>
    /// 环境配置 如 开发环境 测试环境 生产环境
    /// </summary>
    [InjectConfiguration("Common:Env")]
    public string Env { get; set; }
}