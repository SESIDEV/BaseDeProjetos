document.addEventListener("DOMContentLoaded", () => {
    let nomeProjeto = document.querySelector("#NomeProjeto");
    let dataEncerramento = document.querySelector("#DataEncerramento");
    let dataInicio = document.querySelector("#DataInicio");
    let duracaoProjeto = document.querySelector("#DuracaoProjetoEmMeses");
    let custoHM = document.querySelector("#CustoHM");
    let valorProjeto = document.querySelector("#ValorTotalProjeto");
    let selectEmpresa = document.querySelector("#Empresa_Id");
    let proponente = document.querySelector("#ProponenteId");

    adicionarListenerVerificacao(nomeProjeto, validarNomeProjeto);
    adicionarListenerVerificacao(duracaoProjeto, validarDuracaoMeses);
    adicionarListenerVerificacao(custoHM, validarCustoHM);
    adicionarListenerVerificacao(valorProjeto, validarValorProjeto);
    adicionarListenerVerificacao(selectEmpresa, validarSelectEmpresaProjetos);
    adicionarListenerVerificacao(proponente, validarProponente);

    // Check complexo, não podemos usar atalhos
    VerificarDatasProjeto(dataEncerramento, dataInicio);
});

function VerificarDatasProjeto(dataEncerramento, dataInicio) {
    if (dataEncerramento && dataInicio) {
        dataEncerramento.addEventListener("change", () => {
            verificarInput(validarDataEncerramento(dataInicio, dataEncerramento), dataEncerramento);
            verificarInput(validarDataInicio(dataInicio, dataEncerramento), dataInicio);
        });
        dataEncerramento.addEventListener("click", () => {
            verificarInput(validarDataEncerramento(dataInicio, dataEncerramento), dataEncerramento);
            verificarInput(validarDataInicio(dataInicio, dataEncerramento), dataInicio);
        });
    }
}
