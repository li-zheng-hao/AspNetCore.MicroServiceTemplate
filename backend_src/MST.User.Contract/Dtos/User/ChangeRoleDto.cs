using System.ComponentModel.DataAnnotations;
using MST.Infra.Utility.Validation;
using MST.Infra.Utility.Validator;
using MST.User.Core.Consts;

namespace MST.User.Contract;

public class ChangeRoleDto
{
    [Required]
    public string UserName { get; set; }
    [StringRange(UserRole.Admin,UserRole.User,UserRole.Guest)]
    public string Role { get; set; }
}