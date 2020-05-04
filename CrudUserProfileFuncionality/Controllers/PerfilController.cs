using CrudUserProfileFuncionality.Services;
using CrudUserProfileFuncionality.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CrudUserProfileFuncionality.Controllers
{
    public class PerfilController : Controller
    {
        private readonly PerfilService _perfilService;
        private readonly FuncionalidadeService _funcionalidadeService;

        public PerfilController(PerfilService perfilService, FuncionalidadeService funcionalidadeService)
        {
            _perfilService = perfilService;
            _funcionalidadeService = funcionalidadeService;
        }

        /// <summary>
        /// O método Index lista todos os perfis cadastrados.
        /// </summary>
        /// <returns>Página com listagem e ações refentes ao registro.</returns>
        public async Task<IActionResult> Index()
        {
            ViewBag.Message = TempData["Message"];
            return View(await _perfilService.Listar());
        }

        /// <summary>
        /// O método Details exibe os atributos do perfil cadastrado, pontualmente pelo seu identificador único.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Página com atributos referentes ao perfil buscado.</returns>
        public async Task<IActionResult> Details(int id)
        {
            var perfil = await _perfilService.Detalhar(id);

            if (perfil == null)
            {
                return NotFound();
            }

            var perfilViewModel = new PerfilViewModel
            {
                Id = perfil.Id,
                Nome = perfil.Nome,
                Descricao = perfil.Descricao,
                Funcionalidades = perfil.PerfilFuncionalidade
                    .Select(x => new SelectListItem
                    {
                        Text = x.Funcionalidade.Nome,
                        Value = x.Funcionalidade.Descricao
                    })
            };

            return View(perfilViewModel);
        }

        /// <summary>
        /// O método Create exibe campos para a inserção de dados referentes a um novo perfil.
        /// </summary>
        /// <returns>Página com campos para criação de novo registro de perfil.</returns>
        public IActionResult Create()
        {
            var listaFuncionalidades = _funcionalidadeService.Listar().Result;

            var perfilObject = new PerfilViewModel
            {
                Funcionalidades = listaFuncionalidades.Select(x => new SelectListItem
                {
                    Text = x.Nome,
                    Value = x.Id.ToString()
                })
            };
            return View(perfilObject);
        }

        /// <summary>
        /// O método Create recebe o objeto submetido pelo cliente para persistência na base de dados.
        /// </summary>
        /// <param name="perfil"></param>
        /// <returns>Página com a listagem de perfis.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PerfilViewModel perfil)
        {
            if (ModelState.IsValid)
            {
                _perfilService.Criar(perfil);
                return RedirectToAction(nameof(Index));
            }
            return View(perfil);
        }

        /// <summary>
        /// O método Edit exibe campos para a edição de dados referentes a um perfil específico.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Página com campos preenchidos com os dados salvos anteriormente de um perfil específico para edição.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var listaFuncionalidades = _funcionalidadeService.Listar().Result;

            var perfil = await _perfilService.Detalhar(id);

            var perfilObject = new PerfilViewModel
            {
                Id = perfil.Id,
                Nome = perfil.Nome,
                Descricao = perfil.Descricao,
                Funcionalidades = listaFuncionalidades
                    .Select(x => new SelectListItem
                    {
                        Text = x.Nome,
                        Value = x.Id.ToString(),
                        Selected = perfil.PerfilFuncionalidade.Any(y => y.FuncionalidadeId == x.Id)
                    })
            };

            return View(perfilObject);
        }

        /// <summary>
        /// O método Edit recebe o objeto submetido pelo cliente para pesistência na base de dados.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="perfil"></param>
        /// <returns>Página com a listagem de perfis.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PerfilViewModel perfil)
        {
            if (id != perfil.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(perfil);
            try
            {
                _perfilService.Atualizar(perfil);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerfilExists(perfil.Id).Result)
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
        /// O método Delete exibe os atributos do perfil cadastrado, pontualmente pelo seu identificador único para confirmarção de exclusão.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Página com a listagem de perfis.</returns>
        public async Task<IActionResult> Delete(int id)
        {
            var perfil = await _perfilService.Detalhar(id);

            if (perfil == null)
            {
                return NotFound();
            }

            var perfilViewModel = new PerfilViewModel
            {
                Id = perfil.Id,
                Nome = perfil.Nome,
                Descricao = perfil.Descricao,
                Funcionalidades = perfil.PerfilFuncionalidade
                    .Select(x => new SelectListItem
                    {
                        Text = x.Funcionalidade.Nome,
                        Value = x.Funcionalidade.Descricao
                    })
            };

            return View(perfilViewModel);
        }

        /// <summary>
        /// O método Delete recebe o objeto submetido pelo cliente para remoção na base de dados.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Página com a listagem de perfis.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TempData["Message"] = await _perfilService.Excluir(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// O método PerfilExists verifica a existência de registro identificado pelo identificador único na base de dados.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Valor boleano referênte a existência ou não do registro identificado pelo idendificador único passado.</returns>
        private async Task<bool> PerfilExists(int id)
        {
            return await _perfilService.VerificarExistencia(id);
        }
    }
}
