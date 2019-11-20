using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models.ViewModels {
    public class RegistrarAnimalFormViewModel {
        public IFormFile Fotos { get; set; }
        public Animal Animal{ get; set; }
    }
}
