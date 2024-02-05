function validarNomeProspeccao(nomeProspeccao) {
    return nomeProspeccao.value && nomeProspeccao.value.length > 3;
}


function validarSelectEmpresa(selectEmpresa) {
    return selectEmpresa.value && selectEmpresa.value !== "-1";
}

function validarDataContatoInicial(dataContatoInicial) {
    if (dataContatoInicial.value) {
        return true;
    }
    else {
        return false;
    }
}

function verificarCampoEmpresa(selectEmpresa) {
    let validacaoEmpresa = validarSelectEmpresa(selectEmpresa);

    verificarInput(validacaoEmpresa, selectEmpresa);
}

function verificarAnotacoesProspeccao(contatoInicialAnotacoes) {
    let validacaoContatoInicialAnotacoes = validarAnotacoesContatoInicial(contatoInicialAnotacoes);

    verificarInput(validacaoContatoInicialAnotacoes, contatoInicialAnotacoes);
}

function verificarCampoNomeProspeccao(nomeProspeccao) {
    let validacaoNomeProspeccao = validarNomeProspeccao(nomeProspeccao);

    verificarInput(validacaoNomeProspeccao, nomeProspeccao);
}

function verificarCampoContatoInicial(dataContatoInicial) {
    let validacaoContatoInicial = validarDataContatoInicial(dataContatoInicial);

    verificarInput(validacaoContatoInicial, dataContatoInicial)
}

function validarAnotacoesContatoInicial(contatoInicialAnotacoes) {
    return contatoInicialAnotacoes.value && contatoInicialAnotacoes.value.length > 3;
}