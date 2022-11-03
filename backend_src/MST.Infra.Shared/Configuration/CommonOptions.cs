using System.ComponentModel;

namespace MST.Infra.Shared.Configuration;

/// <summary>
/// 一些未分类的通用配置
/// </summary>
[Description("Common")]
public class CommonOptions
{
    /// <summary>
    /// 环境配置 如 开发环境 测试环境 生产环境
    /// </summary>
    public string Env { get; set; }
}