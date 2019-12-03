using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models.ViewModels {
    public class AnimalFilteringViewModel {
        public string Nome { get; set; }
        public string Especie { get; set; }
        public string Porte { get; set; }        
        public int? CidadeId { get; set; }
        public ICollection<Cidade> Cidades { get; set; }
        public IEnumerable<Animal> Animals { get; set; }
    }
}
