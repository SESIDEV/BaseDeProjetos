

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
    var baseUrl = "https://localhost:44352" + path;
    var params = "?ano=";
    var select = document.getElementById('ano').value;

    if (location.href == baseUrl) {
        location.href = baseUrl + params + select;
    }
    else {
        location.href = baseUrl + params + select;
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

function mostreAno(){    

    var ano = getSelected();

    document.querySelector('#ano').value = ano;
}
mostreAno();







