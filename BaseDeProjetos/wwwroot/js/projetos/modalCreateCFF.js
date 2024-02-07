document.addEventListener("DOMContentLoaded", () => {
    let datas = document.querySelectorAll("#Data");
    let curvasFinanceiro = document.querySelectorAll("#PercentualFinanceiro");
    let curvasFisico = document.querySelectorAll("#PercentualFisico");

    adicionarListenerVerificacaoEmLista(datas, validarDataCFF);
    adicionarListenerVerificacaoEmLista(curvasFinanceiro, validarPercentualCurva);
    adicionarListenerVerificacaoEmLista(curvasFisico, validarPercentualCurva);
});