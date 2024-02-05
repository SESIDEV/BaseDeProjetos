function validarNomeCargo(nomeCargo) {
    return nomeCargo.value && nomeCargo.value.length > 3;
}

function verificarNomeCargo(nomeCargo) {
    let validacaoNomeCargo = validarNomeCargo(nomeCargo);

    verificarInput(validacaoNomeCargo, nomeCargo);
}

function validarHoras(horas) {
    return horas.value && parseInt(horas.value) > 0;
}

function verificarHoras(horas) {
    let validacaoHoras = validarHoras(horas);

    verificarInput(validacaoHoras, horas);
}

function validarSalario(salario) {
    return salario.value && parseFloat(salario.value) > 0;
}

function verificarSalario(salario) {
    let validacaoSalario = validarSalario(salario);

    verificarInput(validacaoSalario, salario);
}