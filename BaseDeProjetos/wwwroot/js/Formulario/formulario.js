class Perguntas {
    constructor(tituloPergunta, tipo, obrigatorio) {
        this.tituloPergunta = tituloPergunta
        this.tipo = tipo
        this.obrigatorio = obrigatorio
    }
}

let listaPerguntas = []

function CriarPergunta(tipo, pergunta, obrigatorio) {
    let perguntas = new Perguntas(tipo, pergunta, obrigatorio)
}

function AdicionarPergunta(pergunta) {

    listaPerguntas.push(pergunta)
}

function SerializarPerguntas(perguntas) {
    Json.stringify(perguntas)
}


