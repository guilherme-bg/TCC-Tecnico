using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TCC.Models;
using TCC.Services;

namespace TCC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _UsuarioService;
        public UsuariosController(UsuarioService usuarioService)
        {
            _UsuarioService = usuarioService;
        }

        public IActionResult Index()
        {
            return View(_UsuarioService.FindAll());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuario usuario)
        {
            _UsuarioService.Insert(usuario);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _UsuarioService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _UsuarioService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
