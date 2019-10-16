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
        [Required(ErrorMessage = "Preencha com seu nome, por favor!")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = " O nome tem que ter entre {2} e {1} caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Insita seu telefone, por favor")]
        [RegularExpression(@"^\(?\d{2}\)?[\s-]?[\s9]?\d{4}-?\d{4}$", ErrorMessage = "Digite no formato: (51) 99999-9999")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Selecione a sua moradia!")]
        public string Moradia { get; set; }
        [Required(ErrorMessage = "Selecione uma das opções!")]
        public string Protecao { get; set; }
        [Required(ErrorMessage = "Digite a quantidade de animais")]
        [Display(Name = "Animais em Casa")]
        [Range(0, 300)]
        public int QtAnimais { get; set; }
        public Cidade Cidade { get; set; }
        [Required(ErrorMessage = "Selecione sua cidade!")]
        [Display(Name = "Cidade")]
        public int CidadeId { get; set; }
        [Display(Name = "Sobre mim")]
        [StringLength(1000)]
        public string Bio { get; set; }

        public Usuario()
        {            
        }      
    }
}
