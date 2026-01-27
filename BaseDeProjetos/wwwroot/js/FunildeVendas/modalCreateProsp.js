document.addEventListener('DOMContentLoaded', () => {
    // utilitário para anexar mapeamento nome -> id para um input EmpresaInput( -sufixo?)
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