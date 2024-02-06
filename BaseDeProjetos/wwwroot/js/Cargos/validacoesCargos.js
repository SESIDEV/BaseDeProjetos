function validarNomeCargo(nomeCargo) {
    return nomeCargo.value && nomeCargo.value.length > 3;
}

function validarHoras(horas) {
    return horas.value && parseInt(horas.value) > 0;
}

function validarSalario(salario) {
    return salario.value && parseFloat(salario.value) > 0;
}
