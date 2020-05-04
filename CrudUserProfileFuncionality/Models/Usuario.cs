using System;
using System.ComponentModel.DataAnnotations;

namespace CrudUserProfileFuncionality.Models
{
    public class Usuario
    {
        public Guid Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Email { get; set; }
        public Perfil Perfil { get; set; }
    }
}
