using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TCC.Models;
using TCC.Models.ViewModels;
using TCC.Services;

namespace TCC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _UsuarioService;
        private readonly CidadeService _CidadeService;
        private SignInManager<Usuario> _SignInManager;
        private UserManager<Usuario> _UserManager;
        public UsuariosController(UsuarioService usuarioService, CidadeService cidadeService, SignInManager<Usuario> signInManager, UserManager<Usuario> _userManager)
        {
            _UsuarioService = usuarioService;
            _CidadeService = cidadeService;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _UsuarioService.FindAllAsync();
            return View(list);
        }
        public async Task<IActionResult> Create()
        {
            var cidades =  await _CidadeService.FindAllAsync();
            var ViewModel = new UsuarioFormViewModel { Cidades = cidades };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                var cidades = await _CidadeService.FindAllAsync();
                var viewmodel = new UsuarioFormViewModel { Usuario = usuario, Cidades = cidades };
                return View(usuario);
            }
            await _UsuarioService.InsertAsync(usuario);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            Usuario obj = await _UsuarioService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _UsuarioService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
            
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = await _UsuarioService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = await _UsuarioService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            List<Cidade> cidades = await _CidadeService.FindAllAsync();
            UsuarioFormViewModel viewmodel = new UsuarioFormViewModel { Usuario = obj, Cidades = cidades };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                var cidades = await _CidadeService.FindAllAsync();
                var viewmodel = new UsuarioFormViewModel { Usuario = usuario, Cidades = cidades };
                return View(usuario);
            }
            if (id != usuario.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }
            try
            {
                await _UsuarioService.UpdateAsync(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
