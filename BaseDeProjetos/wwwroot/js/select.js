function gerarOpcoesSelect(nomeModal, idSelect, rota, caixaId, botaoAlterar, loadingIcon) {
    let defRota = "";
    let value = "";
    let inner = "";

    const caixa = document.querySelector(`#${caixaId}`);
    const loading = document.querySelector(`#${loadingIcon}`);
    const select = document.querySelector(`#${idSelect}`);

    caixa.style.display = "none";
    loading.style.display = "block";
    select.innerHTML = '';

    if (nomeModal == null) {
        $(`#${idSelect}`).select2();
    } else {
        $(`#${idSelect}`).select2({ dropdownParent: $(`#${nomeModal}`) });
    }

    switch (rota) {
        case "Pessoas":
            defRota = '/FunilDeVendas/PuxarDadosUsuarios';
            value = "Email";
            inner = "UserName";
            break;
        case "Empresas":
            defRota = '/Empresas/PuxarEmpresas';
            value = "Id";
            inner = "RazaoSocial";
            break;
        case "Tags":
            defRota = '/FunilDeVendas/PuxarTagsProspecoes';
            break;
        case "Prospeccoes":
            defRota = '/FunilDeVendas/PuxarDadosProspeccoes2';
            value = "idProsp";
            inner = "Titulo";
            break;
        default:
            console.log(`Erro: ${rota} é uma rota inválida`);
            break;
    }

    fetch(defRota)
        .then(response => response.json())
        .then(lista => {
            lista.forEach(function (item) {
                const opt = document.createElement("option");
                if (rota !== "Tags") {
                    opt.value = item[value];
                }
                opt.innerHTML = item[inner];
                select.appendChild(opt);
            });

            loading.style.display = "none";
            caixa.style.display = "block";
        });

    document.querySelectorAll(".select2-container").forEach(input => {
        input.style.width = "100%";
    });

    if (botaoAlterar != null) {
        document.querySelector(`#${botaoAlterar}`).style.display = "none";
    }
}

function novaTag() {
    valor = document.querySelector(`.select2-search__field`).value
    document.querySelector(`.select2-search__field`).valorSalvo = valor
}

function addTag(campoInput, idSelect) {
    const valor = document.querySelector('.select2-search__field').valorSalvo;
    const valorAntigo = document.querySelector(`#${campoInput}`).value;

    let valorNovo;
    if (valorAntigo === '') {
        valorNovo = `#${valor}`;
    } else {
        valorNovo = `${valorAntigo};#${valor}`;
    }

    document.querySelector(`#${campoInput}`).value = valorNovo;

    const botao_def = document.querySelector('#bloco_botao');
    const botao_copy = botao_def.cloneNode(true);
    botao_copy.removeAttribute('id');
    botao_copy.title = valor;
    botao_copy.children[0].innerHTML = `#${valor}`;
    botao_copy.classList.remove('d-none');
    document.querySelector(`#select2-${idSelect}-container`).appendChild(botao_copy);
    document.querySelector('.select2-search__field').value = '';
}

function textToSelect(nomeModal, idText, idSelect, rota, caixaId, botaoAlterar, loadingIcon) {
    gerarOpcoesSelect(nomeModal, idSelect, rota, caixaId, botaoAlterar, loadingIcon);

    const botaoDef = document.querySelector('#bloco_botao');
    const listaSel = document.querySelector(`#${idText}`).value.split(";");

    listaSel.forEach(p => {
        if (p !== '') {
            const botaoCopy = botaoDef.cloneNode(true);
            botaoCopy.removeAttribute('id');
            botaoCopy.title = p;
            botaoCopy.children[1].innerHTML = p;
            botaoCopy.classList.remove('d-none');

            document.querySelector(`#select2-${idSelect}-container`).appendChild(botaoCopy);
        }
    });
}