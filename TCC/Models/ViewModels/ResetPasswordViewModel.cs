using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models.ViewModels {
    public class ResetPasswordViewModel {
        [Required]
        [Display(Name = "Nova senha:")]
        [DataType(DataType.Password, ErrorMessage = "Digite sua nova senha.")]
        public string newPassword { get; set; }
        [Required]
        [Display(Name = "Confirme a nova senha:")]
        [DataType(DataType.Password, ErrorMessage = "Confirme sua nova senha, por favor!")]
        [Compare("newPassword", ErrorMessage = "As duas senhas devem ser iguais")]
        public string newPasswordConfirmation { get; set; }
    }
}
