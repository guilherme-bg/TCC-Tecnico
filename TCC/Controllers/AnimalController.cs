using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TCC.Models;
using TCC.Models.ViewModels;
using TCC.Services;

namespace TCC.Controllers {
    [Authorize]
    public class AnimalController : Controller {
        private readonly UserManager<Usuario> UserManager;
        private readonly SignInManager<Usuario> SignInManager;
        private readonly UsuarioService _UsuarioService;
        private readonly TCCContext _TccContext;
        private readonly AnimalService _AnimalService;

        public AnimalController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, UsuarioService usuarioService, TCCContext tccContext, AnimalService animalService) {
            UserManager = userManager;
            SignInManager = signInManager;
            _UsuarioService = usuarioService;
            _TccContext = tccContext;
            _AnimalService = animalService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index() {
            var list = await _AnimalService.FinAllAsync();
            return View(list);
        }
        public async Task<IActionResult> AnimalRegister(){
            RegistrarAnimalFormViewModel model = new RegistrarAnimalFormViewModel();
            return View(model);
        }
    }
}