namespace NidusFront.Models
{
    public class IncubadoraViewModel : PadraoViewModel
    {
        public string? NomeIncubadora { get; set; }
        public int QuantidadeOvos { get; set; }
        public string? Status { get; set; } // 'Ativa' ou 'Inativa'

        // Chaves Estrangeiras (Vínculos)
        public int IdAnimal { get; set; }
        public int IdUsuario { get; set; }

        public int IdFazenda { get; set; }

        public string? FotoAnimalBase64 { get; set; }

        // Propriedades Auxiliares (Usadas apenas para exibição na tela)
        public string? AnimalVinculado { get; set; }
        public string? Responsavel { get; set; }
    }
}