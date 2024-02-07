document.addEventListener("DOMContentLoaded", () => {
    let datas = document.querySelectorAll("#Data");

    adicionarListenerVerificacaoEmLista(datas, validarDataCFF);
});