using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using RecipeApi.IService;

namespace RecipeApi.Service
{
   public class UserContentService: IUserContentService
	{
		private readonly IHttpContextAccessor _httpContentAccesorr;
		public UserContentService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContentAccesorr = httpContextAccessor;
		}
		public ClaimsPrincipal User => _httpContentAccesorr.HttpContext?.User;
		public int? GetUserId =>
            User is null ? null : (int)int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
	}
}