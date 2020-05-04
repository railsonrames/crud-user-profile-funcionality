using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrudUserProfileFuncionality.ViewModels
{
    public class UsuarioViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        [Display(Name = "Perfil")]
        public string SelectedPerfil { get; set; }
        public IEnumerable<SelectListItem> Perfis { get; set; }
    }
}
