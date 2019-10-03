using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCC.Models;

namespace TCC.Data
{
    public class SeedingService
    {
        private TCCContext _context;
        public SeedingService(TCCContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            if( _context.Cidade.Any() || _context.Usuario.Any())
            {
                return;
            }
            Cidade c1 = new Cidade(1, "Alvorada");
            Cidade c2 = new Cidade(2, "Araricá");
            Cidade c3 = new Cidade(3, "Arroio dos Ratos");
            Cidade c4 = new Cidade(4, "Cachoeirinha");
            Cidade c5 = new Cidade(5, "Campo Bom");
            Cidade c6 = new Cidade(6, "Canoas");
            Cidade c7 = new Cidade(7, "Capela de Santana");
            Cidade c8 = new Cidade(8, "Charqueadas");
            Cidade c9 = new Cidade(9, "Dois Irmãos");
            Cidade c10 = new Cidade(10, "Eldorado do Sul");
            Cidade c11 = new Cidade(11, "Esteio");
            Cidade c12 = new Cidade(12, "Estância Velha");
            Cidade c13 = new Cidade(13, "Glorinha");
            Cidade c14 = new Cidade(14, "Gravataí");
            Cidade c15 = new Cidade(15, "Guaíba");
            Cidade c16 = new Cidade(16, "Igrejinha");
            Cidade c17 = new Cidade(17, "Ivoti");
            Cidade c18 = new Cidade(18, "Montenegro");
            Cidade c19 = new Cidade(19, "Nova Hartz");
            Cidade c20 = new Cidade(20, "Nova Santa Rita");
            Cidade c21 = new Cidade(21, "Novo Hamburgo");
            Cidade c22 = new Cidade(22, "Parobé");
            Cidade c23 = new Cidade(23, "Porto Alegre");
            Cidade c24 = new Cidade(24, "Portão");
            Cidade c25 = new Cidade(25, "Rolante");
            Cidade c26 = new Cidade(26, "Santo Antônio da Patrulha");
            Cidade c27 = new Cidade(27, "Sapiranga");
            Cidade c28 = new Cidade(28, "Sapucaia do Sul");
            Cidade c29 = new Cidade(29, "São Jerônimo");
            Cidade c30 = new Cidade(30, "São Leopoldo");
            Cidade c31 = new Cidade(31, "São Sebastião do Caí");
            Cidade c32 = new Cidade(32, "Taquara");
            Cidade c33 = new Cidade(33, "Triunfo");
            Cidade c34 = new Cidade(34, "Viamão");

            _context.Cidade.AddRange(
                c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, 
                c11, c12, c13, c14, c15, c16, c17, c18, c19, 
                c20, c21, c22, c23, c24, c25, c26, c27, c28, 
                c29, c30, c31, c32, c33, c34
                );

            Usuario u1 = new Usuario(1, "Guilherme", "guilhermebritogasparini@gmail.com", "123456789", "980652317", c4, "Casa", "Sim", 1);
            _context.Usuario.Add(u1);

            _context.SaveChanges();
        }
    }
}
