using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models.ViewModels {
    public class EditProfileViewModel {
        
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme sua nova senha")]
        [Compare("NewPassword",
            ErrorMessage = "A senha inserida não é igual à sua nova senha.")]
        public string ConfirmNewPassword { get; set; }

        public Usuario Usuario { get; set; }

        public string Cidade { get; set; }
        public ICollection<Cidade> Cidades { get; set; }
    }
}
