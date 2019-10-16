using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models.ViewModels {
    public class EditarUsuarioViewModel {
        public EditarUsuarioViewModel() {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }
        [Required][EmailAddress]
        public string Email { get; set; }

        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}

