using CrudUserProfileFuncionality.Services;
using CrudUserProfileFuncionality.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudUserProfileFuncionality.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly PerfilService _perfilService;
        private readonly UsuarioService _usuarioService;

        public UsuarioController(PerfilService perfilService, UsuarioService usuarioService)
        {
            _perfilService = perfilService;
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// O método Index lista todos os usuários cadastrados.
        /// </summary>
        /// <returns>Página com listagem e ações refentes ao registro.</returns>
        public async Task<IActionResult> Index()
        {
            ViewBag.Message = TempData["Message"];
            return View(await _usuarioService.Listar());
        }

        /// <summary>
        /// O método Details exibe os atributos do usuário cadastrado, pontualmente pelo seu identificador único.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Página com atributos referentes ao usuário buscado.</returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var usuario = await _usuarioService.Detalhar(id);

            if (usuario == null)
            {
                return NotFound();
            }

            var listaPerfilSalvo = new List<SelectListItem>
            {
                new SelectListItem(usuario.Perfil.Nome, usuario.Perfil.Descricao, true)
            };

            var usuarioViewModel = new UsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfis = listaPerfilSalvo
            };

            return View(usuarioViewModel);
        }

        /// <summary>
        /// O método Create exibe campos para a inserção de dados referentes a um novo usuário.
        /// </summary>
        /// <returns>Página com campos para criação de novo registro de usuário.</returns>
        public IActionResult Create()
        {
            var listaPerfis = _perfilService.Listar().Result;

            var usuarioObject = new UsuarioViewModel
            {
                Perfis = listaPerfis.Select(x => new SelectListItem
                {
                    Text = x.Nome,
                    Value = x.Id.ToString()
                })
            };

            return View(usuarioObject);
        }

        /// <summary>
        /// O método Create recebe o objeto submetido pelo cliente para persistência na base de dados.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>Página com a listagem de usuários.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UsuarioViewModel usuario)
        {
            if (!ModelState.IsValid) return View(usuario);
            _usuarioService.Criar(usuario);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// O método Edit exibe campos para a edição de dados referentes a um usuário específico.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Página com campos preenchidos com os dados salvos anteriormente de um usuário específico para edição.</returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var listaPerfis = _perfilService.Listar().Result;

            var usuario = await _usuarioService.Detalhar(id);

            var usuarioObject = new UsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfis = listaPerfis
                    .Select(x => new SelectListItem
                    {
                        Text = x.Nome,
                        Value = x.Id.ToString(),
                        Selected = usuario.Perfil.Id == x.Id
                    })
            };

            return View(usuarioObject);
        }

        /// <summary>
        /// O método Edit recebe o objeto submetido pelo cliente para pesistência na base de dados.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usuario"></param>
        /// <returns>Página com a listagem de usuários.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UsuarioViewModel usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(usuario);
            try
            {
                _usuarioService.Atualizar(usuario);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.Id).Result)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// O método Delete exibe os atributos do usuário cadastrado, pontualmente pelo seu identificador único para confirmarção de exclusão.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Página com a listagem de usuários.</returns>
        public async Task<IActionResult> Delete(Guid id)
        {

            var usuario = await _usuarioService.Detalhar(id);
            if (usuario == null)
            {
                return NotFound();
            }

            var listaPerfilSalvo = new List<SelectListItem>
                {
                    new SelectListItem(usuario.Perfil.Nome, usuario.Perfil.Descricao, true)
                };

            var usuarioViewModel = new UsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfis = listaPerfilSalvo
            };

            return View(usuarioViewModel);
        }

        /// <summary>
        /// O método Delete recebe o objeto submetido pelo cliente para remoção na base de dados.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Página com a listagem de usuários.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            TempData["Message"] = await _usuarioService.Excluir(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// O método UsuarioExists verifica a existência de registro identificado pelo identificador único na base de dados.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Valor boleano referênte a existência ou não do registro identificado pelo idendificador único passado.</returns>
        private async Task<bool> UsuarioExists(Guid id)
        {
            return await _usuarioService.VerificarExistencia(id);
        }
    }
}
