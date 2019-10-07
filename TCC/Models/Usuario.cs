using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models
{
    public class Usuario : IdentityUser
    {
        [Required(ErrorMessage = "Preencha por favor")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = " O nome tem que ter entre {2} e {1} caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Preencha por favor")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Preencha por favor")]
        public string Moradia { get; set; }
        [Required(ErrorMessage = "Preencha por favor")]
        public string Protecao { get; set; }
        [Required(ErrorMessage = "Preencha por favor")]
        [Display(Name = "Animais em Casa")]
        public int QtAnimais { get; set; }
        public Cidade Cidade { get; set; }
        [Required(ErrorMessage = "Selecione sua cidade")]
        [Display(Name = "Cidade")]
        public int CidadeId { get; set; }

        public Usuario()
        {
        }      
    }
}
