using MST.Infra.Model;

namespace MST.User.Contract.IService;

public interface IUserService
{
    #region 测试代码
    Task<UserDto> GetRandomUserAsync();
    Task<Users> AddRandomUserAsync();
    Task<bool> TransactionQueryAndUpdateUserAsync();
    Task<Users> AddUser(string username, string password);
    #endregion
    
    Task<Users> AddUser(Users user);
    /// <summary>
    /// 更新用户角色
    /// </summary>
    /// <param name="changeRoleDto"></param>
    /// <returns></returns>
    Task<bool> ChangeUserRole(ChangeRoleDto changeRoleDto);
}