using System;
using Microsoft.AspNetCore.Http;

namespace Incubadora.Models
{
    public class EspecieViewModel : PadraoViewModel {
        public string NomeEspecie { get; set; }
        public decimal TemperaturaMin { get; set; }
        public decimal TemperaturaMax { get; set; }
        public decimal UmidadeMin { get; set; }
        public decimal UmidadeMax { get; set; }
        public decimal LuminosidadeMin { get; set; }
        public decimal LuminosidadeMax { get; set; }
        public int TempoIncubacao { get; set; }

        
        public IFormFile Imagem { get; set; } // Arquivo recebido do formulário
        public byte[] ImagemEmByte { get; set; } // Para salvar no banco
        public string ImagemEmBase64 => ImagemEmByte != null ? Convert.ToBase64String(ImagemEmByte) : string.Empty;
    }
}
