namespace BaseDeProjetos.Models
{
    public class AbaFunil
    {
        public AbaFunil(string nome, bool abaAtiva, int quantidade)
        {
            Nome = nome;
            Action = "Index";
            Controller = "FunilDeVendas";
            RouteAba = LimparNomeAba(nome);
            AbaAtiva = abaAtiva;
            Quantidade = quantidade;
        }

        private string LimparNomeAba(string nome)
        {
            return nome.Replace(" ", "").ToLower();
        }

        public string Nome { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string RouteAba { get; set; }
        public bool AbaAtiva { get; set; }
        public int Quantidade { get; set; }
    }
}