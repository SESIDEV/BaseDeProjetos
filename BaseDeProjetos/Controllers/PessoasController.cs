using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using BaseDeProjetos.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace BaseDeProjetos.Controllers
{
    [Authorize]
    public class PessoasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PessoasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pessoas
        [Route("Pessoas")]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;

                return View();
            }
            else
            {
                return View("Forbidden");
            }
        }


        public string dados()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return Helpers.Helpers.PuxarDadosUsuarios(_context);
            else
                return "403 Forbidden";

        }

        public string usuarioAtivo()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

                return usuario.UserName;
            }
            else
            {
                return "403 Forbidden";
            }
        }

        public string dictCompetencias()
        {
            IDictionary<string, string> competencias = new Dictionary<string, string>()
            {
                {"1", "Avaliação do Ciclo de Vida"},
                {"2", "Biocompostos"},
                {"3", "Bioeconomia"},
                {"4", "Biohidrogênio"},
                {"5", "Bioinformática"},
                {"6", "Biologia Molecular"},
                {"7", "Biorefinaria"},
                {"8", "Biosegurança"},
                {"9", "CO2 supercrítico"},
                {"10", "Cromatografia gasosa"},
                {"11", "Cromatografia líquida de alta eficiência"},
                {"12", "Economia Circular"},
                {"13", "Edição de imagens"},
                {"14", "Edição de vídeos"},
                {"15", "Eficiência Energética"},
                {"16", "Energias alternativas"},
                {"17", "Engenharia de Materiais"},
                {"18", "Engenharia de Software"},
                {"19", "Espectrometria de massas"},
                {"20", "Espectroscopia"},
                {"21", "Estatística"},
                {"22", "Estudo de Viabilidade Técnica e Econômica"},
                {"23", "Fluorescência"},
                {"24", "Fotofísica"},
                {"25", "Fotoquímica"},
                {"26", "Gestão de projetos de pesquisa"},
                {"27", "Imageamento Hiperespectral"},
                {"28", "Inteligência Artificial"},
                {"29", "Internacionalização"},
                {"30", "Modelagem Computacional"},
                {"31", "Modelagem de Requisitos"},
                {"32", "Modelagem molecular"},
                {"33", "Otimização de processos"},
                {"34", "Petroquímica"},
                {"35", "Planejamento Experimental (DOE)"},
                {"36", "Processos Extrativos e Purificação"},
                {"37", "Produtos Naturais"},
                {"38", "Programação de Aplicativos Móveis"},
                {"39", "Programação Web"},
                {"40", "Prospecção"},
                {"41", "Química analítica"},
                {"42", "Química de fluxo"},
                {"43", "Química Medicinal"},
                {"44", "Química Orgânica"},
                {"45", "Quimiometria"},
                {"46", "Redes de Saúde"},
                {"47", "Ressonância Magnética Nuclear"},
                {"48", "Saúde Humana"},
                {"49", "Sequenciamento de nova geração"},
                {"50", "Síntese orgânica"},
                {"51", "Sistemas Colaborativos"},
                {"52", "Técnicas de preparo de amostra"},
                {"53", "Tendências tecnológicas e Roadmap"}
            };
            return JsonSerializer.Serialize(competencias);
        }
    }
}