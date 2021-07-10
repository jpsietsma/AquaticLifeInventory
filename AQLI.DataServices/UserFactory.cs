using AQLI.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AQLI.DataServices
{
    public class UserFactory
    {
        private readonly DataFactory Database;
        private readonly UserManager<WebsiteUser> UserManager;

        public UserFactory(DataFactory _dataFactory, UserManager<WebsiteUser> _userManager)
        {
            Database = _dataFactory;
            UserManager = _userManager;
        }

        /// <summary>
        /// Get the current logged in user
        /// </summary>
        /// <param name="_userClaim">Claim Principal of the user to filter on</param>
        public async Task<WebsiteUser> Find_LoggedInUser(ClaimsPrincipal _userClaim)
        {
            return await UserManager.GetUserAsync(_userClaim);
        }
    }
}
