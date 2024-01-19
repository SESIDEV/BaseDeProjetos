function inicializarTabelaPesquisadores(dadosPesquisador) {
    let indicadores = indicadoresContribuicao.concat(indicadoresFinanceiros);
    popularTabelaPesquisadores(indicadores, dadosPesquisador);
}

function popularTabelaPesquisadores(indicadores, dadosPesquisador) {
    let tabelaPesquisadores = document.querySelector("#corpoTabelaPesquisadores");
    tabelaPesquisadores.innerHTML = "";

    let tituloPesquisador = document.querySelector("#tituloPesquisador");
    tituloPesquisador.innerText = `Indicadores do Pesquisador: ${dadosPesquisador["Lider"]["UserName"]}`;

    indicadores.forEach(indicador => {
        let tableRow = document.createElement("tr");
        let tableDataPesquisador = criarTableDataComClasseCell(indicador.nome);
        let valorFormatado = formatarValorPesquisador(indicador.tipoDado, dadosPesquisador[indicador["id"]]);
        let tableDataValor = criarTableDataComClasseCell(valorFormatado);
        let nomeIndicador = `Rank${indicador.id}`;
        let valorRank = dadosPesquisador["RankSobreMedia"][nomeIndicador];
        let tableDataRank;

        if (valorRank) {
            tableDataRank = criarTableDataComClasseCell(valorRank.toFixed(2));
        }
        else {
            tableDataRank = criarTableDataComClasseCell("-");
        }

        tableRow.appendChild(tableDataPesquisador);
        tableRow.appendChild(tableDataValor);
        tableRow.appendChild(tableDataRank);
        tabelaPesquisadores.appendChild(tableRow);
    });
}

function resetTabelaPesquisadores() {
    let corpoTabela = document.querySelector("#corpoTabelaPesquisadores");
    let tabelaContentsReset = criarTabelaLoading(3);
    corpoTabela.innerHTML = "";
    corpoTabela.appendChild(tabelaContentsReset);

    let tituloPesquisador = document.querySelector("#tituloPesquisador");
    tituloPesquisador.innerText = "Indicadores do Pesquisador: ";

    let collapsePesquisador = document.querySelector("#collapsePesquisador");
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

    dadosIndicador.sort((a, b) => b.Valor - a.Valor);

    dadosIndicador.forEach(indicador => {
        if (indicador) {
            corpoTabela.appendChild(criarLinhaPesquisadorIndicador(indicador, tipoDado));
        }
    });

    tituloIndicador.innerText = `Indicador: ${nomeIndicador}`;
}
