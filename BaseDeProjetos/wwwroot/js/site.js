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

function updateLink() {
    var path = location.pathname;
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

function validarCNPJ(lista){
    
    // RECEBER DO C SHARP LISTA COM TODOS OS CNPJotas DO MODEL EMPRESAS #########################################
    let cnpj = document.getElementById("valor_cnpj").value;
//    if (!lista.includes(cnpj)){
        
        if (isNaN(cnpj) || cnpj.length < 14) {
            alert("CNPJ inválido");
        } else {
            AplicarDadosAPI();
        }
//    } else {
//        alert("CNPJ já cadastrado");
//    }
    
    
}


function AplicarDadosAPI() {

    let cnpj = document.querySelector("#valor_cnpj").value;
    let url = window.location.origin + "/Empresas/DadosAPI?query=" + cnpj;
    
    fetch(url).then(res => {

    res.json().then(dados => {
    document.getElementById("NomeEmpresaCadastro").value = dados.nome;
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

    // ESSA LINHA BUSCA O NOME A PARTIR DA SIGLA DEVOLVIDA PELA API --------\/
    document.getElementById("EstadoEmpresaCadastro").value = siglas.find(dados.uf);

    })}) 
}

function Base64() {

    imagem = document.getElementById("logo_imagem").files[0];
    r = new FileReader();
    r.readAsDataURL(imagem);
    r.onload = function(){
        document.getElementById('logo_b64').value = r.result;
        document.getElementById("logo_img_preview").src = r.result;
    }

}

function getSelected() {
    var year = location.href.split('?')[1].split('&');

    for (let i = 0; i < year.length; i++) {
        if (year[i].includes('ano')) {
            var ano = year[i].split('=')[1];
            return ano;
        }
        else {
            return document.getElementById('ano').value;
        }
    }
}
getSelected();

function mostreAno() {
    var ano = getSelected();

    document.querySelector('#ano').value = ano;
}
mostreAno();