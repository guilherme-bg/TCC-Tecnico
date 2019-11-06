using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
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
        private readonly ILogger<AccountController> Logger;
        private readonly IEmailSender _EmailSender;

        public AccountController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, CidadeService cidadeService, UsuarioService usuarioService, TCCContext tccContext, RoleManager<IdentityRole> roleManager, ILogger<AccountController> logger, IEmailSender emailSender) {
            UserManager = userManager;
            SignInManager = signInManager;
            _CidadeService = cidadeService;
            _UsuarioService = usuarioService;
            _TccContext = tccContext;
            RoleManager = roleManager;
            Logger = logger;
            _EmailSender = emailSender;
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
                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail","Account", new { userId = user.Id, token = token }, Request.Scheme);
                    Logger.Log(LogLevel.Warning, confirmationLink);
                    if(SignInManager.IsSignedIn(User) && User.IsInRole("Admin")) {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    await UserManager.AddToRoleAsync(user, "Usuario");
                    await _EmailSender.SendEmailAsync(model.Email, "Confirme o seu Email",
                    $"Por favor confirme o seu email <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>clicando aqui</a>.");
                    return View("AfterRegister");
                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token) {
            if(userId == null || token == null) {
                return RedirectToAction("index", "home");
            }
            var user = await UserManager.FindByIdAsync(userId);
            var result = await UserManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded) {
                await SignInManager.SignInAsync(user, true);
                return View();
            }
            return View("Login");
        }

        [AllowAnonymous]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl) {
            
            if (ModelState.IsValid) {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed && (await UserManager.CheckPasswordAsync(user, model.Password))) {
                    ModelState.AddModelError(string.Empty, "Este email não foi confirmado ainda!");
                    return View(model);
                }
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded) {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) {
                        return Redirect(returnUrl);
                    } else {
                        return RedirectToAction("index", "home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Email ou senha incorreto");

            }
            return View(model);
        }

        /*[AllowAnonymous]
        public async Task<ActionResult> ForgotPassword() {
           return View();
        }*/

        /*[HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model) {
            if (ModelState.IsValid) {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if(user != null && await UserManager.IsEmailConfirmedAsync(user)) {
                    var token = await UserManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { emai = model.Email, token = token }, Request.Scheme);
                    Logger.Log(LogLevel.Warning, passwordResetLink);
                    return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }*/

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

        /*[Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model) {
            var user = await UserManager.FindByIdAsync(UserManager.GetUserId(User));
            var userClaims = await UserManager.GetClaimsAsync(user);
            var userRoles = await UserManager.GetRolesAsync(user);
            var cidades = await _CidadeService.FindAllAsync();
            var profile = new EditProfileViewModel {
                Id = user.Id,
                Usuario = user,
                Cidades = cidades
            };
            return View(model);
        }*/

    }
}