﻿using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;

namespace MST.Auth.Webapi;




public class ProfileService : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        try
        {
            //depending on the scope accessing the user data.
            var claims = context.Subject.Claims.ToList();

            //set issued claims to return
            context.IssuedClaims = claims.ToList();
        }
        catch (Exception ex)
        {
            //log your error
        }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
    }
}