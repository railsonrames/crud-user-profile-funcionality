using CrudUserProfileFuncionality.Models;
using CrudUserProfileFuncionality.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudUserProfileFuncionality.Services
{
    public class UsuarioService
    {
        private readonly DataAppContext _context;

        public UsuarioService(DataAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> Listar()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> Detalhar(Guid id)
        {
            return await _context.Usuarios
                .Where(u => u.Id == id)
                .Include(p => p.Perfil)
                .FirstOrDefaultAsync();
        }

        public async void Criar(UsuarioViewModel usuario)
        {
            var usuarioSaved = await _context.Usuarios.AddAsync(new Usuario
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = _context.Perfis.Find(int.Parse(usuario.SelectedPerfil))
            });

            await _context.SaveChangesAsync();
        }

        public async void Atualizar(UsuarioViewModel usuario)
        {
            var perfil = _context.Perfis.Find(int.Parse(usuario.SelectedPerfil));

            _context.Usuarios.Update(new Usuario
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = perfil
            });

            await _context.SaveChangesAsync();
        }

        public async Task<string> Excluir(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return $"Usuario - {usuario.Nome} - {usuario.Email} foi removido com sucesso.";
        }

        public async Task<bool> VerificarExistencia(Guid id)
        {
            return await _context.Usuarios.AnyAsync(x => x.Id == id);
        }
    }
}
