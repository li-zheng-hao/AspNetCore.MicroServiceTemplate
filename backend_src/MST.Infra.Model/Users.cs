using FreeSql.DataAnnotations;

namespace MST.Infra.Model;

[Index("uk_username", "UserName", true)]
[Index("uk_password", "Password", false)]
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
    /// SHA256加密
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
    
    [Column(Name = "role")] 
    public string Role { get; set; }
}