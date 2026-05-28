namespace NidusFront.Models
{
    public class UsuarioViewModel : PadraoViewModel
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public string? Celular { get; set; }
        public string? Perfil { get; set; }
        public byte[]? Foto { get; set; }

        public string? Cnpj { get; set; }
    }
}