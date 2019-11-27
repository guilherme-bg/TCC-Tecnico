using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models.ViewModels {
    public class RegistrarAnimalFormViewModel {
        public List<IFormFile> Fotos { get; set; } = new List<IFormFile>(new IFormFile[3]);
        [BindProperty]
        public List<string> Saude { get; set; }
        [BindProperty]
        public List<string> Vacina { get; set; }
        public Animal Animal{ get; set; }
    }
}
