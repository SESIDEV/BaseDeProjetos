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

function criarLinhaPesquisadorIndicador(indicador) {
    let pesquisador = indicador["Pesquisador"]["Email"];
    let valor = indicador["Valor"];
    let tableRow = document.createElement("tr");
    let tableDataPesquisador = criarTableDataComClasseCell(pesquisador);
    let tableDataValor = criarTableDataComClasseCell(valor);
    // let tableDataRank = criarTableDataComClasseCell(rank);
    tableRow.appendChild(tableDataPesquisador)
    tableRow.appendChild(tableDataValor);
    return tableRow;
}

function popularTabelaIndicadores(dadosIndicador, nomeIndicador) {
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
            corpoTabela.appendChild(criarLinhaPesquisadorIndicador(indicador));
        }
    });
    tituloIndicador.innerText = `Indicador: ${nomeIndicador}`;
}

async function puxarIndicador(identificadorIndicador, nomeIndicador) {
    try {
        let dadosIndicador = await puxarDados(identificadorIndicador);
        popularTabelaIndicadores(dadosIndicador, nomeIndicador);
    } catch (error) {
        console.error(error);
    }
}