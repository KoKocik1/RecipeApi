using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.IService
{
    public interface IEmailService
    {
        Task SendAsync(string from, string to, string subject, string body);
        
    }
}