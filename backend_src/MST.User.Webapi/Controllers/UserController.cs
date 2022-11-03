using FreeSql;
using Microsoft.AspNetCore.Mvc;
using MST.Infra.Shared.Contract.HttpResponse;
using MST.User.Contract;
using MST.User.Contract.IRepository;
using MST.User.Contract.IService;
using MST.User.Model;

namespace MST.User.Webapi.Controllers;

[ApiController]
[Route("user/[controller]/[action]")]
public class UserController:ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger,IUserService userService)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// 随机获取一个用户
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ResponseResult> GetRandomUser()
    {
        UserDto user=await _userService.GetRandomUserAsync();
        return ResponseResult.Success(user);
    }
    /// <summary>
    /// 随机新增一个用户
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ResponseResult> AddRandomUser()
    {
        Users user=await _userService.AddRandomUserAsync();
        return ResponseResult.Success(user);
    }
    /// <summary>
    /// 测试事务更新用户信息
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<ResponseResult> TransactionQueryAndUpdateUser()
    {
        bool res=await _userService.TransactionQueryAndUpdateUserAsync();
        return res ? ResponseResult.Success("") : ResponseResult.Failure("");
    }
}