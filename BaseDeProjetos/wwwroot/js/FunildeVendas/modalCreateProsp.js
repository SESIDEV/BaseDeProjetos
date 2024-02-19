

document.addEventListener('DOMContentLoaded', () => {
    let nomeProspeccao = document.getElementById("NomeProspeccao");
    let selectEmpresa = document.getElementById("EmpresaId");
    let dataContatoInicial = document.getElementById("Status_0__Data");
    let contatoInicialAnotacoes = document.getElementById("Status_0__Anotacoes");

    adicionarListenerVerificacao(selectEmpresa, validarSelectEmpresa)
    adicionarListenerVerificacao(nomeProspeccao, validarNomeProspeccao);
    adicionarListenerVerificacao(dataContatoInicial, validarDataContatoInicial);
    adicionarListenerVerificacao(contatoInicialAnotacoes, validarAnotacoesContatoInicial);
});