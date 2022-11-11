using System.ComponentModel.DataAnnotations;
using Masuit.Tools.Core.Validator;
using MST.Infra.Utility.Validation;
using MST.Infra.Utility.Validator;

namespace MST.User.Contract;

public class CreateUserDto
{
    [Required]
    public string UserName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    
    [Phone]
    public string Phone { get; set; }
    [ComplexPassword(6,18,MustLetter = true,MustNumber = true)]
    public string Password { get; set; }
    [Compare(nameof(Password),ErrorMessage = "两次输入密码要一致")]
    public string ConfirmPassword { get; set; }
    
    public string Sex { get; set; }
    
    public int Age { get; set; }
    /// <summary>
    /// 头像base64编码
    /// </summary>
    [StringMaxBytes(500)]
    public string Icon { get; set; }
}