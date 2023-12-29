using BaseDeProjetos.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;

namespace BaseDeProjetos.Models
{
    public abstract class PesquisaOpiniao
    {
        [Key]
        public int IdPesquisa { get; set; }

        public string ProjetoId { get; set; }
        public double ResultadoFinal { get; set; }

        // Favor tirar depois caso necessário
        // O EntityFramework tá reclamando que List<int> n pode ser mapeado para nenhum tipo conhecido em SQL
        // The property 'PesquisaProjeto.RespostasIndividuais' could not be mapped,
        // because it is of type 'List<int>' which is not a supported primitive type or a valid entity type.
        // Either explicitly map this property, or ignore it using the '[NotMapped]' attribute or by using
        // 'EntityTypeBuilder.Ignore' in 'OnModelCreating'.

        [NotMapped]
        public List<int> RespostasIndividuais { get; set; }

        public string Comentarios { get; set; }
        public string RepresentacaoTextualQuestionario { get; set; }

        [NotMapped]
        public Dictionary<string, List<PerguntaSatisfacao>> PerguntasSatisfacao { get; set; }

        public void CalcularSatisficaoFinal()
        {
            List<int> resultados = new List<int>();
            foreach (KeyValuePair<string, List<PerguntaSatisfacao>> entrada in this.PerguntasSatisfacao)
            {
                foreach (PerguntaSatisfacao satisfacao in entrada.Value)
                {
                    if (satisfacao.GetType() == typeof(PerguntaLikert))
                        resultados.Add((int)((PerguntaLikert)satisfacao).Resposta);
                }
            }

            this.ResultadoFinal = resultados.Average();
        }

        public void DesserializarPerguntas()
        {
            this.PerguntasSatisfacao = JsonSerializer.Deserialize<Dictionary<string, List<PerguntaSatisfacao>>>(this.RepresentacaoTextualQuestionario);
        }

        public void SerializarPerguntas()
        {
            this.RepresentacaoTextualQuestionario = JsonSerializer.Serialize(this.PerguntasSatisfacao);
        }

        public List<int> ObterHashCodesQuestoes()
        {
            List<int> hashes = new List<int>();
            foreach (KeyValuePair<string, List<PerguntaSatisfacao>> kv in this.PerguntasSatisfacao.Where(p => p.GetType() == typeof(PerguntaLikert)))
            {
                kv.Value.ForEach((v) => hashes.Add(v.Pergunta.GetHashCode()));
            }

            return hashes;
        }
    }

    public class PesquisaProjeto : PesquisaOpiniao
    {
        public PesquisaProjeto()
        {
            if (this.RepresentacaoTextualQuestionario != null)
            {
                this.DesserializarPerguntas();
            }
            else
            {
                this.PerguntasSatisfacao = PesquisaProjeto.InstanciarPerguntas();
            }

            this.SerializarPerguntas(); //Mantém uma representação em memória do questionário
        }

        public PesquisaProjeto(bool InicioFrio) : this()
        {
            if (InicioFrio) // Instanciado como pergunta a ser preenchida
                this.PerguntasSatisfacao = PesquisaProjeto.InstanciarPerguntas();
        }

        private static Dictionary<string, List<PerguntaSatisfacao>> InstanciarPerguntas()
        {
            Dictionary<string, List<PerguntaSatisfacao>> perguntas = new Dictionary<string, List<PerguntaSatisfacao>>
            {
                ["Quão Satisfeito você está com as entregas do Projeto"] = new List<PerguntaSatisfacao>
            {
                new PerguntaLikert() { Pergunta = "Com a qualidade do conteúdo" },
                new PerguntaLikert() { Pergunta = "Com a apresentação dos resultados" },
                new PerguntaLikert() { Pergunta = "Com o cumprimento dos requisitos do projeto" }
            },

                ["Quão satisfeito você está na interação com a equipe do projeto?"] = new List<PerguntaSatisfacao>
            {
                new PerguntaLikert() { Pergunta = "Com a disponibilidade da equipe" },
                new PerguntaLikert() { Pergunta = "Com o comprometimento da equipe" },
                new PerguntaLikert() { Pergunta = "Com as habilidades de comunicação da equipe" }
            },

                ["Quão satisfeito você está com as competências profissionais da equipe do projeto?"] = new List<PerguntaSatisfacao>
            {
                new PerguntaLikert() { Pergunta = "Com a gestão do projeto" },
                new PerguntaLikert() { Pergunta = "Com as competências tecnológicas da equipe" },
                new PerguntaLikert() { Pergunta = "Com o conhecimento na área relacionado ao projeto" }
            },

                ["Quão satisfeito você está com o cumprimento do cronograma do projeto?"] = new List<PerguntaSatisfacao>
            {
                new PerguntaLikert() { Pergunta = "Com o cumprimento de prazos" },
                new PerguntaLikert() { Pergunta = "Com a negociação de adiamentos e mudanças" },
                new PerguntaLikert() { Pergunta = "Com os prazos do cronograma proposto" }
            },
                ["Quão satisfeito você está com o impacto do projeto?"] = new List<PerguntaSatisfacao>
            {
                new PerguntaLikert() { Pergunta = "Com o valor/benefício" },
                new PerguntaLikert() { Pergunta = "Com a relação preço-desempenho" },
                new PerguntaLikert() { Pergunta = "Com o impacto esperado e percebido" }
            },
                ["De 1 a 5 (sendo 1 pouco importante e 5 muitos importante), classifique seu grau de importância com relação aos seguintes aspectos?"] = new List<PerguntaSatisfacao>
            {
                new PerguntaLikert() { Pergunta = "Entregas de projeto" },
                new PerguntaLikert() { Pergunta = "Interação" },
                new PerguntaLikert() { Pergunta = "Competência profissional" },
                new PerguntaLikert() { Pergunta = "Prazos e entrega" },
                new PerguntaLikert() { Pergunta = "Impacto e benefícios" }
            },

                ["Finalizando a pesquisa de opinião"] = new List<PerguntaSatisfacao>{
                new PerguntaBool() { Pergunta = "Você executaria projetos com o instituto novamente?"},
                new PerguntaLikert() { Pergunta = "Qual é a probabilidade de você recomendar o instituto para parceiros de negócios ou colegas?"},
                new PerguntaTexto() { Pergunta = "Gostaria de deixar algum comentário adicional? Escreva abaixo"}
            }
            };

            return perguntas;
        }
    }

    public class PerguntaSatisfacao
    {
        public string Pergunta { get; set; } = "Pergunta não definida";
    }

    public class PerguntaLikert : PerguntaSatisfacao
    {
        public ResultadoOpiniao Resposta { get; set; } = ResultadoOpiniao.Regular;
    }

    public class PerguntaBool : PerguntaSatisfacao
    {
        public bool Resposta { get; set; } = true;
    }

    public class PerguntaTexto : PerguntaSatisfacao
    {
        public string Resposta { get; set; } = "";
    }
}