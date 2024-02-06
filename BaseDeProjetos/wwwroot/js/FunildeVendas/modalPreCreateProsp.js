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

    if (valorSelect !== "-1" && valorSelect !== "-2") {
        toggleButton.dataset.bsTarget = `#CreateFollowupProspModal-${valorSelect}`;

        try {
            const response = await fetch(`/FunildeVendas/RetornarModal?idProsp=${valorSelect}&tipo=CreateFollowup`);
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

        if (valorSelect === "-2") {
            const nomeProspeccao = document.getElementById("NomeProspeccao");
            const statusData = document.getElementById("Status_0__Data");
            const statusAnotacoes = document.getElementById("Status_0__Anotacoes");
            const valorStatus = document.getElementById("Status_0__Status");

            let data = new Intl.DateTimeFormat('pt-BR', formatoData).format(new Date());

            statusData.value = data;
            statusData.placeholder = data;
            statusAnotacoes.value = "Incluído no plano de prospecções como prospecção planejada";

            valorStatus.value = "Planejada";
            nomeProspeccao.value = "Prospecção Planejada";
            nomeProspeccao.placeholder = "Prospecção Planejada";
        }
    }
}