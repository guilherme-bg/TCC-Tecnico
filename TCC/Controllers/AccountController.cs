using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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

        private readonly ILogger<AccountController> Logger;
        private readonly IEmailSender _EmailSender;

        public AccountController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, CidadeService cidadeService, UsuarioService usuarioService, ILogger<AccountController> logger, IEmailSender emailSender) {
            UserManager = userManager;
            SignInManager = signInManager;
            _CidadeService = cidadeService;
            _UsuarioService = usuarioService;


            Logger = logger;
            _EmailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> Logout() {
            await SignInManager.SignOutAsync();
            return RedirectToAction("index", "home");
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

        [AllowAnonymous]
        public async Task<IActionResult> Register() {
            var cidades = await _CidadeService.FindAllAsync();
            var model = new RegistrarUsuarioFormViewModel { Cidades = cidades };
            return View(model);
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
                    var emailtoken = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = emailtoken }, Request.Scheme);
                    Logger.Log(LogLevel.Warning, confirmationLink);
                    if (SignInManager.IsSignedIn(User) && User.IsInRole("Admin")) {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    await UserManager.AddToRoleAsync(user, "Usuario");
                    await _EmailSender.SendEmailAsync(model.Email, "Confirme o seu Email",
                    $"Por favor confirme o seu email <a href='{confirmationLink}'>clicando aqui</a>.");
                    return View("AfterRegister");
                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token) {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null) {
                return View("EmailConfirmationFailed");
            }
            token = token.Replace("%2f", "/").Replace("%2F", "/");
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

        [AllowAnonymous]
        public ActionResult ForgotPassword() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model) {
            if (ModelState.IsValid) {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user != null && await UserManager.IsEmailConfirmedAsync(user)) {
                    var token = await UserManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token }, Request.Scheme);
                    Logger.Log(LogLevel.Warning, passwordResetLink);
                    await _EmailSender.SendEmailAsync(model.Email, "Redefinição de senha", $"Redefina sua senha <a href='{passwordResetLink}'>clicando aqui</a>.");
                    return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordViewModel model) {
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string userId, string token, ResetPasswordViewModel model) {
            var user = await UserManager.FindByIdAsync(userId);
            token = token.Replace("%2f", "/").Replace("%2F", "/");
            if (user == null || token == null) {
                return View("InvalidPasswordReset");
            }
            if (ModelState.IsValid) {
                if (user != null && await UserManager.IsEmailConfirmedAsync(user)) {
                    var result = await UserManager.ResetPasswordAsync(user, token, model.newPassword);
                    if (result.Succeeded) {
                        await SignInManager.SignInAsync(user, true);
                        return View("PasswordResetSuccess");
                    }
                } else {
                    return View("EmailConfirmationFailed");
                }
            }
            return View("index", "home");
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

        [Authorize]
        public async Task<IActionResult> Profile() {
            var userId = UserManager.GetUserId(HttpContext.User);
            var user = await _UsuarioService.FindByIdAsync(userId);
            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> EditProfile() {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            var model = new EditProfileViewModel {
                Id = user.Id,
                Cidades = await _CidadeService.FindAllAsync(),
                Nome = user.Nome,
                Telefone = user.Telefone,
                Moradia = user.Moradia,
                Protecao = user.Protecao,
                QtAnimais = user.QtAnimais,
                CidadeUser = user.Cidade,
                CidadeId = user.CidadeId,
                Bio = user.Bio
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model) {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            model.Cidades = await _CidadeService.FindAllAsync();
            if (user == null) {
                ViewBag.ErrorMessage = $"Usuário não encontrado";
                return View("NotFound");
            } else {
                if (ModelState.IsValid) {
                    user.Nome = model.Nome;
                    user.Telefone = model.Telefone;
                    user.Moradia = model.Moradia;
                    user.Protecao = model.Protecao;
                    user.QtAnimais = model.QtAnimais;
                    user.Cidade = model.CidadeUser;
                    user.CidadeId = model.CidadeId;
                    user.Bio = model.Bio;
                    var result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded) {
                        return RedirectToAction("Profile");
                    }
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

    }
}