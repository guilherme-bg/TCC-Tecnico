using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TCC.Models;
using TCC.Models.ViewModels;
using TCC.Services;

namespace TCC.Controllers {
    public class AccountController : Controller {
        private readonly UserManager<Usuario> UserManager;
        private readonly SignInManager<Usuario> SignInManager;
        private readonly CidadeService _CidadeService;
        private readonly UsuarioService _UsuarioService;
        private readonly TCCContext _TccContext;
        private readonly RoleManager<IdentityRole> RoleManager;
        public AccountController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, CidadeService cidadeService, UsuarioService usuarioService, TCCContext tccContext, RoleManager<IdentityRole> roleManager) {
            UserManager = userManager;
            SignInManager = signInManager;
            _CidadeService = cidadeService;
            _UsuarioService = usuarioService;
            _TccContext = tccContext;
            RoleManager = roleManager;
        }
        [HttpPost]
        public async Task<IActionResult> Logout() {
            await SignInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        [AllowAnonymous]
        public async Task<IActionResult> Register() {
            var cidades = await _CidadeService.FindAllAsync();
            var model = new RegistrarUsuarioFormViewModel { Cidades = cidades };
            return View(model);
        }
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email) {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null) {
                return Json(true);
            } else {
                return Json($"Email {email} já está em uso");
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistrarUsuarioFormViewModel model) {
            var cidades = await _CidadeService.FindAllAsync();
            model.Cidades = cidades;
            if (ModelState.IsValid) {
                var user = new Usuario {
                    UserName = model.Email,
                    Email = model.Email,
                    Nome = model.Usuario.Nome,
                    Telefone = model.Usuario.Telefone,
                    Moradia = model.Usuario.Moradia,
                    Protecao = model.Usuario.Protecao,
                    QtAnimais = model.Usuario.QtAnimais,
                    Cidade = model.Usuario.Cidade,
                    CidadeId = model.Usuario.CidadeId,
                    Bio = model.Usuario.Bio
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    await UserManager.AddToRoleAsync(user, "UsuarioNormal");
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Login() {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl) {
            if (ModelState.IsValid) {
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded) {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) {
                        return Redirect(returnUrl);
                    } else {
                        return RedirectToAction("index", "home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Login Inválido");

            }
            return View(model);
        }

        public async Task<IActionResult> Index() {
            var list = await _UsuarioService.FindAllAsync();
            return View(list);
        }
        public async Task<IActionResult> Details(string id) {
            if (id == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = await _UsuarioService.FindByIdAsync(id);
            if (obj == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }
               
        [AllowAnonymous]
        public IActionResult Error(string message) {
            var viewModel = new ErrorViewModel {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(string id) {
            var user = await UserManager.FindByIdAsync(id);
            if(user == null) {
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


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditUser(EditarUsuarioViewModel model) {
            var user = await UserManager.FindByIdAsync(model.Id);
            if (user == null) {
                return View("Usuário não encontrado");
            } else {
                user.Email = model.Email;
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                return View(model);
            }
        }

        public async Task<IActionResult> DeleteUser(string id) {
            var user = await UserManager.FindByIdAsync(id);
            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded) {
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}