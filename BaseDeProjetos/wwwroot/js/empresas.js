function validarCNPJ(idElemento = "") {
    document.getElementById(`valor_cnpj${idElemento}`).value = document.getElementById(`valor_cnpj${idElemento}`).value.replace(/[^0-9]/g, '');
    let cnpj = document.getElementById(`valor_cnpj${idElemento}`).value;

    if (isNaN(cnpj) || cnpj.length < 14) {
        alert("CNPJ inválido");
    } else {
        aplicarDadosAPI(idElemento);
    }
}

function aplicarDadosAPI(idElemento) {
    let cnpj = document.querySelector(`#valor_cnpj${idElemento}`).value;
    let url = window.location.origin + "/Empresas/DadosAPI?query=" + cnpj;

    fetch(url).then(res => {
        res.json().then(dados => {
            const estadoEmpresaCadastroINT = document.getElementById(`EstadoEmpresaCadastroINT${idElemento}`);
            const estadoEmpresaCadastro = document.getElementById(`EstadoEmpresaCadastro${idElemento}`);

            listaCNAE = [];
            listaCNAE.push(dados.atividade_principal[0].code);
            dados.atividades_secundarias.forEach(ativ => {
                listaCNAE.push(ativ.code);
            });

            document.getElementById(`RazaoSocialEmpresaCadastro${idElemento}`).value = dados.nome;
            document.getElementById(`NomeEmpresa${idElemento}`).value = dados.fantasia;
            document.getElementById(`TipoEmpresaStatus${idElemento}`).innerHTML = "Tipo: " + dados.tipo;
            document.getElementById(`SituacaoEmpresaStatus${idElemento}`).innerHTML = "Situação: " + dados.situacao;

            checarCNAE(listaCNAE, idElemento);

            // Tudo daqui pra baixo foi feito exclusivamente para converter a sigla de cada estado para o nome completo
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

            // Essa linha busca o índice a partir da sigla devolvida pela api --------\/
            let siglaEstado = siglas.find(dados.uf);

            if (siglaEstado) {
                estadoEmpresaCadastro.value = siglaEstado;
            }
            else {
                estadoEmpresaCadastro.value = "SemCadastro";
            }

            // Converter a sigla de cada estado para o índice do enum
            var indices = new Dicionario();

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

            // Essa linha busca o nome a partir da sigla devolvida pela api --------\/
            let nomeEstado = indices.find(dados.uf);

            if (nomeEstado) {
                estadoEmpresaCadastroINT.value = nomeEstado;
            } else {
                estadoEmpresaCadastroINT.value = "SemCadastro";
            }
        })
    })
}

function checarCNAE(listaCNAE, idElemento = "") {
    const cnaeIndustrial = listaCNAE.some(codCNAE => typeof dictCNAE[codCNAE.slice(0, 2)] !== "undefined");
    const boolCnaeIndustrial = cnaeIndustrial ? "True" : "False";
    const checkCNAEelement = document.getElementById(`checkCNAE${idElemento}`);

    document.getElementById(`BoolCnaeIndustrial${idElemento}`).value = boolCnaeIndustrial;
    checkCNAEelement.style.color = cnaeIndustrial ? "green" : "red";
    checkCNAEelement.classList.value = cnaeIndustrial ? "fa fa-check" : "fa fa-close";
    checkCNAEelement.style.display = "block";
}


function FiltroEmpresaEstrangeira() {
    let checkBox = document.getElementById("empresa_estrangeira_check");
    let cnpj = document.getElementById("valor_cnpj");
    let campo1 = document.getElementById("cadastro_campo1");
    let campo2 = document.getElementById("cadastro_campo2");
    let nome = document.getElementById("RazaoSocialEmpresaCadastro");
    let estado_int = document.getElementById("EstadoEmpresaCadastroINT");

    if (checkBox.checked == true) {
        cnpj.value = "00000000000000";
        campo1.style.display = "none"
        estado_int.value = 26;
        campo2.style.display = "none"
        nome.style.display = "none"
    } else {
        cnpj.value = "";
        campo1.style.display = "block"
        estado_int.value = 0;
        campo2.style.display = "block"
        nome.style.display = "block"
    }
}