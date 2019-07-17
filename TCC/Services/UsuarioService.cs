using System.Collections.Generic;
using System.Linq;
using TCC.Models;

namespace TCC.Services
{
    public class UsuarioService
    {
        private readonly TCCContext _Context;
        public UsuarioService(TCCContext context)
        {
            _Context = context;
        }
        public List<Usuario> FindAll()
        {
            return _Context.Usuario.ToList();
        }
        public void Insert(Usuario usuario)
        {
            _Context.Add(usuario);
            _Context.SaveChanges();
        }
        public Usuario FindById(int id)
        {
            return _Context.Usuario.FirstOrDefault(obj => obj.Id == id);
        }
        public void Remove(int id)
        {
            var obj = _Context.Usuario.Find(id);
            _Context.Usuario.Remove(obj);
            _Context.SaveChanges();
        }
    }
}
