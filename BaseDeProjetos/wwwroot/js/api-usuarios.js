class ObterListaDeUsuariosJson {

    constructor() {
        this.listaUsuarios = [];
        this.campoDeUsuariosDisponiveis = document.querySelector("#membrosDisponiveis")
        this.membrosSelecionados = document.querySelector("#membrosSelecionados")
        this.membrosDoProjeto = document.querySelector("#membrosDoProjeto").childNodes
        this.listaMembrosSelecionados = document.querySelector("#membrosSelecionados").childNodes
        this.input = document.querySelector("#MembrosEquipe")
        this.url = `${window.location.origin}/Projetos/ListaUsuarios`

        this.requestUrl();
    }

    async requestUrl() {
        await fetch(this.url)
            .then((response) => {

                return response.json()
            })
            .then((data) => {

                this.listaUsuarios = data;
                this.listaUsuarios.forEach(usuario => {
                    let usuarios = this.criarElemento(usuario);
                    this.campoDeUsuariosDisponiveis.append(usuarios)
                })
            })

    }

    criarElemento(valorDoElemento) {
        let elemento = document.createElement("p")
        elemento.setAttribute("draggable", "true")
        elemento.setAttribute("ondragstart", "dnd.drag(event)")
        elemento.setAttribute("onclick", "criarListaDeUSuarios.removerMembroSelecionado(this)")
        elemento.setAttribute("id", `${valorDoElemento}`)
        elemento.textContent = `${valorDoElemento}`
        elemento.classList.add("badge", "text-bg-success")
        elemento.style.padding = "5px"
        elemento.style.margin = "10px"
        return elemento;
    }

    enviarMembrosParaInput() {
        let membroSelecionado = "";
        this.listaMembrosSelecionados.forEach((membro, indice) => {
            if (indice > 0) {
                membroSelecionado += `${membro.textContent};`.toString()
                this.input.value = membroSelecionado
            }
        })
    }

    exibirMembrosSelecionados() {
        this.membrosDoProjeto.forEach((membro) => {
            this.membrosSelecionados.append(membro)
        })
    }

    removerMembroSelecionado(elemento) {
        let idElemento = elemento.getAttribute("id")
        let pai = document.getElementById(idElemento).parentElement;
        if (pai.getAttribute("id") == "membrosSelecionados") {
            if (confirm("Deseja remover este usuário?") == true) {
                this.campoDeUsuariosDisponiveis.append(elemento)
            }
            
        }
        
    }
}