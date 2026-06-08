document.addEventListener('DOMContentLoaded', () => {
    // utilitário para anexar mapeamento nome -> id para um input EmpresaInput( -sufixo?)
    function novaEmpresaSelecionada(form) {
        const cadastrarNovaEmpresa = form?.querySelector('#CadastrarNovaEmpresa');
        return cadastrarNovaEmpresa?.value === 'true';
    }

    function atualizarModoCadastroEmpresa() {
        const selectCreateProsp = document.getElementById('selectCreateProsp');
        const prospeccaoPlanejada = selectCreateProsp?.value === '-2';
        const opcoesEmpresaNaoPlanejada = document.getElementById('opcoesEmpresaNaoPlanejada');
        const tipoEmpresaCadastrada = document.getElementById('tipoEmpresaCadastrada');
        const tipoEmpresaNova = document.getElementById('tipoEmpresaNova');
        const cadastrarNovaEmpresa = document.getElementById('CadastrarNovaEmpresa');
        const empresaCadastradaFields = document.getElementById('empresaCadastradaFields');
        const novaEmpresaFields = document.getElementById('novaEmpresaFields');
        const empresaInput = document.getElementById('EmpresaInput');
        const empresaId = document.getElementById('EmpresaId');
        const camposNovaEmpresa = document.querySelectorAll('.nova-empresa-field');
        const camposNovaEmpresaObrigatorios = document.querySelectorAll('#NomeEmpresa-prosp, #valor_cnpj-prosp');

        if (prospeccaoPlanejada) {
            if (opcoesEmpresaNaoPlanejada) opcoesEmpresaNaoPlanejada.classList.add('d-none');
            if (tipoEmpresaCadastrada) tipoEmpresaCadastrada.checked = true;
        } else if (opcoesEmpresaNaoPlanejada) {
            opcoesEmpresaNaoPlanejada.classList.remove('d-none');
        }

        const usarNovaEmpresa = !prospeccaoPlanejada && tipoEmpresaNova?.checked;

        if (cadastrarNovaEmpresa) cadastrarNovaEmpresa.value = usarNovaEmpresa ? 'true' : 'false';

        if (empresaCadastradaFields) empresaCadastradaFields.classList.toggle('d-none', usarNovaEmpresa);
        if (novaEmpresaFields) novaEmpresaFields.classList.toggle('d-none', !usarNovaEmpresa);

        if (empresaInput) {
            empresaInput.disabled = usarNovaEmpresa;
            empresaInput.required = !usarNovaEmpresa;
            if (usarNovaEmpresa) empresaInput.value = '';
        }

        if (empresaId && usarNovaEmpresa) empresaId.value = '-1';

        camposNovaEmpresa.forEach((campo) => {
            campo.disabled = !usarNovaEmpresa;
        });

        camposNovaEmpresaObrigatorios.forEach((campo) => {
            campo.required = usarNovaEmpresa;
        });
    }

    window.atualizarModoCadastroEmpresaProsp = atualizarModoCadastroEmpresa;
    window.checarCNPJProspeccao = async function () {
        const cnpj = document.getElementById('valor_cnpj-prosp');
        const status = document.getElementById('StatusCNPJ-prosp');

        if (!cnpj) return;

        cnpj.value = cnpj.value.replace(/[^0-9]/g, '');

        if (status) status.innerHTML = 'Checando...';

        try {
            const response = await fetch(`/Empresas/SeExisteCnpj?cnpj=${encodeURIComponent(cnpj.value)}`);
            const data = await response.json();

            if (data == 0) {
                if (status) status.innerHTML = '';
                cnpj.style.borderColor = '';
                validarCNPJ('-prosp');
            } else {
                if (status) status.innerHTML = '<font color="Red">CNPJ já cadastrado.</font>';
                cnpj.style.borderColor = 'Red';
            }
        } catch (err) {
            if (status) status.innerHTML = '<font color="Red">Não foi possível consultar o CNPJ.</font>';
            console.error('[modalCreateProsp] erro ao consultar CNPJ:', err);
        }
    };

    document.getElementById('tipoEmpresaCadastrada')?.addEventListener('change', atualizarModoCadastroEmpresa);
    document.getElementById('tipoEmpresaNova')?.addEventListener('change', atualizarModoCadastroEmpresa);
    atualizarModoCadastroEmpresa();

    function attachEmpresaMapper(empresaInput) {
        if (!empresaInput) return;

        const suffix = empresaInput.id.replace(/^EmpresaInput/, ''); // '' ou '-{id}'
        const datalist = document.getElementById(`empresas${suffix}`);
        const empresaIdEl = document.getElementById(`EmpresaId${suffix}`); // hidden id com sufixo
        const form = empresaInput.closest('form');

        function mapEmpresa() {
            const value = (empresaInput.value || '').trim();
            const options = [...(datalist?.options || [])];
            // busca exato (case-insensitive) ou por substring
            let option = options.find(o => (o.value || '').trim().toLowerCase() === value.toLowerCase());
            if (!option && value) {
                option = options.find(o => (o.value || '').toLowerCase().includes(value.toLowerCase()));
            }

            if (option && empresaIdEl) {
                empresaIdEl.value = option.dataset.id || "";
            } else if (empresaIdEl) {
                empresaIdEl.value = "";
            }
        }

        empresaInput.addEventListener('change', mapEmpresa);
        empresaInput.addEventListener('blur', mapEmpresa);

        // mapear imediatamente (útil no edit para preencher hidden quando modal abre)
        mapEmpresa();

        if (form) {
            form.addEventListener('submit', function (e) {
                mapEmpresa();
                if (novaEmpresaSelecionada(form)) {
                    return;
                }

                if (!empresaIdEl || !empresaIdEl.value) {
                    e.preventDefault();
                    const span = form.querySelector('span[asp-validation-for="EmpresaId"]') || form.querySelector('.text-danger');
                    if (span) span.textContent = 'Selecione uma empresa válida da lista.';
                    empresaInput.focus();
                }
            });
        }
    }

    // Anexar para create (id sem sufixo)
    const empresaInputCreate = document.getElementById('EmpresaInput');
    if (empresaInputCreate) attachEmpresaMapper(empresaInputCreate);

    // Anexar para todos os edits: inputs cujo id começa com 'EmpresaInput-'
    const editInputs = document.querySelectorAll('input[id^="EmpresaInput-"]');
    editInputs.forEach(inp => attachEmpresaMapper(inp));

    // manter validações existentes (se presentes) sem quebrar
    try {
        adicionarListenerVerificacao(document.getElementById("EmpresaId"), window.validarSelectEmpresa);
        adicionarListenerVerificacao(document.getElementById("NomeProspeccao"), window.validarNomeProspeccao);
    } catch (err) {
        console.debug('[modalCreateProsp] validações auxiliares não anexadas:', err);
    }
});
