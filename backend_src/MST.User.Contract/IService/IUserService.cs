using MST.User.Model;

namespace MST.User.Contract.IService;

public interface IUserService
{
    Task<UserDto> GetRandomUserAsync();
    Task<Users> AddRandomUserAsync();
    Task<bool> TransactionQueryAndUpdateUserAsync();
}