const classesLoading = ["spinner-border", "text-white"];

function carregarGraficoTemporal(dadosRetornados) {
    let labels = null;
    let values = null;

    if (dadosRetornados) {
        labels = dadosRetornados.Labels;
        values = dadosRetornados.Valores;
    }
    else {
        console.error("Os dados retornados estão nulos!");
    }

    let name = `participacao`;

    Highcharts.chart(name, {
        chart: {
            type: 'line'
        },
        title: {
            text: 'Valor das Propostas ao Longo do Tempo'
        },
        subtitle: {
            text: 'Somatório cumulativo'
        },
        xAxis: {
            categories: labels,
        },
        yAxis: {
            title: {
                text: 'Valor em R$'
            }
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true,
                    formatter: function () {
                        return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(this.y);
                    }
                },
                enableMouseTracking: true
            }
        },
        tooltip: {
            formatter: function () {
                const formattedValue = new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(this.y);
                return `Valor das Propostas: ${formattedValue}`;
            }
        },
        series: [{
            name: 'Valor das Propostas',
            data: values
        }]
    });
}

function esconderBotao(idUsuario) {
    document.querySelector(`#button-${idUsuario}`).style.display = 'none';
}

function exibirLoading(idUsuario) {
    const internalDiv = document.querySelector(`#internalDiv-${idUsuario}`);
    if (internalDiv) {
        internalDiv.classList.add(...classesLoading);
        internalDiv.innerHTML = '';
    } else {
        console.error(`Elemento com id 'internalDiv-${idUsuario}' não foi encontrado.`);
    }
}

function esconderLoading(idUsuario) {
    const internalDiv = document.querySelector(`#internalDiv-${idUsuario}`);
    if (internalDiv) {
        internalDiv.classList.remove(...classesLoading);
    }
}

async function puxarDadosGraficoTemporal(idUsuario) {
    let response = await fetch(`${location.origin}/Participacao/RetornarDadosGraficoTemporal/${idUsuario}`);
    const data = await response.json();
    return data;
}

function exibirDados(idUsuario) {
    exibirLoading(idUsuario);
    puxarDadosGraficoTemporal(idUsuario).then(data => {
        esconderBotao(idUsuario);
        esconderLoading(idUsuario);
        carregarGraficoTemporal(data);
    });
}