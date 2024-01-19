// TipoDado: F = Financeiro, f = float, flutuante, i = inteiro (case-sensitive)

const cultura = new Intl.NumberFormat('pt-BR');

const indicadoresFinanceiros = [
    { id: "ValorTotalProspeccoes", nome: "Valor Total das Prospecções", tipoDado: 'F' },
    { id: "ValorMedioProspeccoes", nome: "Valor Médio das Prospecções", tipoDado: 'F' },
    { id: "ValorTotalProspeccoesComProposta", nome: "Valor Total das Prospecções em Proposta", tipoDado: 'F' },
    { id: "ValorMedioProspeccoesComProposta", nome: "Valor Médio das Prospecções em Proposta", tipoDado: 'F' },
    { id: "ValorTotalProspeccoesConvertidas", nome: "Valor Total das Prospecções Convertidas", tipoDado: 'F' },
    { id: "ValorMedioProspeccoesConvertidas", nome: "Valor Médio das Prospecções Convertidas", tipoDado: 'F' },
];

const indicadoresContribuicao = [
    { id: "QuantidadeProspeccoes", nome: "Quantidade de Prospecções", tipoDado: 'i' },
    { id: "QuantidadeProspeccoesLider", nome: "Quantidade de Prospecções (Líder)", tipoDado: 'i' },
    { id: "QuantidadeProspeccoesMembro", nome: "Quantidade de Prospecções (Membro)", tipoDado: 'i' },
    { id: "TaxaConversaoProposta", nome: "Taxa de Conversão em Proposta", tipoDado: 'p' },
    { id: "TaxaConversaoProjeto", nome: "Taxa de Conversão em Projetos", tipoDado: 'p' },
    { id: "QuantidadeProjetos", nome: "Quantidade de Projetos", tipoDado: 'i' },
    { id: "QuantidadeProspeccoesComProposta", nome: "Quantidade de Prospecções em Proposta", tipoDado: 'i' },
    { id: "QuantidadeProspeccoesConvertidas", nome: "Quantidade de Prospecções Convertidas", tipoDado: 'i' },
    { id: "AssertividadePrecificacao", nome: "Assertividade na Precificação", tipoDado: 'f' },
    { id: "FatorContribuicaoFinanceira", nome: "Fator de Contribuição Financeira", tipoDado: 'f' },
    { id: "MediaFatores", nome: "Média dos Fatores", tipoDado: 'f' },
];
async function puxarDados(nomeIndicador) {
    let url = `/Participacao/RetornarDadosIndicador/?nomeIndicador=${nomeIndicador}`;

    if (dataInicio) {
        url += `&dataInicio=${dataInicio}`;
    }

    if (dataFim) {
        url += `&dataFim=${dataFim}`
    }

    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error(error);
    }
}

function criarTableDataComClasseCell(conteudo) {
    let tableData = document.createElement("td");
    tableData.classList.add("cell");
    tableData.innerHTML = conteudo;
    return tableData
}

function criarLinhaPesquisadorIndicador(indicador, tipoDado) {
    let pesquisador = indicador["Pesquisador"]["UserName"];
    let valor = formatarValor(tipoDado, indicador);
    let rank = indicador["Rank"].toFixed(2);
    let conteudosDetalhe = `<a href='#' onclick='puxarPesquisador("${indicador['Pesquisador']['Id']}", false)'>Ver mais</a>`
    let conteudosProspeccoes = `<a href='#' data-bs-target="#modalProspeccoes" data-bs-toggle="modal" onclick='puxarPesquisador("${indicador['Pesquisador']['Id']}", true)'>Ver prospecções</a>`

    let tableRow = document.createElement("tr");
    let tableDataPesquisador = criarTableDataComClasseCell(pesquisador);
    let tableDataValor = criarTableDataComClasseCell(valor);
    let tableDataRank = criarTableDataComClasseCell(rank);
    let tableDataDetalhes = criarTableDataComClasseCell(conteudosDetalhe);
    let tableDataProspeccoes = criarTableDataComClasseCell(conteudosProspeccoes);

    tableRow.appendChild(tableDataPesquisador)
    tableRow.appendChild(tableDataValor);
    tableRow.appendChild(tableDataRank);
    tableRow.appendChild(tableDataDetalhes);
    tableRow.appendChild(tableDataProspeccoes);
    return tableRow;
}

function formatarValor(tipoDado, indicador) {
    const formatadorFinanceiro = new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL',
    });

    if (tipoDado == 'i') {
        return indicador["Valor"];
    } else if (tipoDado == 'F') {
        return formatadorFinanceiro.format(indicador["Valor"]);
    } else if (tipoDado == 'f') {
        return indicador["Valor"].toFixed(2);
    } else if (tipoDado == 'p') {
        return (indicador["Valor"] * 100).toFixed(2) + "%";
    } else {
        return '';
    }
}

function formatarValorPesquisador(tipoDado, valor) {
    const formatadorFinanceiro = new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL',
    });

    if (tipoDado == 'i') {
        return valor;
    } else if (tipoDado == 'F') {
        return formatadorFinanceiro.format(valor);
    } else if (tipoDado == 'f') {
        return valor.toFixed(2);
    } else if (tipoDado == 'p') {
        return (valor * 100).toFixed(2) + "%";
    } else {
        return '';
    }
}

async function puxarIndicador(identificadorIndicador, nomeIndicador, tipoDado) {
    try {
        let dadosIndicador = await puxarDados(identificadorIndicador);
        popularTabelaIndicadores(dadosIndicador, nomeIndicador, tipoDado);
    } catch (error) {
        console.error(error);
    }
}

function criarTabelaLoading(quantidadeColunas) {
    let tableRowReset = document.createElement("tr");
    let classesDiv = ["mt-2", "mb-2", "spinner-border", "text-primary"];

    for (let i = 1; i <= quantidadeColunas; i++) {
        let tableDataReset = document.createElement("td");
        let divDataReset = document.createElement("div");

        divDataReset.classList.add(...classesDiv);
        tableDataReset.appendChild(divDataReset);

        tableRowReset.appendChild(tableDataReset);
    }

    return tableRowReset;
}

function resetTabela() {
    let tituloIndicador = document.querySelector("#tituloIndicador");
    let corpoTabela = document.querySelector("#corpoTabela");
    let collapseIndicador = document.querySelector("#collapseIndicador");
    tituloIndicador.innerText = "Carregando indicador...";

    let tableRowReset = criarTabelaLoading(5);

    corpoTabela.appendChild(tableRowReset);
    collapseIndicador.classList.remove("show");
}


function popularDropdown(idDropdown, indicadores) {
    const menuDropdown = document.querySelector(`#${idDropdown}`);
    indicadores.forEach(indicador => {
        const listItem = document.createElement("li");
        const a = document.createElement("a");
        a.setAttribute("data-bs-target", "#collapseIndicador");
        a.setAttribute("data-bs-toggle", "collapse");
        a.setAttribute("href", "#");
        a.setAttribute("onclick", `puxarIndicador("${indicador.id}", "${indicador.nome}", "${indicador.tipoDado}", "${dataInicio}", "${dataFim}")`);
        a.textContent = indicador.nome;
        a.classList.add("dropdown-item");
        listItem.appendChild(a);
        menuDropdown.appendChild(listItem);
    });
}

async function puxarPesquisador(idPesquisador, prospeccao) {
    let collapsePesquisador = document.querySelector("#collapsePesquisador");
    let loadingProspeccoes = document.querySelector("#loadingProspeccoes");
    loadingProspeccoes.style.display = "block";

    if (prospeccao === false) {
        if (!collapsePesquisador.classList.contains("show")) {
            collapsePesquisador.classList.add("show");
        }
        else {
            resetTabelaPesquisadores();
        }
    }

    try {
        let corpoModal = document.querySelector("#corpoProspeccoes");
        corpoModal.innerHTML = '';
        let dadosPesquisador = await puxarDadosPesquisador(idPesquisador);
        if (prospeccao === false) {
            inicializarTabelaPesquisadores(dadosPesquisador);
            collapsePesquisador.classList.add("show");
        }
        else {
            inicializarModalProspeccoes(dadosPesquisador["Participacoes"], idPesquisador);
        }
    } catch (error) {
        console.error(error);
    }
}

function criarButtonGrafico(id) {
    let buttonGrafico = document.createElement("button");
    buttonGrafico.setAttribute("type", "button");
    buttonGrafico.id = `button-${id}`
    buttonGrafico.classList.add("btn");
    buttonGrafico.classList.add("app-btn-primary");
    buttonGrafico.addEventListener("click", () => exibirDados(id));

    let internalDiv = document.createElement("div");
    internalDiv.id = `internalDiv-${id}`;
    internalDiv.innerText = "Exibir Gráfico";
    buttonGrafico.appendChild(internalDiv);

    return buttonGrafico;
}

function criarFigure() {
    let figure = document.createElement("figure");
    figure.classList.add("highcharts-figure");
    let participacao = document.createElement("div");
    participacao.id = "participacao"
    figure.appendChild(participacao);
    return figure;
}

function inicializarModalProspeccoes(dadosProspeccoes, id) {
    let corpoModal = document.querySelector("#corpoProspeccoes");
    let loadingProspeccoes = document.querySelector("#loadingProspeccoes");

    let buttonGrafico = criarButtonGrafico(id);
    let hr = document.createElement("hr");
    corpoModal.parentElement.appendChild(hr);
    corpoModal.parentElement.appendChild(buttonGrafico);

    let figure = criarFigure();
    corpoModal.parentElement.appendChild(figure);

    dadosProspeccoes.forEach(dado => {
        let cardConteudo = {
            Convertida: dado.Convertida,
            Planejada: dado.Planejada,
            Suspensa: dado.Suspensa,
            NaoConvertida: dado.NaoConvertida,
            EmDiscussao: dado.EmDiscussao,
            ComProposta: dado.ComProposta,
            Ribbon: function () {
                if (this.Convertida) {
                    return '<div class="ribbon ribbon-top-right ribbon-green-yellow"><span>Convertida</span></div>';
                } else if (this.Planejada) {
                    return '<div class="ribbon ribbon-top-right ribbon-blue"><span>Planejada</span></div>';
                } else if (this.Suspensa) {
                    return '<div class="ribbon ribbon-top-right ribbon-yellow"><span>Suspensa</span></div>';
                } else if (this.NaoConvertida) {
                    return '<div class="ribbon ribbon-top-right ribbon-red"><span>Não Convertida</span></div>';
                } else if (this.EmDiscussao) {
                    return '<div class="ribbon ribbon-top-right ribbon-orange"><span>Em Discussão</span></div>';
                } else if (this.ComProposta) {
                    return '<div class="ribbon ribbon-top-right ribbon-green"><span>Com Proposta</span></div>';
                } else {
                    return '<div class="ribbon ribbon-top-right ribbon-red"><span>Desconhecido</span></div>';
                }
            },
            Render: function () {
                return `
                <div class="app-card app-card-doc h-100 shadow-sm">
                    ${this.Ribbon()}
                    <div class="app-card-body p-3">
                        <h4 class="app-doc-title truncate mb-0">
                            ${dado.NomeProjeto}
                        </h4>
                        <div class="app-doc-meta">
                            <ul class="list-unstyled mb-0">
                                <li>
                                    <span style="display:block; font-weight:bold">
                                        Empresa:
                                    </span>${dado.EmpresaProjeto}
                                </li>
                                <li>
                                    <span style="display:block; font-weight:bold">
                                        Valor:
                                    </span>${(dado.ValorNominal != 0 ? "R$" + cultura.format(dado.ValorNominal) : "Sem valor especificado")}
                                </li>
                                ${dado.ValorNominal > 0 ? `
                                    <li>
                                        <span style="display:block; font-weight:bold">
                                            Valor do Líder:
                                        </span>${dado.ValorLider != 0 ? "R$" + cultura.format(dado.ValorLider) : "Sem valor pro líder"}
                                    </li>
                                    <li>
                                        <span style="display:block; font-weight:bold">
                                            Valor dos Pesquisadores (${dado.QuantidadePesquisadores}):
                                        </span>${dado.ValorPesquisadores != 0 ? "R$" + cultura.format(dado.ValorPesquisadores) : "Sem valor para os pesquisadores"} /
                                        ${dado.ValorPorPesquisador != 0 ? "R$" + cultura.format(dado.ValorPorPesquisador) : "Sem valor por pesquisador"}
                                    </li>
                                    <li>
                                        <span style="display:block; font-weight:bold">
                                            Valor dos Bolsistas (${dado.QuantidadeBolsistas}):
                                        </span>${dado.ValorBolsistas != 0 ? "R$" + cultura.format(dado.ValorBolsistas) : "Sem valor para os bolsistas"} /
                                        ${dado.ValorPorBolsista != 0 ? "R$" + cultura.format(dado.ValorPorBolsista) : "Sem valor por bolsista"}
                                    </li>
                                    <li>
                                        <span style="display:block; font-weight:bold">
                                            Valor dos Estagiários (${dado.QuantidadeEstagiarios}):
                                        </span>${dado.ValorEstagiarios != 0 ? "R$" + cultura.format(dado.ValorEstagiarios) : "Sem valor para os estagiários"} /
                                        ${dado.ValorPorEstagiario != 0 ? "R$" + cultura.format(dado.ValorPorEstagiario) : "Sem valor por estagiário"}
                                    </li>` : ''}
                            </ul>
                        </div>
                    </div>
                </div>`;
            }
        }

        let containerConteudoModal = document.createElement("div");
        containerConteudoModal.innerHTML = cardConteudo.Render();

        while (containerConteudoModal.firstChild) {
            corpoModal.appendChild(containerConteudoModal.firstChild);
        }

        loadingProspeccoes.style.display = "none";
    });
}

async function puxarDadosPesquisador(id) {
    let url = `/Participacao/RetornarDadosPesquisador/${id}`

    if (dataInicio && dataFim) {
        url += `?dataInicio=${dataInicio}&dataFim=${dataFim}`
    }

    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error(error);
    }
}


function inicializarPaginaIndicadores() {
    popularDropdown('ulFinanceira', indicadoresFinanceiros);
    popularDropdown('ulContribuicao', indicadoresContribuicao);
}

document.addEventListener('DOMContentLoaded', inicializarPaginaIndicadores());