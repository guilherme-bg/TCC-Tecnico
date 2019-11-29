using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models.ViewModels {
    public class EditarAnimalViewModel {

        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }
        public string Especie { get; set; }
        [Required]
        public string Sexo{ get; set; }
        [Required]
        public string Porte { get; set; }
        [Required]
        public string Descricao{ get; set; }
        [Required]
        public string Obs{ get; set; }
    }
}
