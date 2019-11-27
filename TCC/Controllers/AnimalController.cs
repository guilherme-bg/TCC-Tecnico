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
        private readonly CidadeService _CidadeService;
        private readonly IHostingEnvironment HostingEnvironment;

        public AnimalController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, UsuarioService usuarioService, TCCContext tccContext, AnimalService animalService, CidadeService cidadeService, IHostingEnvironment hostingEnvironment) {
            UserManager = userManager;
            SignInManager = signInManager;
            _UsuarioService = usuarioService;
            _TccContext = tccContext;
            _AnimalService = animalService;
            _CidadeService = cidadeService;
            HostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index() {
            var list = await _AnimalService.FinAllAsync();
            foreach(Animal animal in list) {
                animal.Cidade = await _CidadeService.FindByIdAsync(animal.CidadeId);
                animal.Usuario = await UserManager.FindByIdAsync(animal.UsuarioId);
            }
            return View(list);
        }
        public async Task<IActionResult> AnimalRegister() {
            RegistrarAnimalFormViewModel model = new RegistrarAnimalFormViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AnimalRegister(RegistrarAnimalFormViewModel model) {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            List<string> fotosSalvas = new List<string>();
            if (ModelState.IsValid && fotosSalvas.Count <= 3) {
                string uniqueFileName = null;
                if(model.Fotos != null && model.Fotos.Count > 0) {
                    foreach(IFormFile foto in model.Fotos) {                    
                    string uploadsFolder = Path.Combine(HostingEnvironment.WebRootPath, "images\\");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + user.Nome + "_" + foto.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    await foto.CopyToAsync(new FileStream(filePath, FileMode.Create));
                    fotosSalvas.Add(" ~/images/" + uniqueFileName);
                    }
                }
                Animal animal = new Animal {
                    Nome = model.Animal.Nome,
                    Especie = model.Animal.Especie,
                    Porte = model.Animal.Porte,
                    Sexo = model.Animal.Sexo,
                    Saude = String.Join(",",model.Saude),
                    Vacina = String.Join(",", model.Vacina),
                    Descricao = model.Animal.Descricao,
                    Data_Cadastro = DateTime.Now,
                    Foto1 = fotosSalvas.ElementAt(0),
                    Obs = model.Animal.Obs,
                    UsuarioId = user.Id,
                    Usuario = user,
                    CidadeId = user.CidadeId,
                    Cidade = user.Cidade
                };                
                if (fotosSalvas.Count >= 2) animal.Foto2 = fotosSalvas.ElementAt(1);
                if (fotosSalvas.Count == 3) animal.Foto3 = fotosSalvas.ElementAt(2);

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