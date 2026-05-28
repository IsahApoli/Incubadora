using System;

namespace NidusFront.Models
{
    public class TelemetriaViewModel : PadraoViewModel
    {
        public int IdIncubadora { get; set; }

        public DateTime DataHora { get; set; }

        public decimal TemperaturaAtual { get; set; }

        public int UmidadeAtual { get; set; }

        // Pode ser "OK", "Alerta" ou "Crítico" dependendo da leitura
        public string? StatusGeral { get; set; }
    }
}
