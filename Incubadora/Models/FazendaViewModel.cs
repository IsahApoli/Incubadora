namespace Incubadora.Models
{
    public class FazendaViewModel : PadraoViewModel {
        public string NomeFazenda { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Proprietario { get; set; }
        public string Telefone { get; set; }
        public int EmpresaId { get; set; }
        public string NomeEmpresaJoin { get; set; } // Para exibir na listagem com Join [4]
    }
}
