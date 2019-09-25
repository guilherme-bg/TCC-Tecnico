using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCC.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }
        public string Endereco { get; set; }
        public string CEP { get; set; }
        public string Moradia { get; set; }
        public string Protecao { get; set; }
        public int QtAnimais { get; set; }

        public Usuario()
        {
        }

        public Usuario(int id, string nome, string email, string senha, string telefone, string cPF, string endereco, string cEP, string moradia, string protecao, int qtAnimais)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Senha = senha;
            Telefone = telefone;
            CPF = cPF;
            Endereco = endereco;
            CEP = cEP;
            Moradia = moradia;
            Protecao = protecao;
            QtAnimais = qtAnimais;
        }
    }
}
