var prospeccoesDetalhes = [];

function changePageSize() {
    var pageSize = document.getElementById('tamanhoPagina').value;
    var url = new URL(window.location.href);
    url.searchParams.set('tamanhoPagina', pageSize);
    window.location.href = url.toString();
}

const formatoData = {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
    hour12: false
};

async function trocarModalNovaProsp() {
    const toggleButton = document.getElementById("botaoToggleProspFollowUp");
    toggleButton.disabled = true;

    const valorSelect = document.getElementById("selectCreateProsp").value;
    window.tipoAssociacaoPreselecionada = "nova";

    if (valorSelect !== "-1" && valorSelect !== "-2") {
        toggleButton.dataset.bsTarget = "#CreateFollowupProspModal-" + valorSelect;

        try {
            const response = await fetch("/FunildeVendas/RetornarModal?idProsp=" + encodeURIComponent(valorSelect) + "&tipo=CreateFollowup");
            const result = await response.text();
            document.querySelector("#modalCreateFollowUpProspContainer").innerHTML = result;
        } catch (error) {
            console.error("Error fetching modal content:", error);
        } finally {
            toggleButton.disabled = false;
        }
    } else {
        toggleButton.dataset.bsTarget = "#criarProspModalToggle";
        toggleButton.disabled = false;

        if (window.atualizarModoCadastroEmpresaProsp) {
            window.atualizarModoCadastroEmpresaProsp();
        }

        if (window.sincronizarAssociacaoProspecaoCreate) {
            window.sincronizarAssociacaoProspecaoCreate();
        }

        if (valorSelect === "-2") {
            const nomeProspeccao = document.getElementById("NomeProspeccao");
            const statusData = document.getElementById("Status_0__Data");
            const statusAnotacoes = document.getElementById("Status_0__Anotacoes");
            const valorStatus = document.getElementById("Status_0__Status");

            let data = new Intl.DateTimeFormat('pt-BR', formatoData).format(new Date());

            if (statusData) {
                statusData.value = data;
                statusData.placeholder = data;
            }

            if (statusAnotacoes) {
                statusAnotacoes.value = "Inclu\u00eddo no plano de prospec\u00e7\u00f5es como prospec\u00e7\u00e3o planejada";
            }

            if (valorStatus) {
                valorStatus.value = "Planejada";
            }

            if (nomeProspeccao) {
                nomeProspeccao.value = "Prospec\u00e7\u00e3o planejada";
                nomeProspeccao.placeholder = "Prospec\u00e7\u00e3o planejada";
            }
        } else {
            const nomeProspeccao = document.getElementById("NomeProspeccao");
            const statusAnotacoes = document.getElementById("Status_0__Anotacoes");
            const valorStatus = document.getElementById("Status_0__Status");

            if (valorStatus) {
                valorStatus.value = "ContatoInicial";
            }

            if (statusAnotacoes && statusAnotacoes.value === "Inclu\u00eddo no plano de prospec\u00e7\u00f5es como prospec\u00e7\u00e3o planejada") {
                statusAnotacoes.value = "";
            }

            if (nomeProspeccao && nomeProspeccao.value === "Prospec\u00e7\u00e3o planejada") {
                nomeProspeccao.value = "";
                nomeProspeccao.placeholder = "Ex.: Proposta de biossurfactantes";
            }
        }
    }
}
