using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCC.Models;

namespace TCC.Services
{
    public class UsuarioService
    {
        private readonly TCCContext _Context;
        public UsuarioService(TCCContext context)
        {
            _Context = context;
        }

        public async Task<List<Usuario>> FindAllAsync()
        {
            return await _Context.Usuario.OrderBy(x => x.Nome).ToListAsync();
        }
        public async Task InsertAsync(Usuario usuario)
        {
            _Context.Add(usuario);
            await _Context.SaveChangesAsync();
        }
        public async Task<Usuario> FindByIdAsync(int id)
        {
            return await _Context.Usuario.Include(obj => obj.Cidade).FirstOrDefaultAsync(obj => obj.Id == id);
        }
        public async Task RemoveAsync(int id)
        {
            var obj = await _Context.Usuario.FindAsync(id);
            _Context.Usuario.Remove(obj);
            await _Context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Usuario obj)
        {
            bool hasAny = await _Context.Usuario.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not Found");
            }
            try
            {
                _Context.Update(obj);
                await _Context.SaveChangesAsync();
            }
            catch (DbConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }

}