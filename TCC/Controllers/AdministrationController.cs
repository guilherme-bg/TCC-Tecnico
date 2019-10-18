
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCC.Models;
using TCC.Models.ViewModels;
using TCC.Services;

namespace TCC.Controllers {
    [Authorize(Roles = "Admin,Moderador")]
    public class AdministrationController : Controller {

        private readonly RoleManager<IdentityRole> RoleManager;
        public UserManager<Usuario> UserManager { get; }
        private readonly CidadeService _CidadeService;
        private readonly UsuarioService _UsuarioService;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<Usuario> userManager, CidadeService cidadeService, UsuarioService usuarioService) {
            RoleManager = roleManager;
            UserManager = userManager;
            _CidadeService = cidadeService;
            _UsuarioService = usuarioService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateRole() {
            return View();
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public IActionResult Index() {
            var roles = RoleManager.Roles;
            return View(roles);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        public async Task<IActionResult> Delete(string id) {
            var role = await RoleManager.FindByIdAsync(id);
            var result = await RoleManager.DeleteAsync(role);
            if (result.Succeeded) {
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        public async Task<IActionResult> EditUser(string id) {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null) {
                return View("Usuário não encontrado");
            }
            var userClaims = await UserManager.GetClaimsAsync(user);
            var userRoles = await UserManager.GetRolesAsync(user);
            var cidades = await _CidadeService.FindAllAsync();
            var model = new EditarUsuarioViewModel {
                Id = user.Id,
                Email = user.Email,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditarUsuarioViewModel model) {
            var user = await UserManager.FindByIdAsync(model.Id);
            if (user == null) {
                return View("Usuário não encontrado");
            } else {
                user.Email = model.Email;
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded) {
                    return RedirectToAction("ListUsers");
                }
                return View(model);
            }
        }

        public async Task<IActionResult> ListUsers() {
            var list = await _UsuarioService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> DeleteUser(string id) {
            var user = await UserManager.FindByIdAsync(id);
            if (!await UserManager.IsInRoleAsync(user, "Admin")) {
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded) {
                    return RedirectToAction("ListUsers");
                }
            }
            return View("ListUsers");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUserRoles(string userId) {
            ViewBag.userId = userId;

            var user = await UserManager.FindByIdAsync(userId);

            if (user == null) {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in RoleManager.Roles) {
                var userRolesViewModel = new UserRolesViewModel {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await UserManager.IsInRoleAsync(user, role.Name)) {
                    userRolesViewModel.IsSelected = true;
                } else {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId) {
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null) {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await UserManager.GetRolesAsync(user);
            var result = await UserManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded) {
                ModelState.AddModelError("", "Não foi possível remover os níveis do usuário");
                return View(model);
            }

            result = await UserManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded) {
                ModelState.AddModelError("", "Não foi possível adicionar os níveis selecionados ao usuário");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = userId });
        }
    }
}