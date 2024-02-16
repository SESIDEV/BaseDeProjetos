document.addEventListener("DOMContentLoaded", () => {
    let nomeRubrica = document.querySelectorAll("#Nome");
    let valorRubrica = document.querySelectorAll("#Valor");

    adicionarListenerVerificacaoEmLista(nomeRubrica, validarNomeRubrica);
    adicionarListenerVerificacaoEmLista(valorRubrica, validarValorRubrica);
});