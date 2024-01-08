async function puxarDados(nomeIndicador) {
    try {
        const response = await fetch(`/Participacao/RetornarDadosIndicador/?nomeIndicador=${nomeIndicador}`);
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
    tableData.innerText = conteudo;
    return tableData
}

function criarLinhaPesquisadorIndicador(indicador, tipoDado) {
    let pesquisador = indicador["Pesquisador"]["Email"];
    let valor = formatarValor(tipoDado, indicador);
    let conteudosDetalhe = "<a href='#'>Teste</a>"

    let tableRow = document.createElement("tr");
    let tableDataPesquisador = criarTableDataComClasseCell(pesquisador);
    let tableDataValor = criarTableDataComClasseCell(valor);
    let tableDataRank = criarTableDataComClasseCell("PLACEHOLDER");
    let tableDataDetalhes = criarTableDataComClasseCell(conteudosDetalhe);
    tableRow.appendChild(tableDataPesquisador)
    tableRow.appendChild(tableDataValor);
    tableRow.appendChild(tableDataRank);
    tableRow.appendChild(tableDataDetalhes);
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
        console.log(indicador["Valor"]);
        return (indicador["Valor"] * 100).toFixed(2) + "%";
    } else {
        return '';
    }
}

function popularTabelaIndicadores(dadosIndicador, nomeIndicador, tipoDado) {
    let corpoTabela = document.querySelector("#corpoTabela");
    let tituloIndicador = document.querySelector("#tituloIndicador");

    if (!corpoTabela) {
        throw new Error("A tabela de indicadores não existe");
    }

    if (!dadosIndicador) {
        throw new Error("Não temos dados para exibir na tabela de indicadores");
    }

    if (!nomeIndicador) {
        throw new Error("O indicador não está nomeado");
    }

    corpoTabela.innerHTML = "";
    tituloIndicador.innerText = "Carregando indicador..."; 
    dadosIndicador.forEach(indicador => {
        if (indicador) {
            corpoTabela.appendChild(criarLinhaPesquisadorIndicador(indicador, tipoDado));
        }
    });
    tituloIndicador.innerText = `Indicador: ${nomeIndicador}`;
}

async function puxarIndicador(identificadorIndicador, nomeIndicador, tipoDado) {
    try {
        let dadosIndicador = await puxarDados(identificadorIndicador);
        popularTabelaIndicadores(dadosIndicador, nomeIndicador, tipoDado);
    } catch (error) {
        console.error(error);
    }
}

function resetTabela(event) {
    let tituloIndicador = document.querySelector("#tituloIndicador");
    let corpoTabela = document.querySelector("#corpoTabela");
    let collapseIndicador = document.querySelector("#collapseIndicador");
    tituloIndicador.innerText = "Carregando indicador...";

    let tabelaContentsReset = "<tr>" +
        "<td><div class='mt-2 mb-2 spinner-border text-primary'></div></td>" +
        "<td><div class='mt-2 mb-2 spinner-border text-primary'></div></td>" +
        "<td><div class='mt-2 mb-2 spinner-border text-primary'></div></td>" +
        "<td><div class='mt-2 mb-2 spinner-border text-primary'></div></td>" +
        "</tr>";

    corpoTabela.innerHTML = tabelaContentsReset;
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
        a.setAttribute("onclick", `puxarIndicador("${indicador.id}", "${indicador.nome}", "${indicador.tipoDado}")`);
        a.textContent = indicador.nome;
        a.classList.add("dropdown-item");
        listItem.appendChild(a);
        menuDropdown.appendChild(listItem);
    });
}

function inicializarPaginaIndicadores() {
    // TipoDado: F = Financeiro, f = float, flutuante, i = inteiro (case-sensitive)
    const indicadoresFinanceiros = [
        { id: "ValorTotalProspeccoes", nome: "Valor Total das Prospecções", tipoDado: 'F' },
        { id: "ValorMedioProspeccoes", nome: "Valor Médio das Prospecções", tipoDado: 'F' },
        { id: "ValorTotalProspeccoesComProposta", nome: "Valor Total das Prospecções em Proposta", tipoDado: 'F' },
        { id: "ValorMedioProspeccoesComProposta", nome: "Valor Médio das Prospecções em Proposta", tipoDado: 'F' },
        { id: "ValorTotalProspeccoesConvertidas", nome: "Valor Total das Prospecções Convertidas", tipoDado: 'F' },
        { id: "ValorMedioProspeccoesConvertidas", nome: "Valor Médio das Prospecções Convertidas", tipoDado: 'F' },
    ];

    // TipoDado: F = Financeiro, f = float, flutuante, i = inteiro (case-sensitive)
    const indicadoresContribuicao = [
        { id: "QuantidadeProspeccoes", nome: "Quantidade de Prospecções", tipoDado: 'i' },
        { id: "QuantidadeProspeccoesLider", nome: "Quantidade de Prospecções (Líder)", tipoDado: 'i' },
        { id: "QuantidadeProspeccoesMembro", nome: "Quantidade de Prospecções (Membro)", tipoDado: 'i' },
        { id: "TaxaConversaoProposta", nome: "Taxa de Conversão em Proposta", tipoDado: 'p' },
        { id: "TaxaConversaoProjeto", nome: "Taxa de Conversão em Projetos", tipoDado: 'p' },
        { id: "QuantidadeProjetos", nome: "Quantidade de Projetos", tipoDado: 'i' },
        { id: "QuantidadeProspeccoesComProposta", nome: "Quantidade de Prospecções em Proposta", tipoDado: 'i' },
        { id: "QuantidadeProspeccoesProjeto", nome: "Quantidade de Prospecções Convertidas", tipoDado: 'i' },
        { id: "AssertividadePrecificacao", nome: "Assertividade na Precificação", tipoDado: 'f' },
        { id: "FatorContribuicaoFinanceira", nome: "Fator de Contribuição Financeira", tipoDado: 'f' },
        { id: "MediaFatores", nome: "Média dos Fatores", tipoDado: 'f' },
    ];

    popularDropdown('ulFinanceira', indicadoresFinanceiros);
    popularDropdown('ulContribuicao', indicadoresContribuicao);
}