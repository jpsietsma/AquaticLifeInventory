using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AQLI.Data.Models;
using AQLI.DataServices.context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AQLI.UI.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<WebsiteUser> _userManager;
        private readonly SignInManager<WebsiteUser> _signInManager;
        private readonly DatabaseContext Database;

        public IndexModel(
            UserManager<WebsiteUser> userManager,
            SignInManager<WebsiteUser> signInManager,
            DatabaseContext _context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Database = _context;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }
        }

        private async Task LoadAsync(WebsiteUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = Database.AspNetUsers.Where(u => u.Id == user.Id).Select(p => p.FirstName).FirstOrDefault();
            var lastName = Database.AspNetUsers.Where(u => u.Id == user.Id).Select(p => p.LastName).FirstOrDefault();

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = Database.AspNetUsers.Select(p => p.FirstName).FirstOrDefault();
            var lastName = Database.AspNetUsers.Where(u => u.UserId == user.UserId).Select(p => p.LastName).FirstOrDefault();

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.FirstName != firstName || Input.LastName != lastName)
            {                
                try
                {
                    user.FirstName = Input.FirstName;
                    user.LastName = Input.LastName;

                    Database.Update(user);
                    await Database.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    StatusMessage = e.Message;
                }
            }          

            if (StatusMessage == null)
            {
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Your profile has been updated";
            }

            return RedirectToPage();
        }
    }
}
