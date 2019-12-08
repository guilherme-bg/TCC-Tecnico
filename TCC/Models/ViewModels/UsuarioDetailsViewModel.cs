using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models.ViewModels {
    public class UsuarioDetailsViewModel {
        public Usuario Usuario { get; set; }
        public int AnimaisCadastrados { get; set; }
        public int AnimaisCadastradosAdotados { get; set; }
    }
}
