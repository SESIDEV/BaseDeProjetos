function inicializarTabelaPesquisadores(dadosPesquisador) {
    let indicadores = indicadoresContribuicao.concat(indicadoresFinanceiros);
    popularTabelaPesquisadores(indicadores, dadosPesquisador);
}

function popularTabelaPesquisadores(indicadores, dadosPesquisador) {
    if (!indicadores) {
        throw new Error("Não foi possível encontrar os indicadores para exibição");
    }

    if (!dadosPesquisador) {
        throw new Error("Não foi possível encontrar os dados do pesquisador");
    }

    let tabelaPesquisadores = document.querySelector("#corpoTabelaPesquisadores");
    let tituloPesquisador = document.querySelector("#tituloPesquisador");

    if (!tabelaPesquisadores) {
        throw new Error("Não foi possível encontrar o corpo da tabela de pesquisadores");
    }

    if (!tituloPesquisador) {
        throw new Error("Não foi possível encontrar o campo para o título do pesquisador");
    }

    tabelaPesquisadores.innerHTML = "";
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
    if (!corpoTabela) {
        throw new Error("Elemento #corpoTabelaPesquisadores não encontrado.");
    }

    let tabelaContentsReset = criarTabelaLoading(3);
    if (!tabelaContentsReset) {
        throw new Error("Falha ao criar conteúdo da tabela de carregamento.");
    }

    corpoTabela.innerHTML = "";
    corpoTabela.appendChild(tabelaContentsReset);

    let tituloPesquisador = document.querySelector("#tituloPesquisador");
    if (!tituloPesquisador) {
        throw new Error("Elemento #tituloPesquisador não encontrado.");
    }

    tituloPesquisador.innerText = "Indicadores do Pesquisador: ";
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
