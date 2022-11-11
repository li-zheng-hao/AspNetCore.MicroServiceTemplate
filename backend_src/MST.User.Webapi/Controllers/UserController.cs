﻿using FreeSql;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MST.Infra.Configuration;
using MST.Infra.Model;
using MST.Infra.Shared.Contract.HttpResponse;
using MST.User.Contract;
using MST.User.Contract.IRepository;
using MST.User.Contract.IService;
using MST.User.Core.Consts;

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
    public async Task<HttpResponseResult> GetRandomUser()
    {
        UserDto user=await _userService.GetRandomUserAsync();
        return HttpResponseResult.Success(user);
    }
    /// <summary>
    /// 随机新增一个用户
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<HttpResponseResult> AddRandomUser()
    {
        Users user=await _userService.AddRandomUserAsync();
        return HttpResponseResult.Success(user);
    }
    /// <summary>
    /// 新增指定账号密码的用户
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<HttpResponseResult> AddUser(string username,string password)
    {
        Users user=await _userService.AddUser(username,password);
        return HttpResponseResult.Success(user);
    }
    /// <summary>
    /// 测试事务更新用户信息
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<HttpResponseResult> TransactionQueryAndUpdateUser()
    {
        bool res=await _userService.TransactionQueryAndUpdateUserAsync();
        return res ? HttpResponseResult.Success("") : HttpResponseResult.Failure("");
    }
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="userDto"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<HttpResponseResult> CreateUser(CreateUserDto userDto)
    {
        var model=userDto.Adapt<Users>();
        var res=await _userService.AddUser(model);
        return HttpResponseResult.Success(res.Adapt<CreateUserRespDto>());
    }
    /// <summary>
    /// 更改用户权限
    /// </summary>
    /// <param name="changeRoleDto"></param>
    /// <returns></returns>
    [Authorize(UserRole.Admin)]
    [HttpPost]
    public async Task<HttpResponseResult> ChangeUserRole(ChangeRoleDto changeRoleDto)
    {
        bool changed=await _userService.ChangeUserRole(changeRoleDto);
        if (changed)
            return HttpResponseResult.Success();
        else
            return HttpResponseResult.Failure("未更新任何数据");
    }
}