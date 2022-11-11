using System.ComponentModel;

namespace MST.User.Core.Consts;

public class UserRole
{
    [Description("管理员")]
    public const string Admin="Admin";
    [Description("普通用户")]
    public const string User="User";
    [Description("游客")]
    public const string Guest="Guest";

}