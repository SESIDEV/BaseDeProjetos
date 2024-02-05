function validarNomeProspeccao() {
    let nomeProspeccao = document.getElementById("NomeProspeccao");
    return nomeProspeccao.value && nomeProspeccao.value.length > 3;
}


function validarSelectEmpresa() {
    let selectEmpresa = document.getElementById("EmpresaId");
    return selectEmpresa.value && selectEmpresa.value !== "-1";
}

function validarDataContatoInicial() {
    let dataContatoInicial = document.getElementById("Status_0__Data");
    if (dataContatoInicial.value) {
        return true;
    }
    else {
        return false;
    }
}

function verificarCampoEmpresa() {
    let selectEmpresa = document.getElementById("EmpresaId");
    let validacaoEmpresa = validarSelectEmpresa();

    if (validacaoEmpresa === true) {
        selectEmpresa.classList.remove("is-invalid");
        selectEmpresa.classList.add("is-valid");
    }
    else {
        selectEmpresa.classList.remove("is-valid");
        selectEmpresa.classList.add("is-invalid");
    }
}

function verificarAnotacoesProspeccao() {
    let contatoInicialAnotacoes = document.getElementById("Status_0__Anotacoes");
    let validacaoContatoInicialAnotacoes = validarAnotacoesContatoInicial();

    if (validacaoContatoInicialAnotacoes === true) {
        contatoInicialAnotacoes.classList.remove("is-invalid");
        contatoInicialAnotacoes.classList.add("is-valid");
    }
    else {
        contatoInicialAnotacoes.classList.add("is-invalid");
        contatoInicialAnotacoes.classList.remove("is-valid");
    }
}

function verificarCampoNomeProspeccao() {
    let nomeProspeccao = document.getElementById("NomeProspeccao");
    let validacaoNomeProspeccao = validarNomeProspeccao();

    if (validacaoNomeProspeccao === true) {
        nomeProspeccao.classList.remove("is-invalid");
        nomeProspeccao.classList.add("is-valid");
    }
    else {
        nomeProspeccao.classList.remove("is-valid");
        nomeProspeccao.classList.add("is-invalid");
    }
}

function verificarCampoContatoInicial() {
    let dataContatoInicial = document.getElementById("Status_0__Data");
    let validacaoContatoInicial = validarDataContatoInicial();

    if (validacaoContatoInicial === true) {
        dataContatoInicial.classList.add("is-valid");
        dataContatoInicial.classList.remove("is-invalid");
    }
    else {
        dataContatoInicial.classList.remove("is-valid");
        dataContatoInicial.classList.add("is-invalid");
    }
}

function validarAnotacoesContatoInicial() {
    let contatoInicialAnotacoes = document.getElementById("Status_0__Anotacoes");
    return contatoInicialAnotacoes.value && contatoInicialAnotacoes.value > 3;
}

document.addEventListener('DOMContentLoaded', () => {
    console.log("Adding event listeners");

    let nomeProspeccao = document.getElementById("NomeProspeccao");
    let selectEmpresa = document.getElementById("EmpresaId");
    let dataContatoInicial = document.getElementById("Status_0__Data");
    let contatoInicialAnotacoes = document.getElementById("Status_0__Anotacoes");

    //const concluirCreateProsp = document.getElementById("concluirCreateProsp");

    /*    concluirCreateProsp.addEventListener("click", (event) => validarCampos(event));*/

    if (selectEmpresa) {
        selectEmpresa.addEventListener("change", verificarCampoEmpresa);
    }
    else {
        console.error(`No select emepresa: ${selectEmpresa}`);
    }

    if (nomeProspeccao) {
        nomeProspeccao.addEventListener("change", verificarCampoNomeProspeccao);
    }
    else {
        console.error(`No nomeProspeccao: ${nomeProspeccao}`);
    }

    if (dataContatoInicial) {
        dataContatoInicial.addEventListener("change", verificarCampoContatoInicial);
    }
    else {
        console.error(`No dataContatoInicial: ${dataContatoInicial}`);
    }

    if (contatoInicialAnotacoes) {
        contatoInicialAnotacoes.addEventListener("change", validarAnotacoesContatoInicial);
    }
    else {
        console.error(`No contatoInicialAnotacoes: ${contatoInicialAnotacoes}`);
    }
});