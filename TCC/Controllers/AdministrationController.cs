
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCC.Models;
using TCC.Models.ViewModels;

namespace TCC.Controllers {
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller {

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
                foreach (IdentityError error in result.Errors) {
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

            var model = new EditRoleViewModel {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in UserManager.Users) {
                if (await UserManager.IsInRoleAsync(user, role.Name)) {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model) {
            var role = await RoleManager.FindByIdAsync(model.Id);
            if (role == null) {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            } else {
                role.Name = model.RoleName;
                var result = await RoleManager.UpdateAsync(role);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        public async Task<IActionResult> EditUsersInRole(string roleId) {
            ViewBag.roleId = roleId;
            var role = await RoleManager.FindByIdAsync(roleId);
            var model = new List<UserRoleViewModel>();
            foreach (var user in UserManager.Users) {
                var userRoleViewModel = new UserRoleViewModel { UserId = user.Id, UserName = user.UserName };
                if (await UserManager.IsInRoleAsync(user, role.Name)) {
                    userRoleViewModel.IsSelected = true;
                } else {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId) {
            var role = await RoleManager.FindByIdAsync(roleId);
            for (int i = 0; i < model.Count; i++) {
                var user = await UserManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !await UserManager.IsInRoleAsync(user, role.Name)) {
                    result = await UserManager.AddToRoleAsync(user, role.Name);
                } else if (!model[i].IsSelected && await UserManager.IsInRoleAsync(user, role.Name)) {
                    result = await UserManager.RemoveFromRoleAsync(user, role.Name);
                } else {
                    continue;
                }
                if (result.Succeeded) {
                    if (i < (model.Count - 1)) continue;
                    else return RedirectToAction("Edit", new { Id = roleId });
                }
            }
            return RedirectToAction("Edit", new { Id = roleId });
        }
    }
}
