const loadingClasses = ["spinner-border", "text-primary"];
async function fetchDadosDashboard() {
    let fetchedData;
    await fetch("/Home/ObterDadosReceita")
        .then(res => res.json())
        .then(data => fetchedData = data)
        .catch(err => console.error(`Houve um erro ao obter os dados da receita: ${err}`));
    return fetchedData;
}

function atualizarElementoDashboard(selector, valor, funcaoFormatacao = null) {
    let elem = document.querySelector(selector);
    elem.innerHTML = funcaoFormatacao ? funcaoFormatacao(valor) : valor;
    elem.classList.remove(...loadingClasses);
}

function setarEstiloBarra(selector, valor) {
    let elem = document.querySelector(selector);
    if (elem) {
        elem.style.width = `${valor}%`;
        elem.setAttribute("aria-valuenow", `${valor}`);
    } else {
        console.error(`Não existe elemento com selector: ${selector}`);
    }
}

function formatarData(stringData) {
    const date = new Date(stringData);
    const dia = date.getDate();
    const mes = date.getMonth();
    const ano = date.getFullYear();
    return `Data: ${dia}/${mes}/${ano}`;
}

function formatarDinheiro(valor) {
    return `R$ ${formatarValoresDashboard(valor)}`;
}

function formatarPorcentagem(valor) {
    let result = (valor * 100).toFixed(2);
    return `${result}%`;
}


function preencherDadosDashboard(data) {
    const dadosFinanceiros = data['dadosFinanceiros'];
    const dadosOperacionais = data['dadosOperacionais'];

    atualizarElementoDashboard("#data", data.data.data, formatarData);
    atualizarElementoDashboard("#receita", dadosFinanceiros['receitaTotal'], formatarDinheiro);
    atualizarElementoDashboard("#despesas", dadosFinanceiros['despesaTotal'], formatarDinheiro);
    atualizarElementoDashboard("#investimento", dadosFinanceiros['investimentoTotal'], formatarDinheiro);
    atualizarElementoDashboard("#sustentabilidade", dadosFinanceiros['sustentabilidade'], formatarPorcentagem);
    atualizarElementoDashboard("#volumeNegocios", dadosFinanceiros['volumeNegocios'], formatarDinheiro);
    atualizarElementoDashboard("#prospAtivas", dadosOperacionais['prospAtivas']);
    atualizarElementoDashboard("#projetos", dadosOperacionais['projetos']);
    atualizarElementoDashboard("#usuarios", dadosOperacionais['usuarios']);
    atualizarElementoDashboard("#empresas", dadosOperacionais['empresas']);
}

function inicializarChartProspeccoes(data) {
    let dadosProspeccoes = data['dadosProspeccoes'];
    // O de baixo possui id diferenciado para evitar conflito
    atualizarElementoDashboard("#prospAtivasGraph", dadosProspeccoes['prospAtivas']);
    atualizarElementoDashboard("#prospConcluidas", dadosProspeccoes['prospConcluidas']);
    atualizarElementoDashboard("#prospComProposta", dadosProspeccoes['prospComProposta']);
    atualizarElementoDashboard("#prospPlanejadas", dadosProspeccoes['prospPlanejadas']);

    setarEstiloBarra("#progProspAtivas", dadosProspeccoes['proporcaoAtivas'] * 100);
    setarEstiloBarra("#progProspConcluidas", dadosProspeccoes['proporcaoConcluidas'] * 100);
    setarEstiloBarra("#progProspComProposta", dadosProspeccoes['proporcaoComProposta'] * 100);
    setarEstiloBarra("#progProspPlanejadas", dadosProspeccoes['proporcaoPlanejadas'] * 100);
}

function inicializarBarChart(data) {
    let dadosLinhaDePesquisa = data['dadosGrafico']['linhasDePesquisa'];
    let labelsLinhaDePesquisa = data['dadosGrafico']['labels'];

    window.chartColors = {
        green: '#75c181',
        gray: '#a9b5c9',
        text: '#252930',
        border: '#e7e9ed'
    };

    const barChartConfig = {
        type: 'bar',
        data: {
            labels: labelsLinhaDePesquisa,
            datasets: [{
                label: 'ISI QV',
                backgroundColor: window.chartColors.green,
                borderColor: window.chartColors.green,
                borderWidth: 1,
                maxBarThickness: 16,
                data: dadosLinhaDePesquisa
            }]
        },
        options: {
            responsive: true,
            aspectRatio: 1.5,
            legend: {
                position: 'bottom',
                align: 'end',
            },
            title: {
                display: false,
                text: 'Chart.js Bar Chart Example'
            },
            tooltips: {
                mode: 'index',
                intersect: false,
                titleMarginBottom: 10,
                bodySpacing: 10,
                xPadding: 16,
                yPadding: 16,
                borderColor: window.chartColors.border,
                borderWidth: 1,
                backgroundColor: '#fff',
                bodyFontColor: window.chartColors.text,
                titleFontColor: window.chartColors.text,

            },
            scales: {
                xAxes: [{
                    ticks: {
                        autoSkip: false,
                        maxRotation: 90,
                        minRotation: 90
                    },
                    display: true,
                    gridLines: {
                        drawBorder: false,
                        color: window.chartColors.border,
                    },

                }],
                yAxes: [{
                    display: true,
                    gridLines: {
                        drawBorder: false,
                        color: window.chartColors.borders,
                    },
                }]
            }
        }
    }

    const barChart = document.getElementById('canvas-barchart').getContext('2d');
    window.myBar = new Chart(barChart, barChartConfig);
}

document.addEventListener("DOMContentLoaded", () => {
    fetchDadosDashboard()
        .then(data => {
            if (data) {
                preencherDadosDashboard(data);
                inicializarBarChart(data);
                inicializarChartProspeccoes(data);
            } else {
                console.error("Não há dados para exibir no dashboard.");
            }
        })
});
