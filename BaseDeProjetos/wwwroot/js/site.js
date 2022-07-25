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

function validarCNPJ(){
    
    let cnpj = document.getElementById("valor_cnpj").value;
    if (isNaN(cnpj) || cnpj.length < 14) {
        alert("CNPJ inválido");
    } else {
        AplicarDadosAPI();
    }    
}

function CorStatusGC(lista_status){    
    Array.from(lista_status).forEach((status) => 
    {
        switch(status.outerText) {
            case 'Recebido': status.className = "btn btn-secondary status_gc"; break;
            case 'Aceito': status.className = "btn btn-info status_gc"; break;
            case 'Publicado': status.className = "btn btn-success status_gc"; break;
        }
    })
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