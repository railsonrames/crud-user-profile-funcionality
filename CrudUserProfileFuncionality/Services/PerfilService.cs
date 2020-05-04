using CrudUserProfileFuncionality.Models;
using CrudUserProfileFuncionality.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudUserProfileFuncionality.Services
{
    public class PerfilService
    {
        private readonly DataAppContext _context;

        public PerfilService(DataAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Perfil>> Listar()
        {
            return await _context.Perfis.ToListAsync();
        }

        public async Task<Perfil> Detalhar(int id)
        {
            return await _context.Perfis
                .Where(p => p.Id == id)
                .Include(pf => pf.PerfilFuncionalidade)
                .ThenInclude(f => f.Funcionalidade)
                .FirstOrDefaultAsync();
        }

        public async void Criar(PerfilViewModel perfil)
        {
            var perfilSaved = _context.Perfis.Add(new Perfil
            {
                Nome = perfil.Nome,
                Descricao = perfil.Descricao
            });

            _context.SaveChanges();

            if (perfil.SelectedFuncionalidades.Length > 0)
            {
                foreach (var funcionalidade in perfil.SelectedFuncionalidades)
                {
                    await _context.PerfilFuncionalidade.AddAsync(new PerfilFuncionalidade
                    {
                        PerfilId = perfilSaved.Entity.Id,
                        FuncionalidadeId = int.Parse(funcionalidade)
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async void Atualizar(PerfilViewModel perfil)
        {
            var perfilSaved = _context.Perfis.Update(new Perfil
            {
                Id = perfil.Id,
                Nome = perfil.Nome,
                Descricao = perfil.Descricao
            });

            _context.SaveChanges();

            var funcionalidadesVinculadas = await _context.PerfilFuncionalidade
                .Where(x => x.PerfilId == perfilSaved.Entity.Id)
                .Select(x => x.FuncionalidadeId)
                .ToListAsync();

            if (perfil.SelectedFuncionalidades.Length > 0)
            {
                foreach (var funcionalidadeVinculada in funcionalidadesVinculadas)
                {
                    var vinculo = _context.PerfilFuncionalidade.Find(perfil.Id, funcionalidadeVinculada);
                    _context.PerfilFuncionalidade.Remove(vinculo);
                }

                foreach (var funcionalidade in perfil.SelectedFuncionalidades)
                {
                    var idFuncionalidade = int.Parse(funcionalidade);

                    await _context.PerfilFuncionalidade.AddAsync(new PerfilFuncionalidade
                    {
                        PerfilId = perfilSaved.Entity.Id,
                        FuncionalidadeId = idFuncionalidade
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> Excluir(int id)
        {
            var perfil = await _context.Perfis.FindAsync(id);

            var pertenceUsuario = await _context.Usuarios.AnyAsync(x => x.Perfil == perfil);

            if (pertenceUsuario)
            {
                return "Este perfil está vinculado a um usuário, primeiramente exclua os vínculos com usuários que possuem este perfil.";
            }
            _context.Perfis.Remove(perfil);
            await _context.SaveChangesAsync();

            return $"Perfil - {perfil.Nome} - {perfil.Descricao} foi removido com sucesso.";
        }

        public async Task<bool> VerificarExistencia(int id)
        {
            return await _context.Perfis.AnyAsync(x => x.Id == id);
        }
    }
}
