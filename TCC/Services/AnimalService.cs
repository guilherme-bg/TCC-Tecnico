using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCC.Models;

namespace TCC.Services {
    public class AnimalService {
        private readonly TCCContext _Context;
        
        public AnimalService (TCCContext _tccContext) {
            _Context = _tccContext;
        }

        public async Task<List<Animal>> FinAllAsync() {
            return await _Context.Animal.OrderBy(x => x.Nome).ToListAsync();
        }
    }
}
