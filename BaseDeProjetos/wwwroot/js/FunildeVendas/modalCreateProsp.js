

document.addEventListener('DOMContentLoaded', () => {
    console.log("Adding event listeners");

    let nomeProspeccao = document.getElementById("NomeProspeccao");
    let selectEmpresa = document.getElementById("EmpresaId");
    let dataContatoInicial = document.getElementById("Status_0__Data");
    let contatoInicialAnotacoes = document.getElementById("Status_0__Anotacoes");

    if (selectEmpresa) {
        selectEmpresa.addEventListener("change", () => verificarCampoEmpresa(selectEmpresa));
    }
    else {
        console.error(`No select emepresa: ${selectEmpresa}`);
    }

    if (nomeProspeccao) {
        nomeProspeccao.addEventListener("change", () => verificarCampoNomeProspeccao(nomeProspeccao));
    }
    else {
        console.error(`No nomeProspeccao: ${nomeProspeccao}`);
    }

    if (dataContatoInicial) {
        dataContatoInicial.addEventListener("change", () => verificarCampoContatoInicial(dataContatoInicial));
    }
    else {
        console.error(`No dataContatoInicial: ${dataContatoInicial}`);
    }

    if (contatoInicialAnotacoes) {
        contatoInicialAnotacoes.addEventListener("change", () => verificarAnotacoesProspeccao(contatoInicialAnotacoes));
    }
    else {
        console.error(`No contatoInicialAnotacoes: ${contatoInicialAnotacoes}`);
    }
});