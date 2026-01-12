redePessoas = null

// Auxiliar para tippy no projetos
let tippyDone = []

function previewBase64(inputId, imgId, base64Id) {
    var input = document.getElementById(inputId);
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            var imgPreview = document.getElementById(imgId);
            imgPreview.src = e.target.result;
            converterParaBase64(input.files[0], base64Id);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

function converterParaBase64(file, id) {
    var reader = new FileReader();
    reader.onloadend = function () {
        var base64String = reader.result;
        var logoBase64Input = document.getElementById(id);
        logoBase64Input.value = base64String;
    };
    reader.readAsDataURL(file);
}

function ChecarTipoProducao(id = "") {
    let valor = document.querySelector(`#select_tipo${id}`).value

    switch (valor) {
        case "8": //patente
            document.querySelector(`#campos_patente${id}`).style = 'display:block';
            document.querySelector(`#campos_status${id}`).style = 'display:block';
            document.querySelector(`#campos_doi${id}`).style = 'display:block';
            break;
        case "9": case "10": case "11": case "12": //fatos_relevantes
            document.querySelector(`#campos_patente${id}`).style = 'display:none';
            document.querySelector(`#campos_status${id}`).style = 'display:none';
            document.querySelector(`#campos_doi${id}`).style = 'display:none';
            break;
        default: //demais pubs
            document.querySelector(`#campos_patente${id}`).style = 'display:none';
            document.querySelector(`#campos_status${id}`).style = 'display:block';
            document.querySelector(`#campos_doi${id}`).style = 'display:block';
            break;
    }
}

function CasasFunil() {
    let caixa1 = document.getElementById("caixaISIQV")
    let caixa2 = document.getElementById("caixaCISHO")
    let caixa3 = document.getElementById("caixaISIII")
    let caixa4 = document.getElementById("caixaISISVP")

    const lista_caixas = [caixa1, caixa2, caixa3, caixa4];
    let caixas_ativas = []

    lista_caixas.forEach(elemento => { if (elemento.checked == true) { caixas_ativas.push(elemento) } });

    let outras_casas = ""

    for (let i = 1; i < caixas_ativas.length; i++) {
        outras_casas += "-" + caixas_ativas[i]?.value
    }

    let url_final = window.location.pathname + '?casa=' + caixas_ativas[0].value + outras_casas
    window.location.assign(url_final);
}

function passarComp(element) {
    realocarCompetenciaNaView(element);
    campoComp = document.querySelector("#filt").cloneNode(true)
    elemComp = campoComp.children
    elemComp[0].remove() // retirando a tag <p>
    listaComp = []
    for (let item of elemComp) {
        listaComp.push(item.innerHTML)
    }
    if (localStorage.getItem("Pessoas")) {
        montarNetwork(JSON.parse(localStorage.getItem("Pessoas")), listaComp)
    } else {
        fetch('/Pessoas/dados').then(response => response.json()).then(data => { montarNetwork(data, listaComp) });
    }
}

function realocarCompetenciaNaView(element) {
    if (element.classList[1] == 'bg-secondary') {
        element.classList.replace('bg-secondary', 'bg-primary');
        document.querySelector("#filt").appendChild(element);
    } else {
        element.classList.replace('bg-primary', 'bg-secondary');
        document.querySelector("#notfilt").appendChild(element);
    }
}

function loadAncora(toggleId, iconAncora, campoAgg) {
    alavanca = document.querySelector(`#${toggleId}`);
    icon = document.querySelector(`#${iconAncora}`);
    agg = document.querySelector(`#${campoAgg}`);

    if (alavanca.value == "False") {
        alavanca.checked = false;
        icon.style.color = "rgba(111, 111, 111, 0.2)";
        agg.classList.add("d-none");
    } else {
        alavanca.checked = true;
        icon.style.color = "rgba(111, 111, 111, 1)";
        agg.classList.remove("d-none");
    }
}

function checkAncora(alavanca, iconAncora, campoAgg) {
    icon = document.querySelector(`#${iconAncora}`);
    agg = document.querySelector(`#${campoAgg}`);
    if (alavanca.checked) {
        alavanca.value = "True";
        icon.style.color = "rgba(111, 111, 111, 1)";
        agg.classList.remove("d-none");
    } else {
        alavanca.value = "False";
        icon.style.color = "rgba(111, 111, 111, 0.2)";
        agg.classList.add("d-none");
    }
}

function changeDisplayStyle(element, style) {
    if (element && element.style) {
        element.style.display = style;
        return
    }
    console.warn("O elemento ou o seu style o qual se deseja mudar está nulo");
    if (element) {
        console.warn(`O elemento até existe: ${element}`);
    }
}

function preencherInputEditProjeto(idProjeto) {
    let elementoInput = document.querySelector(`#inputTextPessoas-${idProjeto}`);
    fetch(`/Projetos/RetornarMembrosCSV/${idProjeto}`)
        .then(response => response.json())
        .then(data => {
            elementoInput.value = data['data'];
        }).catch(err => {
            console.error(err);
        });
}

function preencherInputEditProspeccao(idProspeccao) {
    let elementoInput = document.querySelector(`#inputTextPessoas-${idProspeccao}`);
    fetch(`/FunilDeVendas/RetornarMembrosCSV/${idProspeccao}`)
        .then(response => response.json())
        .then(data => {
            elementoInput.value = data['data'];
        }).catch(err => {
            console.error(err);
        });
}

function gerarOpcoesSelect(rota, elementoPai, modelId = "", fillValues = false) { // os últimos 2 parâmetros para tratar no Edit
    elementoPai = `${elementoPai}${modelId}`

    let defRota = '';
    let value = '';
    let inner = '';
    let lider = '';
    let idSelect = `campoSelect${rota}${modelId}`;
    let caixaId = `caixaPesquisa${rota}${modelId}`;
    let botaoAlterar = `botaoToggleCaixaRequest${rota}${modelId}`;
    let loadingIcon = `loadingOpcoesSelect${rota}${modelId}`;

    const caixaElem = document.querySelector(`#${caixaId}`);
    let loadingIconElem = document.querySelector(`#${loadingIcon}`);
    const selectElem = document.querySelector(`#${idSelect}`);
    const liderElem = document.querySelector(`#selectLiderProsp${modelId}`);
    let select2_containers = null;
    const botaoAlterarElem = document.querySelector(`#${botaoAlterar}`);

    changeDisplayStyle(caixaElem, "none");
    changeDisplayStyle(loadingIconElem, "block");

    if (selectElem) {
        selectElem.innerHTML = '';
    }

    switch (rota) { //====================================================== \/\/\/ SWITCH PRINCIPAL \/\/\/ ===============================================================
        case "Pessoas":
            defRota = '/FunilDeVendas/PuxarDadosUsuarios';
            value = "Email";
            inner = "UserName";
            if (liderElem) {
                lider = liderElem.selectedOptions[0].text;
            };
            break;
        case "Empresas":
            defRota = '/Empresas/PuxarEmpresas';
            value = "Id";
            inner = "NomeFantasia";
            break;
        case "Tags":
            defRota = '/FunilDeVendas/PuxarTagsProspecoes';
            value = "Tags"; // talvez quebre ??
            inner = "Tags";
            break;
        case "Prospeccoes":
            defRota = '/FunilDeVendas/PuxarDadosProspeccoes2';
            value = "idProsp";
            inner = "Titulo";
            break;
        default:
            console.error(`Erro: ${rota} é uma rota inválida`);
            break;
    }               //====================================================== /\/\/\ SWITCH PRINCIPAL /\/\/\ ===============================================================
    fetch(defRota)
        .then(response => response.json())
        .then(lista => {
            lista.forEach((item) => {
                if (rota != "Pessoas" || item[inner] != lider) {
                    let opt = document.createElement("option");
                    if (rota != "Tags") { opt.value = item[value] }
                    opt.innerHTML = item[inner]
                    selectElem.appendChild(opt)
                }
            })

            changeDisplayStyle(loadingIconElem, "none");
            changeDisplayStyle(caixaElem, "block");

            if (fillValues) {
                let idText = `inputText${rota}${modelId}`;
                carregarValoresInputParaSelect(idText, idSelect, rota)
            }

            changeDisplayStyle(botaoAlterarElem, "none");

            let checkRota = document.querySelector(`#check${rota}${modelId}`);
            if (checkRota) {
                checkRota.checked = true;
            } else {
                console.error(`checkRota está nulo ou undefined. Rota: ${rota} ModelId: ${modelId ?? 'null'}`)
            }

            // ----
            // O código abaixa seta o parent para o dropdown, ou seja, o parent do Select2 que no caso de modais precisa estar explicitamente definido
            // O código usa jQuery

            if (elementoPai) {
                $(`#${idSelect}`).select2({ dropdownParent: $(`#${elementoPai}`) })
            }

            // Até aqui
            // ----

            select2_containers = document.querySelectorAll(".select2-container");
            select2_containers.forEach(input => input.style.width = "100%");
        })
        .catch(err => {
            console.error(`Erro no fetch ${err}`);
        });
}

function procurarPessoa(select) {
    redePessoas.focus(select.value, { scale: 3, animation: { duration: 400 } })
}

function selectToText(id = "") {
    let checkAlteradosId = `.changeCheck${id}`
    let checkAlteradosElem = document.querySelectorAll(checkAlteradosId);

    // Indicar quais campos select foram alterados (pra não verificar todos a toa)
    let valoresCheck = Array.from(checkAlteradosElem)
        .filter(check => check)
        .map(check => check.value);

    if (valoresCheck.length === 0) {
        console.error("Não temos um check...");
        return;
    }

    valoresCheck.forEach((rota) => {
        if (!rota) {
            console.error("A rota está nula");
        }

        const campoSelectId = `#select2-campoSelect${rota}${id}-container`;
        const campoSelectElem = document.querySelector(campoSelectId);

        if (campoSelectElem) {
            let texto = '';

            campoSelectElem.childNodes.forEach(caixa => {
                const selecoes = document.querySelector(`#campoSelect${rota}${id}`).childNodes; // buscar as options na tag select original para trazer os valores (.value)
                for (const option of selecoes) {
                    if (option.innerText === caixa.title.replace(/\s+/g, ' ')) { // compara o texto selecionado com o texto da option no loop
                        texto += `${option.value};`; // concatena os valores dos textos selecionados
                        break;
                    }
                }
            });

            const inputTextId = `#inputText${rota}${id}`
            const inputTextElem = document.querySelector(inputTextId);

            if (inputTextElem) {
                inputTextElem.value = texto
            }
            else {
                console.error("Não temos um input text...")
            }
        } else {
            console.error("Não temos um Campo Select...");
            return;
        }
    });
}

function carregarValoresInputParaSelect(idText, idSelect) {
    let listaOptions = [];
    let selectOptions = document.querySelector(`#${idSelect}`).options; // todos os valores carregados do fetch
    let listaValoresText = document.querySelector(`#${idText}`).value.split(";"); // todos os valores salvos no item

    listaValoresText.forEach(txt => { //QUANDO TODOS OS VALORES DO BANCO ESTIVEREM TRATADOS, NAO SERA MAIS NECESSARIO ITERAR PELOS VALORES EM TXT PARAR FAZER O TRIGGER NO SELECT2
        if (txt == '') { } else {
            for (var i = 0; i < selectOptions.length; i++) {
                if (selectOptions[i].value === txt) {
                    listaOptions.push(selectOptions[i].value); // a funcao .val() do select2 detecta somente o .value da option
                }
            }
        }
        $(`#${idSelect}`).val(listaOptions).trigger('change');
    })
}

function Base64(id = "") {
    imagem = document.getElementById(`logo_imagem${id}`).files[0];
    r = new FileReader();
    r.readAsDataURL(imagem);
    r.onload = function () {
        document.getElementById(`img_preview${id}`).src = r.result
        document.getElementById(`img_b64${id}`).value = r.result
    }
}

function MostrarImagem(id = "") {
    document.getElementById(`img_preview${id}`).src = document.getElementById(`img_b64${id}`).value
}

function listarCompetencias() {
    fetch('/Pessoas/dictCompetencias').
        then((response) => response.json()).
        then((json) => {
            localStorage.setItem('dictCompetencias', JSON.stringify(json))
            listaComp = Object.values(json)
            badge_def_sel = document.querySelector('#badge_competencia_select')
            listaComp.forEach(c => {
                if (c == '' || c == null) { } else {
                    badge_copy = badge_def_sel.cloneNode(true)
                    badge_copy.removeAttribute('id')
                    badge_copy.innerHTML = c
                    badge_copy.classList.remove('d-none')

                    document.querySelector(`#notfilt`).appendChild(badge_copy)
                }
            })
        })
}

function mostrarCompetencias() {
    fetch('/Pessoas/dictCompetencias').then((response) => response.json()).then((json) => {
        document.querySelector(`#campo_competencias`).innerHTML = ""
        badge_def = document.querySelector('#badge_competencia')
        listaComp = document.querySelector(`#competencias`).innerHTML.split(";")
        listaComp.forEach(num => {
            if (num == '' || num == null) { } else {
                badge_copy = badge_def.cloneNode(true)
                badge_copy.removeAttribute('id')
                badge_copy.innerHTML = json[num]
                badge_copy.classList.remove('d-none')

                document.querySelector(`#campo_competencias`).appendChild(badge_copy)
            }
        })
    })
}

function checarStatusNaoConvertida(select) {
    let naoConversaoElem = document.querySelector("#naoconversao");
    if (select.value == 7) {
        changeDisplayStyle(naoConversaoElem, "block");
    } else {
        changeDisplayStyle(naoConversaoElem, "none");
    }
}

function updateLink() {
    var baseUrl = location.href.split("?")[0];
    var params = "?ano=";
    var select = document.getElementById('ano').value;

    if (location.href == baseUrl) {
        location.href = baseUrl + params + select;
    }
    else {
        location.href = baseUrl + params + select;
    }
}

function montarNetwork(pessoas, competenciasFiltradas = null) {
    var network = null;
    let defuserpic = "https://cdn.icon-icons.com/icons2/1378/PNG/512/avatardefault_92824.png";
    var existeFiltro = false
    var listaPessoas = []
    var ligacoes = []

    if (competenciasFiltradas !== null) {
        existeFiltro = naoNulaOuVazia(competenciasFiltradas)
    }

    let dictCompetencias = JSON.parse(localStorage.getItem('dictCompetencias'))
    pessoas.forEach(pessoa => {
        let listaCompPessoa = []
        let notInclude = false
        var user = {};

        if (existeFiltro) {
            listaCompPessoa = pessoa['Competencia'].split(";").map(cp => dictCompetencias[cp])

            competenciasFiltradas.forEach(comp => {
                if (!listaCompPessoa.includes(comp)) {
                    notInclude = true;
                    return;
                }
            });
            if (notInclude) {
                return;
            }
        }

        user['id'] = pessoa['Email']
        user['title'] = pessoa['UserName']
        user['comp'] = pessoa['Competencia']

        if ((pessoa['Foto'] == null) || (pessoa['Foto'] == "")) {
            user['image'] = defuserpic
        } else if (pessoa['Foto'].includes("data:image/jpeg;base64,")) {
            user['image'] = pessoa['Foto']
        } else {
            user['image'] = "data:image/jpeg;base64," + pessoa['Foto']
        }

        pessoas.filter(ps => ps['Email'] != user['id']).forEach(pessoaParalela => { // para cada pessoa na rede inteira
            if (pessoaParalela['Competencia'] == null) {
                return; // Skip the loop iteration if 'Competencia' is null
            }
            let iterar = true;
            listaCompPessoaParalela = pessoaParalela['Competencia'].split(";").map(cp => dictCompetencias[cp])
            listaCompPessoa.every(compet => { // para cada competencia da pessoa do loop atual
                if (listaCompPessoaParalela.includes(compet)) {
                    let setUsuario = new Set([user['id'], pessoaParalela['Email']]);
                    let ligacoes = ligacoes.map(p => new Set(Object.values(p)))
                    if (ligacoes.filter(p2 => eqSet(p2, setUsuario)).length == 0) {
                        ligacoes.push({ from: user['id'], to: pessoaParalela['Email'] })
                        iterar = false;
                        return;
                    }
                }
            })
            if (iterar == false) {
                return
            }
        })

        listaPessoas.push(user)
    })
    construirGrafo(listaPessoas, ligacoes);
    //({ nodes, edges, network } = construirGrafo(nodes, listaPessoas, edges, listaLigacoes, network));
}

const eqSet = (xs, ys) => xs.size === ys.size && [...xs].every((x) => ys.has(x));

function isSuperset(set, subset) {
    for (var elem of subset) {
        if (!set.has(elem)) {
            return false;
        }
    }
    return true;
}

function construirGrafo(listaPessoas, listaLigacoes) {
    nodes = new vis.DataSet(listaPessoas); //lista de pessoas
    edges = new vis.DataSet(listaLigacoes); //lista de ligacoes
    var container = document.getElementById("chart-line");

    var data = {
        nodes: nodes,
        edges: edges,
    };

    var options = {
        nodes: {
            shape: "circularImage",
            borderWidth: 5,
            size: 20,
            color: {
                border: "#229954",
                background: "#666666",
            },
            font: { color: "#17202A" },
        },
        edges: {
            color: "#DBDBDB",
            value: 0.5,
            shadow: true,
        },
        interaction: {
            dragNodes: true,
            dragView: false,
            zoomView: false,
        },
        physics: {
            enabled: true,
        }
    };
    network = new vis.Network(container, data, options);

    network.on('click', function (selecao) {
        if (selecao.nodes.length != 0) {
            document.getElementById('user_card').style.display = 'block';
            var clickedNode = nodes.get(selecao.nodes)[0];
            network.focus(clickedNode.id, { scale: 2, animation: { duration: 400 } });
            document.getElementById("imagem-do-card").src = clickedNode.image;
            document.getElementById("nome-do-card").innerHTML = clickedNode.title;
            document.getElementById("competencias").innerHTML = clickedNode.comp;
            mostrarCompetencias();
        } else {
            //network.fit({animation:{duration: 400}})
            network.moveTo({ position: { x: 0, y: 0 }, scale: 2, animation: { duration: 400 } });
            document.getElementById("user_card").style.display = 'none';
        }
    });

    network.once('stabilized', function () {
        network.moveTo({ position: { x: 0, y: 0 }, scale: 2 });
    });

    redePessoas = network;
    //return { nodes, edges, network };
}

function naoNulaOuVazia(compFiltradas) {
    return compFiltradas != null && compFiltradas != "" && compFiltradas != [];
}

function converterCompetencias() {
    let inputComp = ""
    let dict1 = {}

    listaComp = inputComp.split(";")

    listaComp.forEach(c => {
        compString = dict1[c]
    })
}

function statusPatente(id = "") {
    let status = document.querySelector(`#StatusPub${id}`).value
    if (status != 5) {
        document.querySelector(`#NumPatente${id}`).readOnly = true;
    } else {
        document.querySelector(`#NumPatente${id}`).readOnly = false;
    }
}

function travarBotao() {
    botoes = document.querySelectorAll(".btn-submit");
    botoes.forEach(botao => {
        botao.disabled = true;
    });
}

addEventListener("submit", travarBotao);

// Não renomeie a função abaixo. Se for renomear, não utilize letra maiúscula.
// Por quê? Não faço ideia. Javascript é um mistério sem solução e não gostou disso.
function repopularnomeprojeto() {
    let campoInput = document.querySelector("#nomeProjeto");
    let inputRepopulado = document.querySelector("#repopularNomeProjeto");
    inputRepopulado.value = campoInput.value;
}

document.addEventListener("DOMContentLoaded", function () {
    // Seleciona todos os selects múltiplos de pessoas
    const selects = document.querySelectorAll("[id^='campoSelectPessoas-']");

    selects.forEach(select => {
        // Inicializa Select2 (se estiver usando)
        $(select).select2({ dropdownParent: $(select).closest('.modal') });

        // Atualiza o input hidden sempre que mudar
        $(select).on('change', function () {
            let id = select.id.split('-').pop();
            atualizarMembros(id);
        });
    });
});

// Atualiza o input hidden com os valores selecionados
function atualizarMembros(id) {
    const select = document.getElementById("campoSelectPessoas-" + id);
    const inputHidden = document.getElementById("inputTextPessoas-" + id);

    if (select && inputHidden) {
        const valoresSelecionados = $(select).val(); // pega os values selecionados do Select2
        inputHidden.value = valoresSelecionados.join(';'); // salva no formato CSV
    }
}

document.addEventListener("DOMContentLoaded", function () {
    // Inicializa Select2 para o Create
    const selectCreate = document.getElementById("campoSelectPessoas-create");
    if (selectCreate) {
        $(selectCreate).select2({ dropdownParent: $(selectCreate).closest('.modal') });

        // Atualiza o input hidden sempre que mudar
        $(selectCreate).on('change', function () {
            const inputHidden = document.getElementById("inputTextPessoas-create");
            const valoresSelecionados = $(selectCreate).val();
            inputHidden.value = valoresSelecionados.join(';'); // salva no formato CSV
        });
    }
});

document.addEventListener("DOMContentLoaded", function () {
    const selectId = "campoSelectPessoas";
    const inputId = "inputTextPessoas";

    // Inicializa o select com as opções
    gerarOpcoesSelect('Pessoas', 'criarProspModalToggle', '', true);

    // Atualiza input hidden sempre que selecionar/deselecionar
    $(document).on('change', `#${selectId}`, function () {
        const valores = $(this).val() || [];
        document.getElementById(inputId).value = valores.join(';');
    });
});
