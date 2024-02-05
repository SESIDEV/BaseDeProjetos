document.addEventListener('DOMContentLoaded', () => {
    let anotacoes = document.querySelectorAll("#Anotacoes");

    anotacoes.forEach(anotacao => {
        if (anotacao) {
            anotacao.addEventListener("change", () => verificarAnotacoesProspeccao(anotacao));
        }
        else {
            console.error(`No anotacao: ${anotacao}`);
        }
    });
});