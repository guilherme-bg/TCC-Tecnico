using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCC.Models;
using TCC.Models.ViewModels;

namespace TCC.Services {
    public class AnimalService {
        private readonly TCCContext _Context;
        
        public AnimalService (TCCContext _tccContext) {
            _Context = _tccContext;
        }

        public async Task<List<Animal>> FindAllAsync() {
            return await _Context.Animal.OrderBy(x => x.Nome).ToListAsync();
        }

        public async Task<Animal> FindByIdAsync(int id) {
            return await _Context.Animal.FindAsync(id);
        }

        public IQueryable<Animal> GetAnimals(AnimalFilteringViewModel model) {
            var result = _Context.Animal.AsQueryable();
            if(model != null) {
                if (!string.IsNullOrEmpty(model.Nome))
                    result = result.Where(x => x.Nome.Contains(model.Nome));
                if (!string.IsNullOrEmpty(model.Especie))
                    result = result.Where(x => x.Especie.Contains(model.Especie));
                if (!string.IsNullOrEmpty(model.Porte))
                    result = result.Where(x => x.Porte.Contains(model.Porte));
                if (model.CidadeId.HasValue)
                    result = result.Where(x => x.CidadeId == model.CidadeId);
            }
            return result;
        }
    }
}
