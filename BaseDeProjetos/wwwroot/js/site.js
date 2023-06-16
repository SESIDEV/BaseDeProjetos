/*O que eu preciso fazer?
 *
 * Função que captura o ano selecionado e filtra os dados a serem exibidos.
 * 1 - Quando o ano for selecionado.
 * 2 - Insira novos parâmetros na url.
 * 3 - Existe parâmetro na url?
 * 4 - Não: Então insira.
 * 5 - Sim: Então altere para o novo parâmetro selecionado.
 * 6 - Fim!
 *
 * */

redePessoas = null

var dictCNAE = {
    "02" : "PRODUÇÃO FLORESTAL (EXTRAÇÃO DE MADEIRA, PRODUÇÃO DE CARVÃO, COLETA DE LÁTEX | CNAE: 210107, 210108, 220901, 220902, 220904)",
    "05" : "EXTRAÇÃO DE CARVÃO MINERAL",
    "06" : "EXTRAÇÃO DE PETRÓLEO E GÁS NATURAL",
    "07" : "EXTRAÇÃO DE MINERAIS METÁLICOS",
    "08" : "EXTRAÇÃO DE MINERAIS NÃO-METÁLICOS",
    "09" : "ATIVIDADES DE APOIO À EXTRAÇÃO DE MINERAIS",
    "10" : "FABRICAÇÃO DE PRODUTOS ALIMENTÍCIOS",
    "11" : "FABRICAÇÃO DE BEBIDAS",
    "12" : "FABRICAÇÃO DE PRODUTOS DO FUMO",
    "13" : "FABRICAÇÃO DE PRODUTOS TÊXTEIS",
    "14" : "CONFECÇÃO DE ARTIGOS DE VESTUÁRIOS E ACESSÓRIOS",
    "15" : "PREPARAÇÃO DE COUROS E FABRICAÇÃO DE ARTEFATOS DE COURO, ARTIGOS PARA VIAGEM E CALÇADOS",
    "16" : "FABRICAÇÃO DE PRODUTOS DE MADEIRA",
    "17" : "FABRICAÇÃO DE CELULOSE, PAPEL E PRODUTOS DE PAPEL",
    "18" : "IMPRESSÃO E REPRODUÇÃO DE GRAVAÇÕES",
    "19" : "FABRICAÇÃO DE COQUE, DE PRODUTOS DERIVADOS DO PETRÓLEO E DE BIOCOMBUSTÍVEL",
    "20" : "FABRICAÇÃO DE PRODUTOS QUÍMICOS",
    "21" : "FABRICAÇÃO DE PRODUTOS FARMOQUÍMICOS E FARMACÊUTICOS",
    "22" : "FABRICAÇÃO DE PRODUTOS DE BORRACHA E DE MATERIAL PLÁSTICO",
    "23" : "FABRICAÇÃO DE PRODUTOS DE MINERAIS NÃO-METÁLICOS",
    "24" : "METALURGIA",
    "25" : "FABRICAÇÃO DE PRODUTOS DE METAL, EXCETO MÁQUINAS E EQUIPAMENTOS",
    "26" : "FARBICAÇÃO DE EQUIPAMENTOS DE INFORMÁTICA, PRODUTOS ELETRÔNICOS E ÓPTICOS",
    "27" : "FABRICAÇÃO DE MÁQUINAS, APARELHOS E MATERIAIS ELÉTRICOS",
    "28" : "FABRICAÇÃO DE MÁQUINAS E EQUIPAMENTOS",
    "29" : "FABRICAÇÃO DE VEÍCULOS AUTOMOTORES, REBOQUES E CARROCERIAS",
    "30" : "FABRICAÇÃO DE OUTROS EQUIPAMENTOS DE TRANSPORTE, EXCETO VEÍCULOS AUTOMOTORES",
    "31" : "FABRICAÇÃO DE MÓVEIS",
    "32" : "FABRICAÇÃO DE PRODUTOS DIVERSOS",
    "33" : "MANUTENÇÃO, REPARAÇÃO E INSTALAÇÃO DE MÁQUINAS E EQUIPAMENTOS",
    "35" : "ELETRICIDADE, GÁS E OUTRAS UTILIDADES",
    "36" : "CAPTAÇÃO, TRATAMENTO E DISTRIBUIÇÃO DE ÁGUA",
    "37" : "ESGOTO E ATIVIDADES RELACIONADAS",
    "38" : "COLETA, TRATAMENTO E DISPOSIÇÃO DE RESÍDUOS",
    "39" : "DESCONTAMINAÇÃO E OUTROS SERVIÇOS DE GESTÃO DE RESÍDUOS",
    "41" : "CONSTRUÇÃO DE EDIFÍCIOS",
    "42" : "OBRAS DE INFRA-ESTRUTURA",
    "43" : "SERVIÇOS ESPECIALIZADOS PARA CONSTRUÇÃO",
    "45" : "REPARAÇÃO DE VEÍCULOS AUTOMOTORES E MOTOCICLETAS",
    "49" : "TRANSPORTE TERRESTRE",
    "52" : "ARMAZENAMENTO E ATIVIDADES AUXILIARES DOS TRANSPORTES (CONCESSIONÁRIAS DE RODÓVIAS, PONTE, TÚNEIS E SERVIÇOS RELACIONADOS – CNAE 5221400)",
    "53" : "CORREIO E OUTRAS ATIVIDADES DE ENTREGA",
    "56" : "ALIMENTAÇÃO (FORNECIMENTO DE ALIMENTOS PREPARADOS PREPONDERANTEMENTE PARA EMPRESAS – CNAE: 5620101)",
    "59" : "ATIVIDADES CINEMATOGRÁFICAS, PRODUÇÃO DE VÍDEOS E DE PROGRAMAS DE TELEVISÃO; GRAVAÇÃO DE SOM E EDUCAÇÃO DE MÚSICA (ESTÚDIOS CINEMATOGRÁFICOS – CNAE: 5911101)",
    "60" : "TELECOMUNICAÇÕES",
    "71" : "SERVIÇOS DE ARQUITETURA E ENGENHARIA; TESTES E ANÁLISES TÉCNICAS (SERVIÇOS DE ENGENHARIA – CNAE: 7112000, SERVIÇO SOCIAL DA INDÚSTRIA – SESI – SERVIÇOS DE PERÍCIA TÉCNICA RELACIONADOS À SEGURANÇA DO TRABALHO – SERVIÇO SOCIAL DA INDÚSTRIA, CNAE: 7119704",
    "77" : "ALUGUÉIS NÃO-IMOBILIÁRIOS E GESTÃO DE ATIVOS INTANGÍVEIS NÃOFINANCEIROS (ALUGUEL DE ANDAÍMES, CNAE: 7732202)",
    "85" : "EDUCAÇÃO (SERVIÇO NACIONAL DE APRENDIZAGEM DA INDÚTRIA – SENAI – OUTRAS ATIVIDADES DE ENSINO NÃO ESPECIFICADAS ANTERIORMENTE – CNAE: 8599699)",
    "91" : "ATIVIDADES LIGADAS AO PATRIMÔNIO CULTURAL E AMBIENTAL (RESTAURAÇÃO E CONSERVAÇÃO DE LUGARES E PRÉDIOS HISTÓRICOS, CNAE: 9102302)"
}

function FiltroEmpresaEstrangeira(){

    let checkBox = document.getElementById("empresa_estrangeira_check");
    let cnpj = document.getElementById("valor_cnpj");
    let campo1 = document.getElementById("cadastro_campo1");
    let campo2 = document.getElementById("cadastro_campo2");
    let nome = document.getElementById("NomeEmpresaCadastro");
    let estado_int = document.getElementById("EstadoEmpresaCadastroINT");


    if (checkBox.checked == true) {

        cnpj.value = "00000000000000";
        campo1.style.display = "none"
        estado_int.value = 26;
        campo2.style.display = "none"
        nome.removeAttribute('readonly')

    } else {

        cnpj.value = "";
        campo1.style.display = "block"
        estado_int.value = 0;
        campo2.style.display = "block"
        nome.setAttribute('readonly')

    }
    
}

function ChecarPatente(){

    let valor = document.querySelector('#select_tipo').value
    if (valor == 8){
        document.querySelector('#campos-patente').style = 'display:block'
    }
    else{
        document.querySelector('#campos-patente').style = 'display:none'
    }
}

function CasasFunil() {

    let caixa1 = document.getElementById("caixaISIQV")
    let caixa2 = document.getElementById("caixaCISHO")
    let caixa3 = document.getElementById("caixaISIII")
    let caixa4 = document.getElementById("caixaISISVP")

    const lista_caixas = [caixa1, caixa2, caixa3, caixa4];
    let caixas_ativas = []

    lista_caixas.forEach(elemento => {if (elemento.checked == true){caixas_ativas.push(elemento)}});

    let outras_casas = ""

    for (let i = 1; i < caixas_ativas.length; i++){
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
    console.log(listaComp)
    console.log(elemComp)
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

function loadAncora(alavanca, iconAncora, campoAgg) {
    icon = document.querySelector(`#${iconAncora}`);
    agg = document.querySelector(`#${campoAgg}`);
    if(alavanca.value == "False"){
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
    if(alavanca.checked){
        alavanca.value = "True";
        icon.style.color = "rgba(111, 111, 111, 1)";
        agg.classList.remove("d-none");
    } else {
        alavanca.value = "False";
        icon.style.color = "rgba(111, 111, 111, 0.2)";
        agg.classList.add("d-none");
    }
}

function gerarOpcoesSelect(nomeModal, idSelect, rota, caixaId, botaoAlterar) {
    let defRota = "";
    let value = "";
    let inner = "";
    document.querySelector(`#loadingOpcoesSelect${rota}`).style.display = "block";
    document.querySelector(`#${idSelect}`).innerHTML = '';
    if (nomeModal == null){
        $(`#${idSelect}`).select2()
    } else {
        $(`#${idSelect}`).select2({dropdownParent: $(`#${nomeModal}`)})
    }
    switch(rota){
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
    fetch(defRota).then(response => response.json()).then(lista => {
        lista.forEach(function (item){
            var opt = document.createElement("option");
            if(rota != "Tags"){opt.value = item[value]}
            opt.innerHTML = item[inner]
            document.querySelector(`#${idSelect}`).appendChild(opt)
        })
        document.querySelectorAll(".select2-container").forEach(input => {input.style.width = "100%"})
    })
    if (botaoAlterar != null){document.querySelector(`#${botaoAlterar}`).style.display = "none";}
    document.querySelector(`#loadingOpcoesSelect${rota}`).style.display = "none";
    document.querySelector(`#${caixaId}`).style.display = "block";
}

function novaTag(){
    valor = document.querySelector(`.select2-search__field`).value
    document.querySelector(`.select2-search__field`).valorSalvo = valor
}

function addTag(campoInput, idSelect) {
    valor = document.querySelector(`.select2-search__field`).valorSalvo
    valorAntigo = document.querySelector(`#${campoInput}`).value

    if (valorAntigo == '') {
        valorNovo = "#" + valor
    } else {
        valorNovo = valorAntigo + ";" + "#" + valor
    }

    document.querySelector(`#${campoInput}`).value = valorNovo
    botao_def = document.querySelector('#bloco_botao')
    botao_copy = botao_def.cloneNode(true)
    botao_copy.removeAttribute('id')
    botao_copy.title = valor
    botao_copy.children[0].innerHTML = "#" + valor
    botao_copy.classList.remove('d-none')
    document.querySelector(`#select2-${idSelect}-container`).appendChild(botao_copy)
    document.querySelector(`.select2-search__field`).value = ''
}

function procurarPessoa(select) {
    redePessoas.focus(select.value, { scale: 3, animation: { duration: 400 } })
}

function selectToText(idSelect, idText) {
    let listaRota = ["Pessoas", "Empresas"]
    listaRota.forEach(function(rota) {
        let texto = ''
        document.querySelector(`#select2-${idSelect}${rota}-container`).childNodes.forEach(p => texto += p.title + ';')
        document.querySelector(`#${idText}${rota}`).value = texto
      })
    
}

function textToSelect(nomeModal, idText, idSelect, rota, caixaId, botaoAlterar) {
    gerarOpcoesSelect(nomeModal, idSelect, rota, caixaId, botaoAlterar)

    botao_def = document.querySelector('#bloco_botao')
    listaSel = document.querySelector(`#${idText}`).value.split(";")
    listaSel.forEach(p => {
        if (p == '') { } else {
            botao_copy = botao_def.cloneNode(true)
            botao_copy.removeAttribute('id')
            botao_copy.title = p
            botao_copy.children[1].innerHTML = p
            botao_copy.classList.remove('d-none')

            document.querySelector(`#select2-${idSelect}-container`).appendChild(botao_copy)
        }
    })
}

function Base64(id) {
    imagem = document.getElementById(`logo_imagem-${id}`).files[0];
    r = new FileReader();
    r.readAsDataURL(imagem);
    r.onload = function () {
        document.getElementById(`img_preview-${id}`).src = r.result
        document.getElementById(`img_b64-${id}`).value = r.result
    }
}

function MostrarImagem(id) {
    document.getElementById(`img_preview-${id}`).src = document.getElementById(`img_b64-${id}`).value
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
    if (select.value == 7) {
        document.querySelector('#naoconversao').style.display = 'block';
    } else {
        document.querySelector('#naoconversao').style.display = 'none';
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

function montarNetwork(pessoas, compFiltradas = null) {
    var nodes = null;
    var edges = null;
    var network = null;
    let defuserpic = "https://cdn.icon-icons.com/icons2/1378/PNG/512/avatardefault_92824.png";
    var existeFiltro = false
    var listaPessoas = []
    var listaLigacoes = []

    if (compFiltradas !== null) {
        existeFiltro = NaoEhNulaOuVazia(compFiltradas)
    }
    let dictCompetencias = JSON.parse(localStorage.getItem('dictCompetencias'))
    pessoas.forEach(p => {
        let listaCompPessoa = []
        let notInclude = false
        var user = {};

        if (existeFiltro) {

            listaCompPessoa = p['Competencia'].split(";").map(cp => dictCompetencias[cp])

            compFiltradas.forEach(comp => {
                if (!listaCompPessoa.includes(comp)) {
                    notInclude = true;
                    return;
                }
            });
            if (notInclude) {
                return;
            }
        }

        user['id'] = p['Email']
        user['title'] = p['UserName']
        user['comp'] = p['Competencia']

        if ((p['Foto'] == null) || (p['Foto'] == "")) {
            user['image'] = defuserpic
        } else if (p['Foto'].includes("data:image/jpeg;base64,")){
            user['image'] = p['Foto']
        } else {
            user['image'] = "data:image/jpeg;base64," + p['Foto']
        }
       
        pessoas.filter(ps => ps['Email'] != user['id']).forEach(pessoaParalela => { // para cada pessoa na rede inteira
            if (pessoaParalela['Competencia'] == null) {
                return; // Skip the loop iteration if 'Competencia' is null
            }
            let iterar = true;
            listaCompPessoaParalela = pessoaParalela['Competencia'].split(";").map(cp => dictCompetencias[cp])
            listaCompPessoa.every(compet => { // para cada competencia da pessoa do loop atual
            if (listaCompPessoaParalela.includes(compet)) {
                let setUsuario = new Set([user['id'],pessoaParalela['Email']]);
                let ligacoes = listaLigacoes.map(p=> new Set(Object.values(p)))
                console.log(ligacoes)
                if (ligacoes.filter(p2=> eqSet(p2,setUsuario)).length == 0){
                        listaLigacoes.push({from: user['id'], to: pessoaParalela['Email']})
                        iterar = false;
                        return;
                    } 
            }
           
            })
            if (iterar == false){
                return
            }
        })

        listaPessoas.push(user)
    })
    construirGrafo(nodes, listaPessoas, edges, listaLigacoes, network);
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

function construirGrafo(nodes, listaPessoas, edges, listaLigacoes, network) {
    nodes = new vis.DataSet(listaPessoas); //lista de pessoas

    edges = new vis.DataSet(listaLigacoes);
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
        physics:{
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

function NaoEhNulaOuVazia(compFiltradas) {
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

function statusPatente(){

    let status = document.querySelector("#StatusPub").value
    if (status != 5){
        document.querySelector("#NumPatente").readOnly = true;
    } else {
        document.querySelector("#NumPatente").readOnly = false;
    }

}

function validarCNPJ(idElemento=null) {
    let cnpj;
    if (idElemento == null) {
        document.getElementById("valor_cnpj").value = document.getElementById("valor_cnpj").value.replace(/[^0-9]/g, '');
        cnpj = document.getElementById("valor_cnpj").value;
    } else {
        document.getElementById(`valor_cnpj-${idElemento}`).value = document.getElementById(`valor_cnpj-${idElemento}`).value.replace(/[^0-9]/g, '');
        cnpj = document.getElementById(`valor_cnpj-${idElemento}`).value;
    }
    if (isNaN(cnpj) || cnpj.length < 14) {
        alert("CNPJ inválido");
    } else {
        AplicarDadosAPI(idElemento);
    }    
}

function checarCNAE(listaCNAE, idElemento=null){
    possuiIndustrial = false;
    listaCNAE.forEach(codcnae => {
        
        if(typeof(dictCNAE[codcnae]) != "undefined"){

            if(idElemento == null){
                document.getElementById("BoolCnaeIndustrial").value = "1";
                document.getElementById("checkCNAE").style.color = "green";
                document.getElementById("checkCNAE").classList.value = "fa fa-check";
                document.getElementById("checkCNAE").style.display = "block";
            } else {
                document.getElementById(`BoolCnaeIndustrial-${idElemento}`).value = "1";
                document.getElementById(`checkCNAE-${idElemento}`).style.color = "green";
                document.getElementById(`checkCNAE-${idElemento}`).classList.value = "fa fa-check";
                document.getElementById(`checkCNAE-${idElemento}`).style.display = "block";
            }

            possuiIndustrial = true;
    
        } else {
    
            if(idElemento == null){
                document.getElementById("BoolCnaeIndustrial").value = "0";
                document.getElementById("checkCNAE").style.color = "red";
                document.getElementById("checkCNAE").classList.value = "fa fa-close";
                document.getElementById("checkCNAE").style.display = "block";
            } else {
                document.getElementById(`BoolCnaeIndustrial-${idElemento}`).value = "0";
                document.getElementById(`checkCNAE-${idElemento}`).style.color = "red";
                document.getElementById(`checkCNAE-${idElemento}`).classList.value = "fa fa-close";
                document.getElementById(`checkCNAE-${idElemento}`).style.display = "block";
            }
        }
    });
}

function AplicarDadosAPI(idElemento) {
    let cnpj;
    if (idElemento == null) {
        cnpj = document.querySelector("#valor_cnpj").value;
    } else {
        cnpj = document.querySelector(`#valor_cnpj-${idElemento}`).value;
    }
    let url = window.location.origin + "/Empresas/DadosAPI?query=" + cnpj;
    
    fetch(url).then(res => {
        res.json().then(dados => {
            listaCNAE = [];
            listaCNAE.push(dados.atividade_principal[0].code);
            dados.atividades_secundarias.forEach(ativ => {listaCNAE.push(ativ.code);});
            if (idElemento == null) {
                document.getElementById("NomeEmpresaCadastro").value = dados.nome;
                document.getElementById("NomeFantasiaEmpresa").value = dados.fantasia;
                document.getElementById("TipoEmpresaStatus").innerHTML = "Tipo: " + dados.tipo;
                document.getElementById("SituacaoEmpresaStatus").innerHTML = "Situação: " + dados.situacao;
                checarCNAE(listaCNAE);
            } else {
                document.getElementById(`NomeEmpresaCadastro-${idElemento}`).value = dados.nome;
                document.getElementById(`NomeFantasiaEmpresa-${idElemento}`).value = dados.fantasia;
                document.getElementById(`TipoEmpresaStatus-${idElemento}`).innerHTML = "Tipo: " + dados.tipo;
                document.getElementById(`SituacaoEmpresaStatus-${idElemento}`).innerHTML = "Situação: " + dados.situacao;
                checarCNAE(listaCNAE, idElemento);
            }

            // TUDO DAQUI PRA BAIXO FOI FEITO EXCLUSIVAMENTE PARA CONVERTER A SIGLA DE CADA ESTADO PARA O NOME COMPLETO
            function Dicionario() {
                this.add = add;
                this.dataStore = [];
                this.find = find;
            }
            function add(key, value) { this.dataStore[key] = value; }
            function find(key) { return this.dataStore[key]; }

            var siglas = new Dicionario();

            siglas.add('RJ', 'Rio de Janeiro')
            siglas.add('SP', 'São Paulo')
            siglas.add('MG', 'Minas Gerais')
            siglas.add('ES', 'Espírito Santo')
            siglas.add('PR', 'Paraná')
            siglas.add('SC', 'Santa Catarina')
            siglas.add('RS', 'Rio Grande do Sul')
            siglas.add('MT', 'Mato Grosso')
            siglas.add('MS', 'Mato Grosso do Sul')
            siglas.add('GO', 'Goiás')
            siglas.add('DF', 'Distrito Federal')
            siglas.add('AM', 'Amazonas')
            siglas.add('PA', 'Pará')
            siglas.add('RR', 'Roraima')
            siglas.add('RO', 'Rondônia')
            siglas.add('MA', 'Maranhão')
            siglas.add('PI', 'Piauí')
            siglas.add('RN', 'Rio Grande do Norte')
            siglas.add('SE', 'Sergipe')
            siglas.add('PE', 'Pernambuco')
            siglas.add('PB', 'Paraíba')
            siglas.add('BA', 'Bahia')
            siglas.add('TO', 'Tocantins')
            siglas.add('AP', 'Amapá')
            siglas.add('CE', 'Ceará')
            siglas.add('AL', 'Alagoas')

            // ESSA LINHA BUSCA O ÍNDICE A PARTIR DA SIGLA DEVOLVIDA PELA API --------\/
            if (idElemento == null) {
                document.getElementById("EstadoEmpresaCadastro").value = siglas.find(dados.uf);
            } else {
                document.getElementById(`EstadoEmpresaCadastro-${idElemento}`).value = siglas.find(dados.uf);
            }



            // CONVERTER A SIGLA DE CADA ESTADO PARA O ÍNDICE DO ENUM
            function Dicionario2() {
                this.add = add;
                this.dataStore = [];
                this.find = find;
            }
            function add(key, value) { this.dataStore[key] = value; }
            function find(key) { return this.dataStore[key]; }

            var indices = new Dicionario2();

            indices.add('RJ', 0)
            indices.add('SP', 1)
            indices.add('MG', 2)
            indices.add('ES', 3)
            indices.add('PR', 4)
            indices.add('SC', 5)
            indices.add('RS', 6)
            indices.add('MT', 7)
            indices.add('MS', 8)
            indices.add('GO', 9)
            indices.add('DF', 10)
            indices.add('AM', 11)
            indices.add('PA', 12)
            indices.add('RR', 13)
            indices.add('RO', 14)
            indices.add('MA', 15)
            indices.add('PI', 16)
            indices.add('RN', 17)
            indices.add('SE', 18)
            indices.add('PE', 19)
            indices.add('PB', 20)
            indices.add('BA', 21)
            indices.add('TO', 22)
            indices.add('AP', 23)
            indices.add('CE', 24)
            indices.add('AL', 25)

            // ESSA LINHA BUSCA O NOME A PARTIR DA SIGLA DEVOLVIDA PELA API --------\/
            if (idElemento == null)
            {
                document.getElementById("EstadoEmpresaCadastroINT").value = indices.find(dados.uf);
            }
            else
            {
                document.getElementById(`EstadoEmpresaCadastroINT-${idElemento}`).value = indices.find(dados.uf);
            }

    })}) 
}

function travarBotao()
{
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