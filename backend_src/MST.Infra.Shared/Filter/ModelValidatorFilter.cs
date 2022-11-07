﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MST.Infra.Shared.Contract.HttpResponse;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetCore.StartUpTemplate.Filter;

/// <summary>
/// 请求验证错误处理
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public class ModelValidatorFilter : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext actionContext)
    {
        var modelState = actionContext.ModelState;
        if (!modelState.IsValid)
        {
            var baseResult = new HttpResponseResult()
            {
                success = false,
                code = ResponseCode.PARAM_ERROR,
                msg = "请提交必要的参数",
            };
            List<string> errors = new List<string>();
            foreach (var key in modelState.Keys)
            {
                var state = modelState[key];
                if (state != null && state.Errors.Any())
                {
                    errors.Add( $"{key} -  { state.Errors.FirstOrDefault()?.ErrorMessage}");
                }
            }
            baseResult.data = errors;
            actionContext.Result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(baseResult),
                ContentType = "application/json"
            };
        }
    }
}
