(function () {
    function inicializarModalCreateProsp() {
        if (window.modalCreateProspInicializado) {
            return;
        }

        window.modalCreateProspInicializado = true;

    function normalizarTexto(valor) {
        return (valor || '').trim().toLowerCase();
    }

    function obterCasaAtualProspecao() {
        const selectCasa = document.getElementById('Casa');
        return selectCasa ? String(selectCasa.value != null ? selectCasa.value : '') : '';
    }

    function obterTipoAssociacaoSelecionado() {
        const tipoAssociacaoSelecionado = document.querySelector('input[name="tipoAssociacaoProspecao"]:checked');
        return tipoAssociacaoSelecionado ? tipoAssociacaoSelecionado.value : 'nova';
    }

    function obterBaseProspecoesRelacionadas() {
        const selectBase = document.getElementById('prospeccoesRelacionadasBase');
        if (!selectBase) {
            return [];
        }

        return Array.from(selectBase.options).map((option) => ({
            id: option.value,
            casa: option.dataset.casa || '',
            casaNome: option.dataset.casaNome || '',
            label: option.dataset.label || option.textContent.trim()
        }));
    }

    function preencherBuscaRelacionamento(input, datalist, prospeccoes, incluirCasaNoTexto) {
        if (!input || !datalist) {
            return;
        }

        const valorAtual = input.value;
        datalist.innerHTML = '';

        prospeccoes.forEach((prospeccao) => {
            const option = document.createElement('option');
            option.value = incluirCasaNoTexto
                ? prospeccao.label + ' (' + prospeccao.casaNome + ')'
                : prospeccao.label;
            option.dataset.id = prospeccao.id;
            datalist.appendChild(option);
        });

        const valorAindaExiste = Array.from(datalist.options)
            .some((option) => normalizarTexto(option.value) === normalizarTexto(valorAtual));

        if (valorAindaExiste) {
            input.value = valorAtual;
        }
    }

    function obterIdSelecionadoRelacionamento(input, datalist) {
        if (!input || !datalist) {
            return '';
        }

        const valor = normalizarTexto(input.value);
        if (!valor) {
            return '';
        }

        const options = Array.from(datalist.options);
        const option = options.find((item) => normalizarTexto(item.value) === valor);

        return option && option.dataset ? option.dataset.id || '' : '';
    }

    function atualizarAssociacaoProspecaoCreate() {
        const tipoAssociacao = obterTipoAssociacaoSelecionado();
        const casaAtual = obterCasaAtualProspecao();
        const prospeccoesRelacionadas = obterBaseProspecoesRelacionadas();
        const hiddenPrincipalId = document.getElementById('ProspeccaoPrincipalId');
        const validacao = document.getElementById('validacaoProspecaoPrincipal');
        const blocoMesmaCasa = document.getElementById('associacaoMesmaCasaFields');
        const blocoOutraCasa = document.getElementById('associacaoOutraCasaFields');
        const inputMesmaCasa = document.getElementById('inputProspecaoPrincipalMesmaCasa');
        const inputOutraCasa = document.getElementById('inputProspecaoPrincipalOutraCasa');
        const listaMesmaCasa = document.getElementById('listaProspecaoPrincipalMesmaCasa');
        const listaOutraCasa = document.getElementById('listaProspecaoPrincipalOutraCasa');

        const prospeccoesMesmaCasa = prospeccoesRelacionadas.filter((prospeccao) => prospeccao.casa === casaAtual);
        const prospeccoesOutraCasa = prospeccoesRelacionadas.filter((prospeccao) => prospeccao.casa !== casaAtual);

        preencherBuscaRelacionamento(inputMesmaCasa, listaMesmaCasa, prospeccoesMesmaCasa, false);
        preencherBuscaRelacionamento(inputOutraCasa, listaOutraCasa, prospeccoesOutraCasa, true);

        if (blocoMesmaCasa) {
            blocoMesmaCasa.classList.toggle('d-none', tipoAssociacao !== 'mesma-casa');
        }

        if (blocoOutraCasa) {
            blocoOutraCasa.classList.toggle('d-none', tipoAssociacao !== 'outra-casa');
        }

        let valorSelecionado = '';
        if (tipoAssociacao === 'mesma-casa') {
            valorSelecionado = obterIdSelecionadoRelacionamento(inputMesmaCasa, listaMesmaCasa);
        } else if (tipoAssociacao === 'outra-casa') {
            valorSelecionado = obterIdSelecionadoRelacionamento(inputOutraCasa, listaOutraCasa);
        }

        if (hiddenPrincipalId) {
            hiddenPrincipalId.value = tipoAssociacao === 'nova' ? '' : valorSelecionado;
        }

        if (validacao) {
            validacao.textContent = '';
        }
    }

    window.sincronizarAssociacaoProspecaoCreate = function () {
        const tipoAssociacao = window.tipoAssociacaoPreselecionada || 'nova';
        const radio = document.querySelector('input[name="tipoAssociacaoProspecao"][value="' + tipoAssociacao + '"]');
        const inputMesmaCasa = document.getElementById('inputProspecaoPrincipalMesmaCasa');
        const inputOutraCasa = document.getElementById('inputProspecaoPrincipalOutraCasa');
        const hiddenPrincipalId = document.getElementById('ProspeccaoPrincipalId');

        if (radio) {
            radio.checked = true;
        }

        if (inputMesmaCasa) {
            inputMesmaCasa.value = '';
        }

        if (inputOutraCasa) {
            inputOutraCasa.value = '';
        }

        if (hiddenPrincipalId) {
            hiddenPrincipalId.value = '';
        }

        atualizarAssociacaoProspecaoCreate();
    };

    function novaEmpresaSelecionada(form) {
        const cadastrarNovaEmpresa = form ? form.querySelector('#CadastrarNovaEmpresa') : null;
        return cadastrarNovaEmpresa && cadastrarNovaEmpresa.value === 'true';
    }

    function atualizarModoCadastroEmpresa() {
        const selectCreateProsp = document.getElementById('selectCreateProsp');
        const prospeccaoPlanejada = selectCreateProsp && selectCreateProsp.value === '-2';
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

        const usarNovaEmpresa = !prospeccaoPlanejada && tipoEmpresaNova && tipoEmpresaNova.checked;

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
            const response = await fetch('/Empresas/SeExisteCnpj?cnpj=' + encodeURIComponent(cnpj.value));
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

    const tipoEmpresaCadastradaEl = document.getElementById('tipoEmpresaCadastrada');
    const tipoEmpresaNovaEl = document.getElementById('tipoEmpresaNova');

    if (tipoEmpresaCadastradaEl) {
        tipoEmpresaCadastradaEl.addEventListener('change', atualizarModoCadastroEmpresa);
    }

    if (tipoEmpresaNovaEl) {
        tipoEmpresaNovaEl.addEventListener('change', atualizarModoCadastroEmpresa);
    }
    atualizarModoCadastroEmpresa();

    document.querySelectorAll('input[name="tipoAssociacaoProspecao"]').forEach((radio) => {
        radio.addEventListener('change', atualizarAssociacaoProspecaoCreate);
    });

    ['input', 'change', 'blur'].forEach((eventName) => {
        const inputMesmaCasa = document.getElementById('inputProspecaoPrincipalMesmaCasa');
        const inputOutraCasa = document.getElementById('inputProspecaoPrincipalOutraCasa');

        if (inputMesmaCasa) {
            inputMesmaCasa.addEventListener(eventName, atualizarAssociacaoProspecaoCreate);
        }

        if (inputOutraCasa) {
            inputOutraCasa.addEventListener(eventName, atualizarAssociacaoProspecaoCreate);
        }
    });

    const casaEl = document.getElementById('Casa');
    const modalCreateEl = document.getElementById('criarProspModalToggle');

    if (casaEl) {
        casaEl.addEventListener('change', atualizarAssociacaoProspecaoCreate);
    }

    if (modalCreateEl) {
        modalCreateEl.addEventListener('show.bs.modal', window.sincronizarAssociacaoProspecaoCreate);
    }

    atualizarAssociacaoProspecaoCreate();

    function attachEmpresaMapper(empresaInput) {
        if (!empresaInput) return;

        const suffix = empresaInput.id.replace(/^EmpresaInput/, '');
        const datalist = document.getElementById('empresas' + suffix);
        const empresaIdEl = document.getElementById('EmpresaId' + suffix);
        const form = empresaInput.closest('form');

        function mapEmpresa() {
            const value = (empresaInput.value || '').trim();
            const options = Array.from(datalist ? datalist.options : []);
            let option = options.find(o => (o.value || '').trim().toLowerCase() === value.toLowerCase());
            if (!option && value) {
                option = options.find(o => (o.value || '').toLowerCase().includes(value.toLowerCase()));
            }

            if (option && empresaIdEl) {
                empresaIdEl.value = option.dataset.id || '';
            } else if (empresaIdEl) {
                empresaIdEl.value = '';
            }
        }

        empresaInput.addEventListener('change', mapEmpresa);
        empresaInput.addEventListener('blur', mapEmpresa);
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

    const empresaInputCreate = document.getElementById('EmpresaInput');
    if (empresaInputCreate) attachEmpresaMapper(empresaInputCreate);

    const editInputs = document.querySelectorAll('input[id^="EmpresaInput-"]');
    editInputs.forEach(inp => attachEmpresaMapper(inp));

    const createForm = document.querySelector('#criarProspModalToggle form') || document.querySelector('form[action*="/FunilDeVendas/Create"]');
    if (createForm) {
        createForm.addEventListener('submit', function (event) {
            atualizarAssociacaoProspecaoCreate();

            const tipoAssociacao = obterTipoAssociacaoSelecionado();
            const hiddenPrincipalId = document.getElementById('ProspeccaoPrincipalId');
            const validacao = document.getElementById('validacaoProspecaoPrincipal');

            if ((tipoAssociacao === 'mesma-casa' || tipoAssociacao === 'outra-casa') && !(hiddenPrincipalId && hiddenPrincipalId.value)) {
                event.preventDefault();
                if (validacao) {
                    validacao.textContent = 'Selecione a prospecção principal para continuar.';
                }
            }
        });
    }

    try {
        adicionarListenerVerificacao(document.getElementById('EmpresaId'), window.validarSelectEmpresa);
        adicionarListenerVerificacao(document.getElementById('NomeProspeccao'), window.validarNomeProspeccao);
    } catch (err) {
        console.debug('[modalCreateProsp] validações auxiliares não anexadas:', err);
    }
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', inicializarModalCreateProsp);
    } else {
        inicializarModalCreateProsp();
    }
})();
