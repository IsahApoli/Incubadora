namespace NidusFront.Models
{
    // A herança aqui puxa o "Id" lá do PadraoViewModel
    public class FazendaViewModel : PadraoViewModel
    {
        public string? NomeFantasia { get; set; }
        public string? Cnpj { get; set; }
    }
}