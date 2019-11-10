using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TCC.Models.ViewModels
{
    public class RegistrarUsuarioFormViewModel {
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account", ErrorMessage = "O email já está em uso")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("Password",
            ErrorMessage = "As duas senhas devem ser iguais")]
        public string ConfirmPassword { get; set; }

        public Usuario Usuario { get; set; }

        public string Cidade { get; set; }
        public ICollection<Cidade> Cidades{ get; set; }
    }
}
