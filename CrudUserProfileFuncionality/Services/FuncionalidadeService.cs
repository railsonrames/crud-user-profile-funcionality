using CrudUserProfileFuncionality.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudUserProfileFuncionality.Services
{
    public class FuncionalidadeService
    {
        private readonly DataAppContext _context;

        public FuncionalidadeService(DataAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Funcionalidade>> Listar()
        {
            return await _context.Funcionalidades.ToListAsync();
        }

        public async Task<Funcionalidade> Detalhar(int id)
        {
            return await _context.Funcionalidades.FindAsync(id);
        }

        public async void Criar(Funcionalidade funcionalidade)
        {
            await _context.Funcionalidades.AddAsync(funcionalidade);
            await _context.SaveChangesAsync();
        }

        public async void Atualizar(Funcionalidade funcionalidade)
        {
            _context.Funcionalidades.Update(funcionalidade);
            await _context.SaveChangesAsync();
        }

        public async Task<string> Excluir(int id)
        {
            var funcionalidade = await _context.Funcionalidades.FindAsync(id);

            var pertencePerfil = _context.PerfilFuncionalidade.Any(x => x.FuncionalidadeId == id);

            if (pertencePerfil)
            {
                return "Esta funcionalidade está vinculada a um perfil, primeiramente exlua os perfis que possuem essa funcionalidade.";
            }
            _context.Funcionalidades.Remove(funcionalidade);
            await _context.SaveChangesAsync();

            return $"Funcionalidade - {funcionalidade.Nome} - {funcionalidade.Descricao} foi removida com sucesso.";
        }

        public async Task<bool> VerificarExistencia(int id)
        {
            return await _context.Funcionalidades.AnyAsync(x => x.Id == id);
        }
    }
}
