
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCC.Models;
using TCC.Models.ViewModels;

namespace TCC.Controllers {
    public class AdministrationController : Controller{

        private readonly RoleManager<IdentityRole> RoleManager;

        public UserManager<Usuario> UserManager { get; }

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<Usuario> userManager) {
            RoleManager = roleManager;
            UserManager = userManager;
        }

        public IActionResult CreateRole() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRoleViewModel) {
            if (ModelState.IsValid) {
                IdentityRole identityRole = new IdentityRole { Name = createRoleViewModel.RoleName };
                IdentityResult result = await RoleManager.CreateAsync(identityRole);
                if (result.Succeeded) {
                    return RedirectToAction("index", "Administration");
                }
                foreach(IdentityError error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(createRoleViewModel);
        }

        public IActionResult Index() {
            var roles = RoleManager.Roles;
            return View(roles);
        }

        public async Task<IActionResult> Edit(string id) {
            var role = await RoleManager.FindByIdAsync(id);

            var model = new EditRoleViewModel { Id = role.Id, RoleName = role.Name };
            foreach (var user in UserManager.Users){
                if(await UserManager.IsInRoleAsync(user, role.Name)) {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model) {
            var role = await RoleManager.FindByIdAsync(model.Id);
            role.Name = model.RoleName;
            var result = await RoleManager.UpdateAsync(role);
            if (result.Succeeded) {
                return RedirectToAction("index");
            }
            return View(model);
        }



    }
}
