using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models
{
    public class Cidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public Cidade()
        {
        }

        public Cidade(int id, string nome)
        {
            Id = id;
            Nome = nome;            
        }

        public void AdicionarUsuarios(Usuario usuario)
        {
            Usuarios.Add(usuario);
        }
    }
}
