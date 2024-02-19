
document.addEventListener('DOMContentLoaded', () => {
    let listNomeProspeccao = document.querySelectorAll("#NomeProspeccao");
    let listSelectEmpresa = document.querySelectorAll("#EmpresaId");

    adicionarListenerVerificacaoEmLista(listSelectEmpresa, validarSelectEmpresa);
    adicionarListenerVerificacaoEmLista(listNomeProspeccao, validarNomeProspeccao);
});