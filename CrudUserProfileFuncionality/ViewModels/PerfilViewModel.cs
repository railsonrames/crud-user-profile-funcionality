using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CrudUserProfileFuncionality.ViewModels
{
    public class PerfilViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string[] SelectedFuncionalidades { get; set; }
        public IEnumerable<SelectListItem> Funcionalidades { get; set; }
    }
}
