using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrudUserProfileFuncionality.Models
{
    public class Perfil
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public List<PerfilFuncionalidade> PerfilFuncionalidade { get; set; }
    }
}
