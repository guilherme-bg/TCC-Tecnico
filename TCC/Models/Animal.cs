using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models {
    public class Animal {
        public int Id { get; set; }
        public string Foto { get; set; }
        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required]
        [Display(Name = "Espécie")]
        public string Especie { get; set; }
        [Required]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }
        [Required]
        [Display(Name = "Porte")]
        public string Porte { get; set; }
        [Display(Name = "Saúde")]
        public string Saude { get; set; }
        [Required]
        [Display(Name = "Sobre o animal")]
        public string Descricao { get; set; }
        [Display(Name = "Observações sobre a saúde")]
        public string Obs { get; set; }
        public DateTime Data_Cadastro { get; set; }
        public Usuario Usuario { get; set; }
        public string UsuarioId { get; set; }


        public Animal() {
        }

        public Animal(int id, string nome, string especie, string sexo, string porte, string saude, string descricao, DateTime data_Cadastro, Usuario usuario, string usuarioId, string obs) {
            Id = id;
            Nome = nome;
            Especie = especie;
            Sexo = sexo;
            Porte = porte;
            Saude = saude;
            Descricao = descricao;
            Data_Cadastro = data_Cadastro;
            Usuario = usuario;
            UsuarioId = usuarioId;
            Obs = obs;
        }
    }
}
