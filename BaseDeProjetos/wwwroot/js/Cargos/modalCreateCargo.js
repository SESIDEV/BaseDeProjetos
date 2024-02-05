document.addEventListener("DOMContentLoaded", () => {
    let nomeCargo = document.querySelector("#Nome");
    let horas = document.querySelector("#HorasSemanais");
    let salario = document.querySelector("#Salario");

    adicionarListenerVerificacao(nomeCargo, verificarNomeCargo)

    adicionarListenerVerificacao(horas, verificarHoras);

    adicionarListenerVerificacao(salario, verificarSalario)
});