function obterCampo(objeto, campo) {
    if (!objeto) return undefined;
    const campoCamelCase = campo.charAt(0).toLowerCase() + campo.slice(1);
    return objeto[campo] ?? objeto[campoCamelCase];
}

async function fetchIndicadoresMensais(casaSelecionada, anoSelecionado) {
    const resposta = await fetch(`/FunilDeVendas/GerarIndicadoresMensais/${encodeURIComponent(casaSelecionada)}/${anoSelecionado}`);

    if (!resposta.ok) {
        throw new Error(`Erro ao buscar indicadores mensais: ${resposta.status}`);
    }

    return resposta.json();
}

function obterPlotLineMesAtual(dados) {
    const mesAtual = obterCampo(dados, 'MesAtual');

    if (!mesAtual || mesAtual < 1 || mesAtual > 12) {
        return [];
    }

    return [{
        color: '#dc3545',
        dashStyle: 'Dash',
        width: 2,
        value: mesAtual,
        zIndex: 5
    }];
}

function criarGraficoExecutado(container, titulo, subtituloEixo, dados, grupo, cor, seriesExtras = []) {
    const meses = obterCampo(dados, 'Meses') ?? [];
    const anoAnterior = obterCampo(dados, 'AnoAnterior');
    const dadosGrupo = obterCampo(dados, grupo);
    const serieExecutado = obterCampo(dadosGrupo, 'Executado') ?? [];
    const serieExecutadoAnoAnterior = obterCampo(dadosGrupo, 'ExecutadoAnoAnterior') ?? [];
    const series = [{
        name: 'Executado',
        color: cor,
        data: serieExecutado
    }, {
        name: anoAnterior ? `Executado (${anoAnterior})` : 'Executado ano anterior',
        color: '#9aa0a6',
        data: serieExecutadoAnoAnterior,
        marker: {
            symbol: 'circle'
        }
    }, ...seriesExtras];

    Highcharts.chart(container, {
        chart: {
            type: 'line',
            spacingTop: 16,
            spacingRight: 18,
            spacingBottom: 12,
            spacingLeft: 8
        },
        title: {
            text: titulo,
            style: {
                fontSize: '13px',
                fontWeight: '700',
                letterSpacing: '0px',
                textTransform: 'uppercase'
            }
        },
        credits: {
            enabled: false
        },
        exporting: {
            enabled: false
        },
        xAxis: {
            categories: meses,
            title: {
                text: 'MES'
            },
            plotLines: obterPlotLineMesAtual(dados),
            gridLineWidth: 1
        },
        yAxis: {
            min: 0,
            allowDecimals: false,
            title: {
                text: subtituloEixo
            }
        },
        legend: {
            align: 'center',
            verticalAlign: 'top',
            itemStyle: {
                fontSize: '10px'
            }
        },
        tooltip: {
            shared: true,
            valueDecimals: 0
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true,
                    style: {
                        fontSize: '9px',
                        textOutline: 'none'
                    }
                },
                marker: {
                    enabled: true,
                    radius: 3,
                    symbol: 'square'
                }
            }
        },
        series: series
    });
}

function escaparHtml(valor) {
    return String(valor ?? '')
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#039;');
}

function formatarNumero(valor) {
    return Number(valor ?? 0).toLocaleString('pt-BR');
}

function formatarMoeda(valor) {
    return Number(valor ?? 0).toLocaleString('pt-BR', {
        style: 'currency',
        currency: 'BRL'
    });
}

function obterCorEquipe(indice) {
    const cores = ['#f45b93', '#00a7d8', '#55a630', '#ffc000', '#c65bd3', '#2bd9d1', '#ff1f1f', '#f1a6cf', '#d9d9d9', '#92d050'];
    return cores[indice % cores.length];
}

function renderizarTabelaEquipe(dados) {
    const container = document.getElementById('tabela-equipe-indicadores');
    if (!container) return;

    const equipe = obterCampo(dados, 'Equipe') ?? {};
    const linhas = obterCampo(equipe, 'Linhas') ?? [];
    const totais = obterCampo(equipe, 'Totais') ?? {};

    if (!linhas.length) {
        container.innerHTML = `
            <table class="indicadores-equipe-table">
                <thead>
                    <tr>
                        <th>Equipe</th>
                        <th>Contatos Realizados</th>
                        <th>Propostas Enviadas</th>
                        <th>Propostas Convertidas</th>
                        <th>Valor (prop. Enviadas)</th>
                        <th>Valor (prop. Convertidas)</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="empty-row"><td colspan="6">Nao ha dados de equipe para o ano selecionado.</td></tr>
                </tbody>
            </table>`;
        return;
    }

    const linhasHtml = linhas.map((linha, indice) => {
        const equipeNome = obterCampo(linha, 'Equipe');
        return `
            <tr>
                <td class="equipe-cell" style="background:${obterCorEquipe(indice)}">${escaparHtml(equipeNome)}</td>
                <td>${formatarNumero(obterCampo(linha, 'ContatosRealizados'))}</td>
                <td>${formatarNumero(obterCampo(linha, 'PropostasEnviadas'))}</td>
                <td>${formatarNumero(obterCampo(linha, 'PropostasConvertidas'))}</td>
                <td>${formatarMoeda(obterCampo(linha, 'ValorPropostasEnviadas'))}</td>
                <td>${formatarMoeda(obterCampo(linha, 'ValorPropostasConvertidas'))}</td>
            </tr>`;
    }).join('');

    container.innerHTML = `
        <table class="indicadores-equipe-table">
            <thead>
                <tr>
                    <th>Equipe</th>
                    <th>Contatos Realizados</th>
                    <th>Propostas Enviadas</th>
                    <th>Propostas Convertidas</th>
                    <th>Valor (prop. Enviadas)</th>
                    <th>Valor (prop. Convertidas)</th>
                </tr>
            </thead>
            <tbody>
                ${linhasHtml}
                <tr class="total-row">
                    <td></td>
                    <td>${formatarNumero(obterCampo(totais, 'ContatosRealizados'))}</td>
                    <td>${formatarNumero(obterCampo(totais, 'PropostasEnviadas'))}</td>
                    <td>${formatarNumero(obterCampo(totais, 'PropostasConvertidas'))}</td>
                    <td>${formatarMoeda(obterCampo(totais, 'ValorPropostasEnviadas'))}</td>
                    <td>${formatarMoeda(obterCampo(totais, 'ValorPropostasConvertidas'))}</td>
                </tr>
            </tbody>
        </table>`;
}
function renderizarIndicadoresMensais(dados) {
    const contatosRealizados = obterCampo(obterCampo(dados, 'ContatosRealizados'), 'Executado') ?? [];
    const serieContatosRealizados = {
        name: 'Contatos Realizados',
        color: '#ed7d31',
        data: contatosRealizados,
        marker: {
            symbol: 'diamond'
        }
    };

    criarGraficoExecutado('grafico-contatos', 'Contatos Realizados', 'TOTAL DE CONTATOS REALIZADOS', dados, 'ContatosRealizados', '#ed7d31');
    criarGraficoExecutado('grafico-propostas', 'Propostas Enviadas', 'QUANTIDADE TOTAL', dados, 'PropostasEnviadas', '#70ad47', [serieContatosRealizados]);
    criarGraficoExecutado('grafico-convertidas', 'Propostas Convertidas', 'QUANTIDADE TOTAL', dados, 'PropostasConvertidas', '#ffc000');
    renderizarTabelaEquipe(dados);
}

function navegarParaFiltrosIndicadores() {
    const casaSelecionada = obterCasasSelecionadasIndicadores();
    const anoSelecionado = document.getElementById('anoIndicadoresSelect').value;
    window.location.href = `/FunilDeVendas/Index/${encodeURIComponent(casaSelecionada)}/indicadores/${anoSelecionado}`;
}

function obterCasasSelecionadasIndicadores() {
    const casasCheckboxes = Array.from(document.querySelectorAll('input[name="casasIndicadores"]'));
    if (!casasCheckboxes.length) return casa || 'Todas';

    const selecionadas = casasCheckboxes
        .filter(checkbox => checkbox.checked)
        .map(checkbox => checkbox.value);

    if (!selecionadas.length || selecionadas.includes('Todas')) {
        return 'Todas';
    }

    return selecionadas.join(',');
}

function configurarMenuCasasIndicadores() {
    const todasCheckbox = document.getElementById('casaIndicadoresTodas');
    const casasCheckboxes = Array.from(document.querySelectorAll('input[name="casasIndicadores"]'))
        .filter(checkbox => checkbox.value !== 'Todas');

    if (todasCheckbox) {
        todasCheckbox.addEventListener('change', () => {
            if (todasCheckbox.checked) {
                casasCheckboxes.forEach(checkbox => {
                    checkbox.checked = false;
                });
            }
        });
    }

    casasCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', () => {
            if (checkbox.checked && todasCheckbox) {
                todasCheckbox.checked = false;
            }
        });
    });
}

document.addEventListener('DOMContentLoaded', () => {
    const anoSelect = document.getElementById('anoIndicadoresSelect');
    const aplicarCasas = document.getElementById('aplicarCasasIndicadores');

    configurarMenuCasasIndicadores();
    if (aplicarCasas) aplicarCasas.addEventListener('click', navegarParaFiltrosIndicadores);
    if (anoSelect) anoSelect.addEventListener('change', navegarParaFiltrosIndicadores);

    fetchIndicadoresMensais(casa, anoIndicadores)
        .then(renderizarIndicadoresMensais)
        .catch(erro => {
            console.error(erro);
            ['grafico-contatos', 'grafico-propostas', 'grafico-convertidas'].forEach(id => {
                const elemento = document.getElementById(id);
                if (elemento) elemento.innerHTML = '<div class="p-4 text-danger">Nao foi possivel carregar os indicadores.</div>';
            });
        });
});
