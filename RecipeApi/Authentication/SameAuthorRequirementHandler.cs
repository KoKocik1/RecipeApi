using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RecipeApi.Database;

namespace RecipeApi.Authentication
{
    public class SameAuthorRequirementHandler : AuthorizationHandler<SameAuthorRequirement, Recipe>
    {
        public SameAuthorRequirementHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        SameAuthorRequirement requirement,
        Recipe recipe)
        {
            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (recipe.UserId == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            if (context.User.HasClaim(ClaimTypes.Role, "Admin"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}