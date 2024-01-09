async function fetchDadosIndicadoresProspeccao() {
    let fetchedData;
    await fetch(`/FunildeVendas/GerarIndicadoresProsp/${casa}`)
        .then(res => res.json())
        .then(data => fetchedData = data)
        .catch(err => console.error(`Houve um erro ao obter os dados dos indicadores: ${err}`))
    return fetchedData;
}

function preencherDadosIndicadoresProsp(dadosProsp) {
    let identificadores = ["EmpresasProspectadas", "TempoMedioContato", "ProspContatoInicial", "PercentualInfrutiferas"]
    identificadores.forEach(identificador => {


        switch (identificador) {
            case 'EmpresasProspectadas':
                document.getElementById(identificador).innerText = dadosProsp[identificador]
                break
            case 'TempoMedioContato':
                document.getElementById(identificador).innerText = dadosProsp[identificador].toFixed(1)
                break
            case 'ProspContatoInicial':
                document.getElementById(identificador).innerText = dadosProsp[identificador]
                break
            case 'PercentualInfrutiferas':
                document.getElementById(identificador).innerText = dadosProsp[identificador].toFixed(2) + '%'
                break
            default:
                console.log("erro")


        }
    })
}

async function fetchDadosStatusProspComProposta() {
    let fetchedData;
    await fetch(`/FunildeVendas/GerarStatusProspComProposta/${casa}`)
        .then(res => res.json())
        .then(data => fetchedData = data)
        .catch(err => console.error(`Houve um erro ao obter os dados dos indicadores: ${err}`))
    return fetchedData;
}

function preencherDadosStatusProspComProposta(dadosProsp) {
    let identificadores = ["TaxaConversao", "TicketMedioProsp", "PropostasEnviadas", "ProjetosContratados"]
    identificadores.forEach(identificador => {


        switch (identificador) {
            case 'TaxaConversao':
                document.getElementById(identificador).innerText = dadosProsp[identificador].toFixed(2) + '%'
                break
            case 'TicketMedioProsp':
                document.getElementById(identificador).innerText = dadosProsp[identificador].toLocaleString('pt-BR', { style: 'currency', currency: 'BRL'})
                break
            case 'PropostasEnviadas':
                document.getElementById(identificador).innerText = dadosProsp[identificador]
                break
            case 'ProjetosContratados':
                document.getElementById(identificador).innerText = dadosProsp[identificador]
                break
            default:
                console.log("erro")


        }
    })
}


async function fetchDadosStatusGeralProspPizza() {
    let fetchedData;
    await fetch(`/FunildeVendas/GerarStatusGeralProspPizza/${casa}`)
        .then(res => res.json())
        .then(data => fetchedData = data)
        .catch(err => console.error(`Houve um erro ao obter os dados dos indicadores: ${err}`))
    return fetchedData;
}

function preencherDadosStatusGeralProspPizza(dadosProsp) {
    let pizzaDesistencia = Highcharts.chart('pizza_desistencia', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: null
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                }
            }
        },
        series: [{
            size: '60%',
            name: 'Propostas',
            colorByPoint: true,
            data: [{
                name: 'Em andamento',
                y: dadosProsp['PercentualEmAndamento'],
                drilldown: null
            }, {
                name: 'Convertidas',
                y: dadosProsp['PercentualConvertidas'],
                drilldown: null
            },
            {
                name: 'Não Convertidas',
                y: dadosProsp['PercentualNaoConvertidas'],
                drilldown: null
            },
            {
                name: 'Canceladas',
                y: dadosProsp['PercentualCanceladas'],
                drilldown: null
            },
            ],
        }],
    });
}

async function fetchDadosGraficoBarraTipoContratacao() {
    let fetchedData;
    await fetch(`/FunildeVendas/GerarGraficoBarraTipoContratacao/${casa}`)
        .then(res => res.json())
        .then(data => fetchedData = data)
        .catch(err => console.error(`Houve um erro ao obter os dados dos indicadores: ${err}`))
    return fetchedData;
}
function preencherDadosGraficoBarraTipoContratacao(dadosProsp) {
    console.log(dadosProsp)

    let comProposta = "ComProposta"
    let contatoInicial = "ContatoInicial"
    let emDiscussao = "EmDiscussao"

    const seriespadrao = [
        { name: 'Contratação Direta', data: [dadosProsp[contatoInicial]["ContratacaoDireta"], dadosProsp[emDiscussao]["ContratacaoDireta"], dadosProsp[comProposta]["ContratacaoDireta"]] },
        { name: 'Editais de Inovação SESI/SENAI', data: [dadosProsp[contatoInicial]["EditaisInovacao"], dadosProsp[emDiscussao]["EditaisInovacao"], dadosProsp[comProposta]["EditaisInovacao"]] },
        { name: 'Agências de Fomento', data: [dadosProsp[contatoInicial]["AgenciaFomento"], dadosProsp[emDiscussao]["AgenciaFomento"], dadosProsp[comProposta]["AgenciaFomento"]] },
        { name: 'Embrapii', data: [dadosProsp[contatoInicial]["Embrapii"], dadosProsp[emDiscussao]["Embrapii"], dadosProsp[comProposta]["Embrapii"]] },
        { name: 'A definir', data: [dadosProsp[contatoInicial]["Definir"], dadosProsp[emDiscussao]["Definir"], dadosProsp[comProposta]["Definir"]] },
        { name: 'Parceiros de Edital', data: [dadosProsp[contatoInicial]["ParceiroEdital"], dadosProsp[emDiscussao]["ParceiroEdital"], dadosProsp[comProposta]["ParceiroEdital"]["Parceiro_ComProposta"]] },
        { name: 'ANP/ANEEL', data: [dadosProsp[contatoInicial]["ANP_ANEEL"], dadosProsp[emDiscussao]["ANP_ANEEL"], dadosProsp[comProposta]["ANP_ANEEL"]] }
    ];
    
    let chart1 = Highcharts.chart('funil', {
        chart: { type: 'column', allowMutatingData: false },
        title: { text: null },
        xAxis: { categories: ["Contato inicial", "Em discussão", "Com Proposta"], },
        yAxis: { min: 0, title: { text: 'Ocorrências' } },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f} prospecções</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: { column: { pointPadding: 0.2, borderWidth: 0 } },
        series: seriespadrao
    })
}

async function fetchDadosStatusProspeccoesPropostaPizza() {
    let fetchedData;
    await fetch(`/FunildeVendas/GerarStatusProspPropostaPizza/${casa}`)
        .then(res => res.json())
        .then(data => fetchedData = data)
        .catch(err => console.error(`Houve um erro ao obter os dados dos indicadores: ${err}`))
    return fetchedData;
}
function preencherDadosStatusProspeccoesPropostaPizza(dadosProsp) {

    let pizzaConversao = Highcharts.chart('pizza_conversao', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: 'Status de Prospecções com propostas'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.y} prospecções'
                }
            }
        },
        series: [{
            size: '60%',
            name: 'Propostas',
            colorByPoint: true,
            data: [{
                name: 'Em andamento',
                y: dadosProsp['QuantidadeEmAndamento'],
            }, {
                name: 'Convertidas',
                y: dadosProsp['QuantidadeConvertidas'],
            }, {
                name: 'Não convertidas',
                y: dadosProsp['QuantidadeNaoConvertidas'],
            }]
        }]
    });

}



document.addEventListener("DOMContentLoaded", () => {
    fetchDadosIndicadoresProspeccao()
        .then(data => {
            if (data) {
                preencherDadosIndicadoresProsp(data);
            } else {
                console.error("Não há dados para exibir no dashboard.");
            }
        });
    fetchDadosStatusProspComProposta()
        .then(data => {
            if (data) {
                preencherDadosStatusProspComProposta(data);
            } else {
                console.error("Não há dados para exibir no dashboard.");
            }
        });
    fetchDadosStatusGeralProspPizza()
        .then(data => {
            if (data) {
                preencherDadosStatusGeralProspPizza(data);
            } else {
                console.error("Não há dados para exibir no dashboard.");
            }
        });
    fetchDadosStatusProspeccoesPropostaPizza()
        .then(data => {
            if (data) {
                preencherDadosStatusProspeccoesPropostaPizza(data);
            } else {
                console.error("Não há dados para exibir no dashboard.");
            }
        });
    fetchDadosGraficoBarraTipoContratacao()
        .then(data => {
            if (data) {
                preencherDadosGraficoBarraTipoContratacao(data);
            } else {
                console.error("Não há dados para exibir no dashboard.");
            }
        });
});



