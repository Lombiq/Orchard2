using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Orchard.Users.Models;
using Orchard.Setup.Events;

namespace Orchard.Users.Services
{
    /// <summary>
    /// During setup, creates the admin user account.
    /// </summary>
    public class SetupEventHandler : ISetupEventHandler
    {
        private readonly IUserService _userService;

        public SetupEventHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task Setup(
            string siteName,
            string userName,
            string email,
            string password,
            string dbProvider,
            string dbConnectionString,
            string dbTablePrefix,
            Action<string, string> reportError
            )
        {
            return _userService.CreateUserAsync(userName, email, new string [] { "Administrator" }, password, reportError);
        }
    }
}