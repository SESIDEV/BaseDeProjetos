
document.addEventListener('DOMContentLoaded', () => {
    let listNomeProspeccao = document.querySelectorAll("#NomeProspeccao");
    let listSelectEmpresa = document.querySelectorAll("#EmpresaId");

    adicionarListenerVerificacaoEmLista(listSelectEmpresa, validarSelectEmpresa);
    adicionarListenerVerificacaoEmLista(listNomeProspeccao, validarNomeProspeccao);

    const formatadorMoedaProspeccao = new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL',
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });

    function obterNumeroMoedaProspeccao(valor) {
        const texto = String(valor || '')
            .replace('R$', '')
            .replace(/\s/g, '')
            .trim();

        if (!texto) return null;

        if (texto.includes(',')) {
            return Number(texto.replace(/\./g, '').replace(',', '.'));
        }

        return Number(texto);
    }

    function formatarInputMoedaProspeccao(input) {
        const numero = obterNumeroMoedaProspeccao(input.value);
        input.value = numero === null || Number.isNaN(numero)
            ? ''
            : formatadorMoedaProspeccao.format(numero);
    }

    function prepararInputMoedaProspeccao(input) {
        const numero = obterNumeroMoedaProspeccao(input.value);
        input.value = numero === null || Number.isNaN(numero)
            ? ''
            : String(numero);
    }

    function prepararFormularioMoedaProspeccao(formulario) {
        formulario.querySelectorAll('input[data-moeda="true"]').forEach(prepararInputMoedaProspeccao);
    }

    document.querySelectorAll('input[data-moeda="true"]').forEach(formatarInputMoedaProspeccao);

    document.addEventListener('focusin', (event) => {
        const input = event.target;
        if (!(input instanceof HTMLInputElement) || input.dataset.moeda !== 'true') {
            return;
        }

        prepararInputMoedaProspeccao(input);
    });

    document.addEventListener('focusout', (event) => {
        const input = event.target;
        if (!(input instanceof HTMLInputElement) || input.dataset.moeda !== 'true') {
            return;
        }

        formatarInputMoedaProspeccao(input);
    });

    document.addEventListener('submit', (event) => {
        const formulario = event.target;
        if (!(formulario instanceof HTMLFormElement)) {
            return;
        }

        if (!formulario.querySelector('input[data-moeda="true"]')) {
            return;
        }

        prepararFormularioMoedaProspeccao(formulario);
    }, true);
});
