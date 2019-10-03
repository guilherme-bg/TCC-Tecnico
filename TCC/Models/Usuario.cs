using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Preencha por favor")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = " O nome tem que ter entre {2} e {1} caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Preencha por favor")]
        [EmailAddress(ErrorMessage = "Insira um email válido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage ="Preencha por favor")]
        [StringLength(223, MinimumLength = 8, ErrorMessage ="A senha deve ter entre {2} e {1} caracteres")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
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

        public Usuario(int id, string nome, string email, string senha, string telefone, Cidade cidade, string moradia, string protecao, int qtAnimais)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Senha = senha;
            Telefone = telefone;
            Cidade = cidade;
            Moradia = moradia;
            Protecao = protecao;
            QtAnimais = qtAnimais;
        }
    }
}
