using System;

namespace Incubadora.Models
{
        public class LoteViewModel : PadraoViewModel
        {
            public string CodigoLote { get; set; }

            public int QuantidadeOvos { get; set; }

            // Carimbo de data/hora do início da incubação 
            public DateTime DataEntrada { get; set; }

            // Ciclo de vida: INCUBANDO, NASCIDO ou DESCARTADO 
            public string StatusLote { get; set; }

            public int EspecieId { get; set; }

            public int IncubadoraId { get; set; }

            /* 
               PROPRIEDADES PARA JOIN (Requisito Obrigatório) 
               Estes campos são preenchidos na listagem para exibir nomes em vez de IDs
            */

            public string NomeEspecie { get; set; }

            public string NomeIncubadora { get; set; }
        }
}
