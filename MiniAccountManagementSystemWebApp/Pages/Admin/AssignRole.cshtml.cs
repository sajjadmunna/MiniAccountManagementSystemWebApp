using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAccountManagementSystemWebApp.Pages.Admin
{
    //[Authorize(Roles = "Admin")]
    public class AssignRoleModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AssignRoleModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [BindProperty]
        public string UserEmail { get; set; }

        [BindProperty]
        public string RoleName { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByEmailAsync(UserEmail);
            if (user == null)
            {
                Message = $"❌ No user found with email: {UserEmail}";
                return Page();
            }

            if (!await _roleManager.RoleExistsAsync(RoleName))
            {
                Message = $"❌ Role '{RoleName}' does not exist.";
                return Page();
            }

            var result = await _userManager.AddToRoleAsync(user, RoleName);
            Message = result.Succeeded ? $"✅ Role '{RoleName}' assigned to '{UserEmail}'." : "❌ Failed to assign role.";

            return Page();
        }
        //public void OnGet()
        //{
        //}
    }
}
