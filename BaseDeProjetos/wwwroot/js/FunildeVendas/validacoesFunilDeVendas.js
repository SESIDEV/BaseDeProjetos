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

function validarAnotacoesContatoInicial(contatoInicialAnotacoes) {
    return contatoInicialAnotacoes.value && contatoInicialAnotacoes.value.length > 3;
}