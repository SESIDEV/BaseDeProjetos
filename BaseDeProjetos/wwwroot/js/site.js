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

function FiltroEmpresaEstrangeira(){

    let checkBox = document.getElementById("empresa_estrangeira_check");
    let cnpj = document.getElementById("valor_cnpj");
    let campo1 = document.getElementById("cadastro_campo1");
    let campo2 = document.getElementById("cadastro_campo2");
    let nome = document.getElementById("NomeEmpresaCadastro");
    let estado_int = document.getElementById("EstadoEmpresaCadastroINT");
    
  
    if (checkBox.checked == true){
    
        cnpj.value = 00000000000000;
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
    campoComp = document.querySelector("#filt").cloneNode(true)
    elemComp = campoComp.children
    elemComp[0].remove() // retirando a tag <p>
    listaComp = []
    for (let item of elemComp) {
        listaComp.push(item.innerHTML)
    }
    fetch('/Pessoas/dados').then(response => response.json()).then(data => {montarNetwork(data, listaComp)});
    if (element.classList[1] == 'bg-secondary'){
        element.classList.replace('bg-secondary','bg-primary')
        document.querySelector("#filt").appendChild(element)
    } else {
        element.classList.replace('bg-primary','bg-secondary')
        document.querySelector("#notfilt").appendChild(element)
    }
}

function gerarOpcoesSelectPessoas(nomeModal, idSelect) {
    document.querySelector(`#${idSelect}`).innerHTML = '';
    if (nomeModal == null){
        $(`#${idSelect}`).select2()
    } else {
        $(`#${idSelect}`).select2({dropdownParent: $(`#${nomeModal}`)})
    }
    fetch('/FunilDeVendas/PuxarDadosUsuarios').then(response => response.json()).then(lista => {
        lista.forEach(function (item){
            var opt = document.createElement("option");
            opt.value = item['Email']
            opt.innerHTML = item['UserName']
            document.querySelector(`#${idSelect}`).appendChild(opt)
    })})
}

function gerarOpcoesSelectTags(nomeModal, idSelect) {
    document.querySelector(`#${idSelect}`).innerHTML = '';
    if (nomeModal == null){
        $(`#${idSelect}`).select2()
    } else {
        $(`#${idSelect}`).select2({dropdownParent: $(`#${nomeModal}`)})
    }
    fetch('/FunilDeVendas/PuxarTagsProspecoes').then(response => response.json()).then(lista => {
        lista.forEach(function (item){
            var opt = document.createElement("option");
            opt.innerHTML = item
            document.querySelector(`#${idSelect}`).appendChild(opt)
    })})
}
function novaTag(){
    valor = document.querySelector(`.select2-search__field`).value
    document.querySelector(`.select2-search__field`).valorSalvo = valor
}

function addTag(campoInput, idSelect){
    valor = document.querySelector(`.select2-search__field`).valorSalvo
    valorAntigo = document.querySelector(`#${campoInput}`).value

    if(valorAntigo == ''){
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
    redePessoas.focus(select.value, {scale:3, animation:{duration: 400}})
}

function selectToText(idSelect, idText) {
    let texto = ''
    document.querySelector(`#select2-${idSelect}-container`).childNodes.forEach(p => texto += p.title + ';')
    document.querySelector(`#${idText}`).value = texto
}

function textToSelect(nomeModal, idText, idSelect) {
    gerarOpcoesSelectPessoas(nomeModal, idSelect)

    botao_def = document.querySelector('#bloco_botao')
    listaSel = document.querySelector(`#${idText}`).value.split(";")
    listaSel.forEach(p => {
        if (p == ''){} else {
        botao_copy = botao_def.cloneNode(true)
        botao_copy.removeAttribute('id')
        botao_copy.title = p
        botao_copy.children[1].innerHTML = p
        botao_copy.classList.remove('d-none')

        document.querySelector(`#select2-${idSelect}-container`).appendChild(botao_copy)
    }})
}

function listarCompetencias(){
    fetch('/Pessoas/dictCompetencias').then((response) => response.json()).then((json) => {
        listaComp = Object.values(json)
        badge_def_sel = document.querySelector('#badge_competencia_select')
        listaComp.forEach(c => {
            if (c == '' || c == null){} else {
            badge_copy = badge_def_sel.cloneNode(true)
            badge_copy.removeAttribute('id')
            badge_copy.innerHTML = c
            badge_copy.classList.remove('d-none')

            document.querySelector(`#notfilt`).appendChild(badge_copy)
    }})})
}

function mostrarCompetencias() {
    fetch('/Pessoas/dictCompetencias').then((response) => response.json()).then((json) => {
        document.querySelector(`#campo_competencias`).innerHTML = ""
        badge_def = document.querySelector('#badge_competencia')
        listaComp = document.querySelector(`#competencias`).innerHTML.split(";")
        listaComp.forEach(num => {
            if (num == '' || num == null){} else {
            badge_copy = badge_def.cloneNode(true)
            badge_copy.removeAttribute('id')
            badge_copy.innerHTML = json[num]
            badge_copy.classList.remove('d-none')

            document.querySelector(`#campo_competencias`).appendChild(badge_copy)
    }})})
}

function checarStatusNaoConvertida(select){
    if (select.value == 7){
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

function montarNetwork(pessoas, competencias=null) {
	var nodes = null;
	var edges = null;
	var network = null;
    let defuserpic = "https://cdn.icon-icons.com/icons2/1378/PNG/512/avatardefault_92824.png";
    var existeFiltro = false
    var listaPessoas = []

    if (competencias != null){
        compFiltradas = document.querySelector(`#${competencias}`).split(";")
        existeFiltro = NaoEhNulaOuVazia(compFiltradas)
    }

    pessoas.forEach(p => {
        let listaCompPessoa = null
        let notInclude = false
        var obj = {};

        if (existeFiltro){
            listaCompPessoa = p['Competencia'].split(";")

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

        obj['id'] = p['Email']
        obj['title'] = p['UserName']
        obj['image'] = p['Foto']
        obj['comp'] = p['Competencia']
        
        if ((p['Foto'] == null) || (p['Foto'] == "")){
            obj['image'] = defuserpic
        }

        listaPessoas.push(obj)
    })

	nodes = new vis.DataSet(listaPessoas);//lista de pessoas
	 
	edges = new vis.DataSet([
	  { from: 1, to: 2 },
	  { from: 2, to: 3 },
	]);
  
	// create a network
	var container = document.getElementById("chart-line");
    
	var data = {
	  nodes: nodes,
	  edges: edges,
	};

	var options = {
		nodes: {
            shape: "circularImage",
			borderWidth: 5,
			size: 30,
			color: {
				border: "#229954",
				background: "#666666",
			},
			font: {color: "#17202A"},
        },
		edges: {
			color: "#D0D3D4",
			value: 5,
			shadow: true,
		},
		interaction:{
			dragNodes: true,
			dragView: false,
			zoomView: false,
		},
	}
	network = new vis.Network(container, data, options)

	network.on('click', function(selecao) {
		if (selecao.nodes.length != 0){
			document.getElementById('user_card').style.display = 'block'
			var clickedNode = nodes.get(selecao.nodes)[0];
			network.focus(clickedNode.id,{scale:2, animation:{duration: 400}})
			document.getElementById("imagem-do-card").src = clickedNode.image
            document.getElementById("nome-do-card").innerHTML = clickedNode.title
            document.getElementById("competencias").innerHTML = clickedNode.comp
            mostrarCompetencias()
		} else {
			//network.fit({animation:{duration: 400}})
			network.moveTo({position:{x:0,y:0}, scale:2,animation:{duration: 400}})
			document.getElementById("user_card").style.display = 'none'
		}
	});
	
	network.once('stabilized', function() {
		network.moveTo({position:{x:0,y:0}, scale:2})
	});

    redePessoas = network;
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

function validarCNPJ(){
    document.getElementById("valor_cnpj").value = document.getElementById("valor_cnpj").value.replace(/[^0-9]/g, '');
    let cnpj = document.getElementById("valor_cnpj").value;
    if (isNaN(cnpj) || cnpj.length < 14) {
        alert("CNPJ inválido");
    } else {
        AplicarDadosAPI();
    }    
}

function AplicarDadosAPI() {

    let cnpj = document.querySelector("#valor_cnpj").value;
    let url = window.location.origin + "/Empresas/DadosAPI?query=" + cnpj;
    
    fetch(url).then(res => {

    res.json().then(dados => {
    document.getElementById("NomeEmpresaCadastro").value = dados.nome;
    document.getElementById("NomeFantasiaEmpresa").value = dados.fantasia;
    document.getElementById("TipoEmpresaStatus").innerHTML = "Tipo: " + dados.tipo;
    document.getElementById("SituacaoEmpresaStatus").innerHTML = "Situação: " + dados.situacao;

    // TUDO DAQUI PRA BAIXO FOI FEITO EXCLUSIVAMENTE PARA CONVERTER A SIGLA DE CADA ESTADO PARA O NOME COMPLETO
    function Dicionario(){
        this.add = add;
        this.dataStore = [];
        this.find = find;
    }
    function add(key, value){this.dataStore[key] = value;}
    function find(key){return this.dataStore[key];}

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
    document.getElementById("EstadoEmpresaCadastro").value = siglas.find(dados.uf);



    // CONVERTER A SIGLA DE CADA ESTADO PARA O ÍNDICE DO ENUM
    function Dicionario2(){
        this.add = add;
        this.dataStore = [];
        this.find = find;
    }
    function add(key, value){this.dataStore[key] = value;}
    function find(key){return this.dataStore[key];}

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
    document.getElementById("EstadoEmpresaCadastroINT").value = indices.find(dados.uf);

    })}) 
}
