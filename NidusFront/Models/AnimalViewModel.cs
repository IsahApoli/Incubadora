using System;
using Microsoft.AspNetCore.Http;

namespace NidusFront.Models
{
    public class AnimalViewModel : PadraoViewModel
    {
        public string? NomeEspecie { get; set; }

        // MUDANÇA AQUI: Agora recebe o que o usuário digitar como um texto puro
        public decimal TempMin { get; set; }
        public decimal TempMax { get; set; }

        public int UmidMin { get; set; }
        public int UmidMax { get; set; }
        public int LuzMin { get; set; }
        public int LuzMax { get; set; }
        public string? Tipo { get; set; }
        public int? IdUsuario { get; set; }

        public IFormFile? FotoUpload { get; set; }
        public byte[]? FotoEmBytes { get; set; }

        public string ImagemBase64
        {
            get
            {
                if (FotoEmBytes != null && FotoEmBytes.Length > 0)
                {
                    return Convert.ToBase64String(FotoEmBytes);
                }
                return string.Empty;
            }
        }
    }
}