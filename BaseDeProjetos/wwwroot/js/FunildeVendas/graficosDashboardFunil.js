async function fetchDadosIndicadoresProspeccao() {
    let fetchedData;
    await fetch("/FunildeVendas/GerarIndicadoresProsp")
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
    await fetch("/FunildeVendas/GerarStatusProspComProposta")
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
    await fetch("/FunildeVendas/GerarStatusGeralProspPizza")
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

async function fetchDadosStatusProspeccoesPropostaPizza() {
    let fetchedData;
    await fetch("/FunildeVendas/GerarStatusProspPropostaPizza")
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
});



