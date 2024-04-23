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
            // There have access admins and login users, so I need to check both
            // If I add [Authorization(roles="admin, user")] I won't know if the user is an admin or not
            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            // Check if the user is the author of the recipe
            if (recipe.UserId == userId)
            {
                context.Succeed(requirement);
            }

            // Check if the user is an admin
            if (context.User.HasClaim(ClaimTypes.Role, "Admin"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}