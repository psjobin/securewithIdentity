using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace secureApp.Pages
{
    [Authorize]
    public class securePageModel : PageModel
    {

        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<IdentityUser> _usermanager;

        public securePageModel(RoleManager<IdentityRole> rolemanager, UserManager<IdentityUser> usermanager)
        {
            _rolemanager = rolemanager;
            _usermanager = usermanager;
        }
        public void OnGet()
        {
        }

        async public Task OnPostNewRole()
        {
            var roleName = Request.Form["roleName"];
            var res = await _rolemanager.CreateAsync(new IdentityRole(roleName));

        }

        async public Task OnPostAddToAdmin()
        {
            var userName = Request.Form["userName"];
            var user = _usermanager.Users.FirstOrDefault();
            await _usermanager.AddToRoleAsync(user, "admin");

        }
    }
}
