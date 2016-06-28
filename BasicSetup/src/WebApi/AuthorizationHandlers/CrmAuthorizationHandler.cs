using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.AuthorizationHandlers
{
    //WORK IN PROGRESS
    //public class CrmAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Customer>
    //{
       
    //    protected override void Handle(AuthorizationContext context, OperationAuthorizationRequirement requirement, Customer resource)
    //    {
    //        if (requirement.Name=="read")
    //        {
    //            if (context.User.HasClaim( "role", "Geek"))
    //            {
    //                context.Succeed(requirement);
    //            }
    //            else
    //            {
    //                context.Fail();
    //            }
                
    //        }
    //    }
    //}
}
