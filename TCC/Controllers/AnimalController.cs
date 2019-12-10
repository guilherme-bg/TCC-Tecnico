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
        public async Task<IActionResult> Index(AnimalFilteringViewModel model) {
            var list = _AnimalService.GetAnimals(model);            
            foreach (Animal animal in list) {
                animal.Cidade = await _CidadeService.FindByIdAsync(animal.CidadeId);
                animal.Usuario = await UserManager.FindByIdAsync(animal.UsuarioId);
            }            
            model.Animals = list;
            model.Cidades = await _CidadeService.FindAllAsync();
            return View(model);

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
                if (model.Fotos != null && model.Fotos.Count > 0) {
                    foreach (IFormFile foto in model.Fotos) {
                        if(foto.FileName.Contains(".jpg") || foto.FileName.Contains(".png")) {
                            string uploadsFolder = Path.Combine(HostingEnvironment.WebRootPath, "images\\");
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + user.Nome + "_" + foto.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                            await foto.CopyToAsync(new FileStream(filePath, FileMode.Create));
                            fotosSalvas.Add(" ~/images/" + uniqueFileName);                            
                        } else {
                            return RedirectToAction("AnimalRegister");
                        }                        
                    }                    
                }
                Animal animal = new Animal {
                    Nome = model.Animal.Nome,
                    Especie = model.Animal.Especie,
                    Porte = model.Animal.Porte,
                    Sexo = model.Animal.Sexo,
                    Descricao = model.Animal.Descricao,
                    Data_Cadastro = DateTime.Now,
                    Foto1 = fotosSalvas.ElementAt(0),
                    Obs = model.Animal.Obs,
                    UsuarioId = user.Id,
                    Usuario = user,
                    CidadeId = user.CidadeId,
                    Cidade = user.Cidade
                };
                if (model.Saude != null) animal.Saude = String.Join(",", model.Saude);
                if (model.Vacina != null) animal.Vacina = String.Join(",", model.Vacina);
                if (fotosSalvas.Count >= 2) animal.Foto2 = fotosSalvas.ElementAt(1);
                if (fotosSalvas.Count == 3) animal.Foto3 = fotosSalvas.ElementAt(2);

                _TccContext.Animal.Add(animal);
                animal.Usuario.AddAnimal(animal);
                _TccContext.SaveChanges();
                return RedirectToAction("index", "animal");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id) {
            var animal = await _AnimalService.FindByIdAsync(id);
            var user = await UserManager.GetUserAsync(HttpContext.User);
            if (animal.UsuarioId == user.Id || (await UserManager.IsInRoleAsync(user, "Admin") || await UserManager.IsInRoleAsync(user, "Moderador"))) {
                var model = new EditarAnimalViewModel {
                    Id = animal.Id,
                    Especie = animal.Especie,
                    Nome = animal.Nome,
                    Sexo = animal.Sexo,
                    Porte = animal.Porte,
                    Descricao = animal.Descricao,
                    Obs = animal.Obs
                };
                return View(model);
            } else {
                return View("AccessDenied");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditarAnimalViewModel model) {
            var animal = await _AnimalService.FindByIdAsync(model.Id);

            if (ModelState.IsValid) {
                animal.Nome = model.Nome;
                animal.Porte = model.Porte;
                animal.Sexo = model.Sexo;
                animal.Descricao = model.Descricao;
                animal.Obs = model.Descricao;
                _TccContext.Animal.Update(animal);
                _TccContext.SaveChanges();
                return RedirectToAction("Details", animal);
            }
            return View(model);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Details(int id) {
            var animal = await _AnimalService.FindByIdAsync(id);
            animal.Usuario = await UserManager.FindByIdAsync(animal.UsuarioId);
            animal.Usuario.Cidade = await _CidadeService.FindByIdAsync(animal.Usuario.CidadeId);
            return View(animal);
        }

        public async Task<IActionResult> Adopted(int id) {
            var animal = await _AnimalService.FindByIdAsync(id);
            var user = await UserManager.GetUserAsync(HttpContext.User);
            if (animal.UsuarioId == user.Id) {
                animal.Adotado = true;
                _TccContext.Update(animal);
                _TccContext.SaveChanges();
                animal.Usuario = await UserManager.FindByIdAsync(animal.UsuarioId);
                animal.Usuario.Cidade = await _CidadeService.FindByIdAsync(animal.Usuario.CidadeId);
                return View("Details", animal);
            } else {
                return View("AccessDenied");
            }
        }

        public async Task<IActionResult> AvailableAgain(int id) {
            var animal = await _AnimalService.FindByIdAsync(id);
            var user = await UserManager.GetUserAsync(HttpContext.User);
            if (animal.UsuarioId == user.Id) {
                animal.Adotado = false;
                _TccContext.Update(animal);
                _TccContext.SaveChanges();
                animal.Usuario = await UserManager.FindByIdAsync(animal.UsuarioId);
                animal.Usuario.Cidade = await _CidadeService.FindByIdAsync(animal.Usuario.CidadeId);
                return View("Details", animal);
            } else {
                return View("AccessDenied");
            }
        }

        [Authorize(Roles = "Admin, Moderador")]
        public async Task<IActionResult> Delete(int id) {
            var animal = await _AnimalService.FindByIdAsync(id);
            if (User.IsInRole("Admin") || User.IsInRole("Moderador")) {
                animal.Adotado = false;
                _TccContext.Remove(animal);
                _TccContext.SaveChanges();
                return RedirectToAction("Index");
            } else {
                return View("AccessDenied");
            }
        }
    }
}