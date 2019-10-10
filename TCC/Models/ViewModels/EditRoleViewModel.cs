using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models.ViewModels {
    public class EditRoleViewModel {
        public EditRoleViewModel() {
            Users = new List<string>();
        }
        
        public string Id { get; set; }

        [Display(Name = "Nome do Nível")]
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
