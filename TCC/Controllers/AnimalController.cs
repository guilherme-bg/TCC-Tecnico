using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TCC.Models;
using TCC.Models.ViewModels;
using TCC.Services;
using Microsoft.AspNetCore.Http;

namespace TCC.Controllers {
    [Authorize]
    public class AnimalController : Controller {
        private readonly UserManager<Usuario> UserManager;
        private readonly SignInManager<Usuario> SignInManager;
        private readonly UsuarioService _UsuarioService;
        private readonly TCCContext _TccContext;
        private readonly AnimalService _AnimalService;
        private readonly IHostingEnvironment HostingEnvironment;

        public AnimalController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, UsuarioService usuarioService, TCCContext tccContext, AnimalService animalService, IHostingEnvironment hostingEnvironment) {
            UserManager = userManager;
            SignInManager = signInManager;
            _UsuarioService = usuarioService;
            _TccContext = tccContext;
            _AnimalService = animalService;
            HostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index() {
            var list = await _AnimalService.FinAllAsync();
            return View(list);
        }
        public async Task<IActionResult> AnimalRegister() {
            RegistrarAnimalFormViewModel model = new RegistrarAnimalFormViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AnimalRegister(RegistrarAnimalFormViewModel model) {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid) {
                string uniqueFileName = null;
                if(model.Fotos != null && model.Fotos.Count > 0) {
                    foreach(IFormFile foto in model.Fotos) {                    
                    string uploadsFolder = Path.Combine(HostingEnvironment.WebRootPath, "images\\");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + user.Nome + "_" + foto.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    await foto.CopyToAsync(new FileStream(filePath, FileMode.Create));
                    }
                }
                Animal animal = new Animal {
                    Nome = model.Animal.Nome,
                    Especie = model.Animal.Especie,
                    Porte = model.Animal.Especie,
                    Sexo = model.Animal.Sexo,
                    Saude = String.Join(",",model.Saude),
                    Foto = uniqueFileName,
                    Descricao = model.Animal.Descricao,
                    Data_Cadastro = DateTime.Now,
                    Obs = model.Animal.Obs,
                    UsuarioId = user.Id,
                    Usuario = user
                };                
                _TccContext.Animal.Add(animal);
                animal.Usuario.AddAnimal(animal);
                _TccContext.SaveChanges();                
                return RedirectToAction("index","home");
            }
            return View(model);
        }
        public async Task<IActionResult> Details(int id) {
            var animal = await _AnimalService.FindByIdAsync(id);
            return View(animal);
        }

    }
}