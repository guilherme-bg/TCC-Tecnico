using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models {
    public class Animal {
        public int Id { get; set; }
        public string Foto1 { get; set; }
        public string Foto2 { get; set; }
        public string Foto3 { get; set; }
        [Required(ErrorMessage = "Digite o nome do animal.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Selecione a espécie.")]
        [Display(Name = "Espécie")]
        public string Especie { get; set; }
        [Required(ErrorMessage = "Selecione o sexo do animal.")]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }
        [Required(ErrorMessage = "Selecione o porte do animal.")]
        [Display(Name = "Porte")]
        public string Porte { get; set; }
        [Display(Name = "Saúde")]
        public string Saude { get; set; }
        [Display(Name = "Vacinas")]
        public string Vacina { get; set; }
        [Required(ErrorMessage = "Digite a história do amiguinho.")]
        [Display(Name = "Sobre o animal")]
        public string Descricao { get; set; }
        [Display(Name = "Observações ou deficiências")]
        public string Obs { get; set; }
        public bool Adotado { get; set; }
        public DateTime Data_Cadastro { get; set; }
        public Usuario Usuario { get; set; }
        public string UsuarioId { get; set; }
        public int CidadeId { get; set; }
        public Cidade Cidade { get; set; }


        public Animal() {
        }        
    }
}
