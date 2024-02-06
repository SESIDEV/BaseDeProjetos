function validarNomeProjeto(nomeProjeto) {
    return nomeProjeto.value && nomeProjeto.value.length > 3;
}

function validarProponente(proponente) {
    return proponente.value && proponente.value != "";
}

// TODO: Parse date?
function validarDataFim(dataInicio, dataFim) {
    return dataFim.value > dataInicio.value;
}

function validarDuracaoMeses(duracaoMeses) {
    return parseInt(duracaoMeses.value) > 0;
}

function validarCustoHM(custoHM) {
    return parseFloat(custoHM.value) > 0;
}

function validarValorProjeto(valorProjeto) {
    return parseFloat(valorProjeto.value) > 0;
}

function validarSelectEmpresaProjetos(selectEmpresa) {
    return selectEmpresa.value && selectEmpresa.value != "-1" && selectEmpresa.value != "";
}