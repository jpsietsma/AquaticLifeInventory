using AQLI.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AQLI.DataServices.Extensions
{
    public static class ClaimsPrincipalExt
    {
        /// <summary>
        /// Return the WebsiteUser object for the currently logged in user.
        /// </summary>
        public static WebsiteUser GetUserModel(this ClaimsPrincipal _principalObj, UserManager<WebsiteUser> _manager)
        {
            return _manager.GetUserAsync(_principalObj).Result;
        }

        /// <summary>
        /// Return the AquaticLife UserId for the logged in user.
        /// </summary>
        public static string GetAQLUserId(this ClaimsPrincipal _principalObj, UserManager<WebsiteUser> _manager)
        {
            var user = _manager.GetUserAsync(_principalObj).Result;
                        
            return user == null ? "" : user.UserId.ToString();
        }

        /// <summary>
        /// Return the first name of the logged in user.
        /// </summary>
        public static string GetFirstName(this ClaimsPrincipal _principalObj, UserManager<WebsiteUser> _manager)
        {
            if (_principalObj != null)
            {
                return GetUserModel(_principalObj, _manager).FirstName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Return the last name of the logged in user.
        /// </summary>
        public static string GetLastName(this ClaimsPrincipal _principalObj, UserManager<WebsiteUser> _manager)
        {
            if (_principalObj != null)
            {
                return GetUserModel(_principalObj, _manager).LastName;
            }
            else
            {
                return null;
            }
        }
    }
}
