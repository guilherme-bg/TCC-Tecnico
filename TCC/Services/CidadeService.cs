using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCC.Models;

namespace TCC.Services {
    public class CidadeService {
        private readonly TCCContext _Context;
        public CidadeService(TCCContext context) {
            _Context = context;
        }
        public async Task<List<Cidade>> FindAllAsync() {
            return await _Context.Cidade.OrderBy(x => x.Nome).ToListAsync();
        }

        public async Task<Cidade> FindByIdAsync(int id) {
            return await _Context.Cidade.FindAsync(id);
        }
    }
}
