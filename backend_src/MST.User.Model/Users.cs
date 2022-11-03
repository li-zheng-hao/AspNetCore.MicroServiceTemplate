using FreeSql.DataAnnotations;

namespace MST.User.Model;

[Index("uk_username", "UserName", true)]
[Table(Name = "mst_users")]
public class Users
{
    /// <summary>
    /// 建议使用雪花ID 这里图方便直接用guid自动生成
    /// </summary>
    [Column(IsPrimary = true,Name = "id")]
    public Guid Id { get; set; }

    [Column(Name = "username")] 
    public string UserName { get; set; }

    /// <summary>
    /// 一般需要加密后的密码 这里图方便直接用明文
    /// </summary>
    [Column(Name = "password")]
    public string Password { get; set; }

    [Column(Name = "email")] 
    public string Email { get; set; }
    
    [Column(Name = "phone")] 
    string Phone { get; set; }
    
    [Column(Name = "sex")] 
    public string Sex { get; set; }

    [Column(Name = "age")] 
    public int? Age { get; set; }
}