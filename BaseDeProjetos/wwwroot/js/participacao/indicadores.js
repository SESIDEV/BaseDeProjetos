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
    let valor = indicador["Valor"];
    let tableRow = document.createElement("tr");
    let tableDataPesquisador = criarTableDataComClasseCell(pesquisador);
    let tableDataValor = criarTableDataComClasseCell(valor);
    // let tableDataRank = criarTableDataComClasseCell(rank);
    tableRow.appendChild(tableDataPesquisador)
    tableRow.appendChild(tableDataValor);
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