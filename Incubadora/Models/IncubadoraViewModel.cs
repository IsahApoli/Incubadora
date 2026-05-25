using System;

namespace Incubadora.Models
{
    public class IncubadoraViewModel : PadraoViewModel {
        public string NomeIncubadora { get; set; }
        public string Status { get; set; }
        public int QtdOvos { get; set; }
        public DateTime DataIncubação { get; set; }
        public int FazendaId { get; set; }

    }
}
