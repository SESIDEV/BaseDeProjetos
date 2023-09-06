using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDeProjetos.Models
{
    public abstract class PesquisaOpiniao
    {
        [Key]
        public int IdPesquisa { get; set; }
        public string ProjetoId { get; set; }
        public ResultadoOpiniao ResultadoFinal { get; set; }

        [NotMapped]
        public Dictionary<string, List<PerguntaSatisfacao>> PerguntasSatisfacao { get; set; }

        public void calcularSatisficaoFinal()
        {
            //TODO
        }
    }

    public class PesquisaProjeto : PesquisaOpiniao
    {

        public PesquisaProjeto()
        {
            this.PerguntasSatisfacao = PesquisaProjeto.InstanciarPerguntas();
        }

        private static Dictionary<string, List<PerguntaSatisfacao>> InstanciarPerguntas()
        {

            Dictionary<string, List<PerguntaSatisfacao>> perguntas = new Dictionary<string, List<PerguntaSatisfacao>>();

            perguntas["Quão Satisfeito você está com as entregas do Projeto"] = new List<PerguntaSatisfacao>
            {
                new PerguntaLikert() { Pergunta = "Com a qualidade do conteúdo" },
                new PerguntaLikert() { Pergunta = "Com a apresentação dos resultados" },
                new PerguntaLikert() { Pergunta = "Com o cumprimento dos requisitos do projeto" }
            };

            perguntas["Quão satisfeito você está na interação com a equipe do projeto?"] = new List<PerguntaSatisfacao>
            {
                new PerguntaLikert() { Pergunta = "Com a disponibilidade da equipe" },
                new PerguntaLikert() { Pergunta = "Com o comprometimento da equipe" },
                new PerguntaLikert() { Pergunta = "Com as habilidades de comunicação da equipe" }
            };

            perguntas["Quão satisfeito você está com as competências profissionais da equipe do projeto?"] = new List<PerguntaSatisfacao>
            {
                new PerguntaLikert() { Pergunta = "Com a gestão do projeto" },
                new PerguntaLikert() { Pergunta = "Com as competências tecnológicas da equipe" },
                new PerguntaLikert() { Pergunta = "Com o conhecimento na área relacionado ao projeto" }
            };

            perguntas["Quão satisfeito você está com o cumprimento do cronograma do projeto?"] = new List<PerguntaSatisfacao>
            {
                new PerguntaLikert() { Pergunta = "Com o cumprimento de prazos" },
                new PerguntaLikert() { Pergunta = "Com a negociação de adiamentos e mudanças" },
                new PerguntaLikert() { Pergunta = "Com os prazos do cronograma proposto" }
            };
            perguntas["Quão satisfeito você está com o impacto do projeto?"] = new List<PerguntaSatisfacao>
            {
                new PerguntaLikert() { Pergunta = "Com o valor/benefício" },
                new PerguntaLikert() { Pergunta = "Com a relação preço-desempenho" },
                new PerguntaLikert() { Pergunta = "Com o impacto esperado e percebido" }
            };
            perguntas["De 1 a 5 (sendo 1 pouco importante e 5 muitos importante), classifique seu grau de importância com relação aos seguintes aspectos?"] = new List<PerguntaSatisfacao>
            {
                new PerguntaLikert() { Pergunta = "Entregas de projeto" },
                new PerguntaLikert() { Pergunta = "Interação" },
                new PerguntaLikert() { Pergunta = "Competência profissional" },
                new PerguntaLikert() { Pergunta = "Prazos e entrega" },
                new PerguntaLikert() { Pergunta = "Impacto e benefícios" }
            };

            perguntas["Finalizando a pesquisa de opinião"] = new List<PerguntaSatisfacao>{
                new PerguntaBool() { Pergunta = "Você executaria projetos com o instituto novamente?"},
                new PerguntaLikert() { Pergunta = "Qual é a probabilidade de você recomendar o instituto para parceiros de negócios ou colegas?"},
                new PerguntaTexto() { Pergunta = "Gostaria de deixar algum comentário adicional? Escreva abaixo"}
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
