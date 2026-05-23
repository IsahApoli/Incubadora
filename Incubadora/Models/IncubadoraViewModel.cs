using System;

namespace Incubadora.Models
{
    public class IncubadoraViewModel : PadraoViewModel {
        public string NomeIncubadora { get; set; }
        public string Modelo { get; set; }
        public string Localizacao { get; set; }
        public string Status { get; set; }
        public int CapacidadeMaxima { get; set; }
        public DateTime DataInstalacao { get; set; }
        public int FazendaId { get; set; }
    }
}
