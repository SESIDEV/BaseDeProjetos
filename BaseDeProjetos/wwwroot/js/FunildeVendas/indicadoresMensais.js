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
    const seriePlanejado = obterCampo(dadosGrupo, 'Planejado') ?? [];
    const serieExecutado = obterCampo(dadosGrupo, 'Executado') ?? [];
    const serieExecutadoAnoAnterior = obterCampo(dadosGrupo, 'ExecutadoAnoAnterior') ?? [];
    const temPlanejado = seriePlanejado.some(valor => valor !== null && valor !== undefined);
    const series = [];

    if (temPlanejado) {
        series.push({
            name: 'Planejado',
            color: '#1f4e79',
            dashStyle: 'Dash',
            data: seriePlanejado,
            marker: {
                symbol: 'circle'
            }
        });
    }

    series.push({
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
    }, ...seriesExtras);

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

function formatarPercentual(valor) {
    return `${Number(valor ?? 0).toLocaleString('pt-BR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    })}%`;
}

function formatarNumeroInput(valor) {
    return Number(valor ?? 0).toLocaleString('pt-BR', {
        maximumFractionDigits: 2
    });
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
                <td class="equipe-cell">${escaparHtml(equipeNome)}</td>
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

function renderizarTabelaTaxas(dados) {
    const container = document.getElementById('tabela-taxas-indicadores');
    if (!container) return;

    const taxas = obterCampo(dados, 'Taxas') ?? {};
    const linhas = obterCampo(taxas, 'Linhas') ?? [];
    const totais = obterCampo(taxas, 'Totais') ?? {};

    if (!linhas.length) {
        container.innerHTML = `
            <table class="indicadores-equipe-table">
                <thead>
                    <tr>
                        <th>Taxas</th>
                        <th>Proposi&ccedil;&atilde;o</th>
                        <th>Convers&atilde;o</th>
                        <th>Sucesso</th>
                        <th>Contribui&ccedil;&atilde;o de Receita Gerada</th>
                        <th>Assertividade de Propostas</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="empty-row"><td colspan="6">Nao ha dados de taxas para o ano selecionado.</td></tr>
                </tbody>
            </table>`;
        return;
    }

    const linhasHtml = linhas.map((linha, indice) => {
        const equipeNome = obterCampo(linha, 'Equipe');
        return `
            <tr>
                <td class="equipe-cell">${escaparHtml(equipeNome)}</td>
                <td>${formatarPercentual(obterCampo(linha, 'Proposicao'))}</td>
                <td>${formatarPercentual(obterCampo(linha, 'Conversao'))}</td>
                <td>${formatarPercentual(obterCampo(linha, 'Sucesso'))}</td>
                <td>${formatarPercentual(obterCampo(linha, 'ContribuicaoReceitaGerada'))}</td>
                <td>${formatarPercentual(obterCampo(linha, 'AssertividadePropostas'))}</td>
            </tr>`;
    }).join('');

    container.innerHTML = `
        <table class="indicadores-equipe-table">
            <thead>
                <tr>
                    <th>Taxas</th>
                    <th>Proposi&ccedil;&atilde;o</th>
                    <th>Convers&atilde;o</th>
                    <th>Sucesso</th>
                    <th>Contribui&ccedil;&atilde;o de Receita Gerada</th>
                    <th>Assertividade de Propostas</th>
                </tr>
            </thead>
            <tbody>
                ${linhasHtml}
                <tr class="total-row">
                    <td></td>
                    <td>${formatarPercentual(obterCampo(totais, 'Proposicao'))}</td>
                    <td>${formatarPercentual(obterCampo(totais, 'Conversao'))}</td>
                    <td>${formatarPercentual(obterCampo(totais, 'Sucesso'))}</td>
                    <td>${formatarPercentual(obterCampo(totais, 'ContribuicaoReceitaGerada'))}</td>
                    <td>${formatarPercentual(obterCampo(totais, 'AssertividadePropostas'))}</td>
                </tr>
            </tbody>
        </table>`;
}

function renderizarTabelaContatosPesquisador(dados) {
    const container = document.getElementById('tabela-contatos-pesquisador-indicadores');
    if (!container) return;

    const contatosPesquisador = obterCampo(dados, 'ContatosPesquisador') ?? {};
    const linhas = obterCampo(contatosPesquisador, 'Linhas') ?? [];
    const totais = obterCampo(contatosPesquisador, 'Totais') ?? {};
    const pesquisadoresDisponiveis = obterCampo(contatosPesquisador, 'PesquisadoresDisponiveis') ?? [];
    const podeEditarArraste = obterCampo(contatosPesquisador, 'PodeEditarArraste') === true;
    const meses = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];
    const controleAdicionar = podeEditarArraste
        ? `
            <div class="indicadores-add-pesquisador">
                <select class="form-select form-select-sm" id="novoPesquisadorContatos" ${pesquisadoresDisponiveis.length ? '' : 'disabled'}>
                    <option value="">Adicionar pesquisador</option>
                    ${pesquisadoresDisponiveis.map(pesquisador => `
                        <option value="${escaparHtml(obterCampo(pesquisador, 'PesquisadorId'))}">
                            ${escaparHtml(obterCampo(pesquisador, 'Pesquisador'))}
                        </option>`).join('')}
                </select>
                <button type="button" class="btn app-btn-secondary btn-sm" id="adicionarPesquisadorContatos" ${pesquisadoresDisponiveis.length ? '' : 'disabled'}>Adicionar</button>
            </div>`
        : '';

    if (!linhas.length) {
        container.innerHTML = `
            ${controleAdicionar}
            <table class="indicadores-equipe-table">
                <thead>
                    <tr>
                        <th>Pesquisador lider</th>
                        <th>Arraste</th>
                        ${meses.map(mes => `<th>${mes}</th>`).join('')}
                        <th>Total</th>
                        <th>%</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="empty-row"><td colspan="16">Nao ha contatos por pesquisador para o ano selecionado.</td></tr>
                </tbody>
            </table>`;
        configurarAdicionarPesquisadorContatos(container);
        return;
    }

    const linhasHtml = linhas.map(linha => {
        const pesquisadorId = obterCampo(linha, 'PesquisadorId');
        const pesquisador = obterCampo(linha, 'Pesquisador');
        const mesesLinha = obterCampo(linha, 'Meses') ?? [];
        const arraste = obterCampo(linha, 'Arraste') ?? 0;
        const inputSomenteLeitura = !podeEditarArraste || !pesquisadorId;
        const arrasteHtml = `
            <input class="arraste-input"
                   type="text"
                   inputmode="decimal"
                   value="${formatarNumeroInput(arraste)}"
                   data-pesquisador-id="${escaparHtml(pesquisadorId)}"
                   ${inputSomenteLeitura ? 'readonly' : ''} />`;

        return `
            <tr>
                <td class="equipe-cell">${escaparHtml(pesquisador)}</td>
                <td>${arrasteHtml}</td>
                ${mesesLinha.map(valor => `<td>${formatarNumero(valor)}</td>`).join('')}
                <td>${formatarNumero(obterCampo(linha, 'Total'))}</td>
                <td>${formatarPercentual(obterCampo(linha, 'Percentual'))}</td>
            </tr>`;
    }).join('');

    const mesesTotais = obterCampo(totais, 'Meses') ?? [];
    container.innerHTML = `
        ${controleAdicionar}
        <table class="indicadores-equipe-table">
            <thead>
                <tr>
                    <th>Pesquisador lider</th>
                    <th>Arraste</th>
                    ${meses.map(mes => `<th>${mes}</th>`).join('')}
                    <th>Total</th>
                    <th>%</th>
                </tr>
            </thead>
            <tbody>
                ${linhasHtml}
                <tr class="total-row">
                    <td>Total</td>
                    <td>${formatarNumero(obterCampo(totais, 'Arraste'))}</td>
                    ${mesesTotais.map(valor => `<td>${formatarNumero(valor)}</td>`).join('')}
                    <td>${formatarNumero(obterCampo(totais, 'Total'))}</td>
                    <td>${formatarPercentual(obterCampo(totais, 'Percentual'))}</td>
                </tr>
            </tbody>
        </table>`;

    container.querySelectorAll('.arraste-input:not([readonly])').forEach(input => {
        input.addEventListener('change', () => salvarArrasteContatosPesquisador(input));
    });
    configurarAdicionarPesquisadorContatos(container);
}

function configurarAdicionarPesquisadorContatos(container) {
    const botao = container.querySelector('#adicionarPesquisadorContatos');
    const select = container.querySelector('#novoPesquisadorContatos');
    if (!botao || !select) return;

    botao.addEventListener('click', async () => {
        if (!select.value) return;

        botao.disabled = true;
        select.disabled = true;

        try {
            await salvarArrasteContatosPesquisadorValor(select.value, '0');
        } catch (erro) {
            console.error(erro);
            botao.disabled = false;
            select.disabled = false;
        }
    });
}

async function salvarArrasteContatosPesquisador(input) {
    input.classList.remove('is-error');
    input.classList.add('is-saving');

    try {
        await salvarArrasteContatosPesquisadorValor(input.dataset.pesquisadorId || '', input.value || '0');
    } catch (erro) {
        console.error(erro);
        input.classList.add('is-error');
    } finally {
        input.classList.remove('is-saving');
    }
}

async function salvarArrasteContatosPesquisadorValor(pesquisadorId, valor) {
    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
    const corpo = new URLSearchParams();
    corpo.append('__RequestVerificationToken', token || '');
    corpo.append('casa', obterCasasSelecionadasIndicadores());
    corpo.append('ano', document.getElementById('anoIndicadoresSelect')?.value || anoIndicadores);
    corpo.append('pesquisadorId', pesquisadorId || '');
    corpo.append('valor', valor || '0');

    const resposta = await fetch('/FunilDeVendas/SalvarArrasteContatosPesquisador', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
            'RequestVerificationToken': token || ''
        },
        body: corpo.toString()
    });

    if (!resposta.ok) {
        throw new Error(`Erro ao salvar arraste: ${resposta.status}`);
    }

    const dadosAtualizados = await fetchIndicadoresMensais(obterCasasSelecionadasIndicadores(), document.getElementById('anoIndicadoresSelect')?.value || anoIndicadores);
    renderizarIndicadoresMensais(dadosAtualizados);
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
    renderizarTabelaTaxas(dados);
    renderizarTabelaContatosPesquisador(dados);
}

function navegarParaFiltrosIndicadores() {
    const casaSelecionada = obterCasasSelecionadasIndicadores();
    const anoSelecionado = document.getElementById('anoIndicadoresSelect').value;
    window.location.href = `/FunilDeVendas/Index/${encodeURIComponent(casaSelecionada)}/indicadores/${anoSelecionado}`;
}

function obterCasasSelecionadasIndicadores() {
    const casasCheckboxes = Array.from(document.querySelectorAll('input[name="casasIndicadores"]'));
    if (!casasCheckboxes.length) return casa || 'ISIQV';

    const selecionadas = casasCheckboxes
        .filter(checkbox => checkbox.checked)
        .map(checkbox => checkbox.value);

    if (selecionadas.includes('Todas')) {
        return 'Todas';
    }

    if (!selecionadas.length) {
        return 'ISIQV';
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
            ['grafico-contatos', 'grafico-propostas', 'grafico-convertidas', 'tabela-equipe-indicadores', 'tabela-taxas-indicadores', 'tabela-contatos-pesquisador-indicadores'].forEach(id => {
                const elemento = document.getElementById(id);
                if (elemento) elemento.innerHTML = '<div class="p-4 text-danger">Nao foi possivel carregar os indicadores.</div>';
            });
        });
});
