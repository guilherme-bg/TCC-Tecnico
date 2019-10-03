using System.Collections.Generic;

namespace TCC.Models.ViewModels
{
    public class UsuarioFormViewModel
    {
        public Usuario Usuario { get; set; }
        public ICollection<Cidade> Cidades{ get; set; }
    }
}
