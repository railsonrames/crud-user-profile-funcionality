using CrudUserProfileFuncionality.Models;
using CrudUserProfileFuncionality.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CrudUserProfileFuncionality.Controllers
{
    public class FuncionalidadeController : Controller
    {
        private readonly FuncionalidadeService _funcionalidadeService;

        public FuncionalidadeController(FuncionalidadeService funcionalidadeService)
        {
            _funcionalidadeService = funcionalidadeService;
        }

        /// <summary>
        /// O método Index lista todos as funcionalidades cadastradas.
        /// </summary>
        /// <returns>Página com listagem e ações refentes ao registro.</returns>
        public async Task<IActionResult> Index(string message)
        {
            ViewBag.Message = TempData["Message"];
            return View(await _funcionalidadeService.Listar());
        }

        /// <summary>
        /// O método Details exibe os atributos da funcionalidade cadastrada, pontualmente pelo seu identificador único.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Página com atributos referentes a funcionalidade buscada.</returns>
        public async Task<IActionResult> Details(int id)
        {
            var funcionalidade = await _funcionalidadeService.Detalhar(id);
            if (funcionalidade == null)
            {
                return NotFound();
            }

            return View(funcionalidade);
        }

        /// <summary>
        /// O método Create exibe campos para a inserção de dados referentes a uma nova funcionalidade.
        /// </summary>
        /// <returns>Página com campos para criação de novo registro de funcionalidade.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// O método Create recebe o objeto submetido pelo cliente para persistência na base de dados.
        /// </summary>
        /// <param name="funcionalidade"></param>
        /// <returns>Página com a listagem de funcionalidades.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao")] Funcionalidade funcionalidade)
        {
            if (ModelState.IsValid)
            {
                _funcionalidadeService.Criar(funcionalidade);
                return RedirectToAction(nameof(Index));
            }
            return View(funcionalidade);
        }

        /// <summary>
        /// O método Edit exibe campos para a edição de dados referentes a uma funcionalidade específica.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Página com campos preenchidos com os dados salvos anteriormente de uma funcionalidade específica para edição.</returns>
        public async Task<IActionResult> Edit(int id)
        {

            var funcionalidade = await _funcionalidadeService.Detalhar(id);
            if (funcionalidade == null)
            {
                return NotFound();
            }
            return View(funcionalidade);
        }


        /// <summary>
        /// O método Edit recebe o objeto submetido pelo cliente para pesistência na base de dados.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="funcionalidade"></param>
        /// <returns>Página com a listagem de funcionalidades.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao")] Funcionalidade funcionalidade)
        {
            if (id != funcionalidade.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(funcionalidade);
            try
            {
                _funcionalidadeService.Atualizar(funcionalidade);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionalidadeExists(funcionalidade.Id).Result)
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
        /// O método Delete exibe os atributos da funcionalidade cadastrada, pontualmente pelo seu identificador único para confirmarção de exclusão.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Página com a listagem de funcionalidades.</returns>
        public async Task<IActionResult> Delete(int id)
        {
            var funcionalidade = await _funcionalidadeService.Detalhar(id);

            if (funcionalidade == null)
            {
                return NotFound();
            }

            return View(funcionalidade);
        }

        /// <summary>
        /// O método Delete recebe o objeto submetido pelo cliente para remoção na base de dados.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Página com a listagem de funcionalidades.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TempData["Message"] = await _funcionalidadeService.Excluir(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// O método FuncionalidadeExists verifica a existência de registro identificado pelo identificador único na base de dados.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Valor boleano referênte a existência ou não do registro identificado pelo idendificador único passado.</returns>
        private async Task<bool> FuncionalidadeExists(int id)
        {
            return await _funcionalidadeService.VerificarExistencia(id);
        }
    }
}
