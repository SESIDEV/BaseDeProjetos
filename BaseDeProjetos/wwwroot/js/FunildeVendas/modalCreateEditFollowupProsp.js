document.addEventListener('DOMContentLoaded', () => {
    let anotacoes = document.querySelectorAll("#Anotacoes");

    adicionarListenerVerificacaoEmLista(anotacoes, validarAnotacoesContatoInicial)
});