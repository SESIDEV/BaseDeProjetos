
document.addEventListener('DOMContentLoaded', () => {
    let listNomeProspeccao = document.querySelectorAll("#NomeProspeccao");
    let listSelectEmpresa = document.querySelectorAll("#EmpresaId");

    listSelectEmpresa.forEach((selectEmpresa) => {
        if (selectEmpresa) {
            selectEmpresa.addEventListener("change", () => verificarCampoEmpresa(selectEmpresa));
        }
        else {
            console.error(`No select empresa: ${selectEmpresa}`);
        }
    });

    listNomeProspeccao.forEach((nomeProspeccao) => {
        if (nomeProspeccao) {
            nomeProspeccao.addEventListener("change", () => verificarCampoNomeProspeccao(nomeProspeccao));
        }
        else {
            console.error(`No nomeProspeccao: ${nomeProspeccao}`);
        }
    });
});