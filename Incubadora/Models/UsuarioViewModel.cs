namespace Incubadora.Models
{
    public class UsuarioViewModel : PadraoViewModel {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string TipoUsuario { get; set; } // ADMIN_EMPRESA, ADMIN_FAZENDA, FUNCIONARIO [5]
        public int? FazendaId { get; set; }
    }
}
