using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models {
    public class Animal {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Especie { get; set; }
        [Required]
        public string Sexo { get; set; }
        [Required]
        public string Porte { get; set; }
        [Required]
        public string Saude{ get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public DateTime Data_Cadastro{ get; set; }
        public Usuario Usuario { get; set; }
        public int UsuarioId{ get; set; }

        public Animal() {
        }

        public Animal(int id, string nome, string especie, string sexo, string porte, string saude, string descricao, DateTime data_Cadastro, Usuario usuario) {
            Id = id;
            Nome = nome;
            Especie = especie;
            Sexo = sexo;
            Porte = porte;
            Saude = saude;
            Descricao = descricao;
            Data_Cadastro = data_Cadastro;
            Usuario = usuario;
        }
    }
}
