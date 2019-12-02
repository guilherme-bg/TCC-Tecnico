using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models.ViewModels {
    public class AnimalFilteringViewModel {
        public string Nome { get; set; }
        public string Especie { get; set; }
        public string Porte { get; set; }
        public string Castrado { get; set; }
        public string Vermifugado { get; set; }
        public string Doador { get; set; }
        public string Cidade { get; set; }
    }
}
