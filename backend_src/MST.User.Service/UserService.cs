using AspNetCore.StartUpTemplate.Core;
using FreeSql;
using Microsoft.Extensions.Logging;
using MST.User.Contract;
using MST.User.Contract.IRepository;
using MST.User.Contract.IService;
using MST.User.Model;
using Quickwire.Attributes;

namespace MST.User.Service;

[InjectAllInitOnlyProperties]
[RegisterService(ServiceType = typeof(IUserService))]
public class UserService:IUserService
{
    private readonly IBaseRepository<Users> _userRepository;
    public ILogger<UserService> _logger { get; init; }
    public UserService(IBaseRepository<Users> userRepository)
    {
        _userRepository = userRepository;
    }
    // 关于直接return task和await的区别 https://stackoverflow.com/questions/38017016/async-task-then-await-task-vs-task-then-return-task 
    public Task<UserDto> GetRandomUserAsync() => _userRepository.Where(it => true).FirstAsync<UserDto>();
    public Task<Users> AddRandomUserAsync()
    {
        var user = new Users()
            { Age = 1, Email = "1233123@qq.com", Password = "123", Sex = "男", UserName = Guid.NewGuid().ToString() };
        return _userRepository.InsertAsync(user);
    }
    [Transactional]
    public async Task<bool> TransactionQueryAndUpdateUserAsync()
    {
        await InternalUpdateAsync();
        throw new Exception("测试抛出异常后是否会回滚");
    }
    [Transactional]
    private async Task<bool> InternalUpdateAsync()
    {
        var user=await _userRepository.Where(it => true).FirstAsync<Users>();
        _logger.LogInformation($"尝试更新的用户id {user.Id} 用户的当前年龄{user.Age} +1");
        user.Age += 1;
        return ( await _userRepository.UpdateAsync(user))>0;
    }
}