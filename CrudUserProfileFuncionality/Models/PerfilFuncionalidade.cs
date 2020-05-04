namespace CrudUserProfileFuncionality.Models
{
    public class PerfilFuncionalidade
    {
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; }
        public int FuncionalidadeId { get; set; }
        public Funcionalidade Funcionalidade { get; set; }
    }
}
