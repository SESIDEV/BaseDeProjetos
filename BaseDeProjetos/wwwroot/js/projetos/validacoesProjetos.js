function validarNomeProjeto(nomeProjeto) {
    return nomeProjeto.value && nomeProjeto.value.length > 3;
}

function validarProponente(proponente) {
    return proponente.value && proponente.value != "";
}

function validarLider(lider) {
    return lider.value && lider.value != "";
}

function validarDataEncerramento(dataInicio, dataEncerramento) {
    return new Date(dataEncerramento.value) > new Date(dataInicio.value);
}

function validarDataInicio(dataInicio, dataEncerramento) {
    return new Date(dataInicio.value) < new Date(dataEncerramento.value);
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
function validarNomeRubrica(nome) {
    return nome.value && nome.value.length > 3;
}
function validarValorRubrica(valor) {
    return valor.value && parseFloat(valor.value) > 0;
}