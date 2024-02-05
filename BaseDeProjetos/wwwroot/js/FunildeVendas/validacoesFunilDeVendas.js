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

    if (validacaoEmpresa === true) {
        selectEmpresa.classList.remove("is-invalid");
        selectEmpresa.classList.add("is-valid");
    }
    else {
        selectEmpresa.classList.remove("is-valid");
        selectEmpresa.classList.add("is-invalid");
    }
}

function verificarAnotacoesProspeccao(contatoInicialAnotacoes) {
    let validacaoContatoInicialAnotacoes = validarAnotacoesContatoInicial(contatoInicialAnotacoes);

    if (validacaoContatoInicialAnotacoes === true) {
        contatoInicialAnotacoes.classList.remove("is-invalid");
        contatoInicialAnotacoes.classList.add("is-valid");
    }
    else {
        contatoInicialAnotacoes.classList.add("is-invalid");
        contatoInicialAnotacoes.classList.remove("is-valid");
    }
}

function verificarCampoNomeProspeccao(nomeProspeccao) {
    let validacaoNomeProspeccao = validarNomeProspeccao(nomeProspeccao);

    if (validacaoNomeProspeccao === true) {
        nomeProspeccao.classList.remove("is-invalid");
        nomeProspeccao.classList.add("is-valid");
    }
    else {
        nomeProspeccao.classList.remove("is-valid");
        nomeProspeccao.classList.add("is-invalid");
    }
}

function verificarCampoContatoInicial(dataContatoInicial) {
    let validacaoContatoInicial = validarDataContatoInicial(dataContatoInicial);

    if (validacaoContatoInicial === true) {
        dataContatoInicial.classList.add("is-valid");
        dataContatoInicial.classList.remove("is-invalid");
    }
    else {
        dataContatoInicial.classList.remove("is-valid");
        dataContatoInicial.classList.add("is-invalid");
    }
}

function validarAnotacoesContatoInicial(contatoInicialAnotacoes) {
    return contatoInicialAnotacoes.value && contatoInicialAnotacoes.value.length > 3;
}