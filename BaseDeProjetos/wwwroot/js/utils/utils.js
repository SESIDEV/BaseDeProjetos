function validarInput(element) {
    element.classList.remove("is-invalid");
    element.classList.add("is-valid");
}

function invalidarInput(element) {
    element.classList.remove("is-valid");
    element.classList.add("is-invalid");
}

function verificarInput(resultadoValidacao, element) {
    if (resultadoValidacao === true) {
        validarInput(element);
    }
    else {
        invalidarInput(element);
    }
}


function adicionarListenerVerificacao(elemento, fnValidacao) {
    console.log(`Adicionando listener para ${elemento.id} tendo em conta função de validação ${fnValidacao}`);
    if (elemento) {
        const verificacao = () => verificarCampo(elemento, fnValidacao);
        elemento.removeEventListener("change", verificacao);
        elemento.removeEventListener("click", verificacao);
        elemento.addEventListener("change", verificacao);
        elemento.addEventListener("click", verificacao);
    }
    else {
        console.error(`Não temos o elemento HTML: ${elemento}`);
    }
}

function adicionarListenerVerificacaoEmLista(listaElementos, fnValidacao) {
    listaElementos.forEach(elemento => {
        adicionarListenerVerificacao(elemento, fnValidacao);
    })
}

function verificarCampo(elemento, fnValidacao) {
    verificarInput(fnValidacao(elemento), elemento);
}