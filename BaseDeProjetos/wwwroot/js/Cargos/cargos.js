let cargosCarregados = []

async function carregarModalCargo(idCargo, tipo) {
    if (idCargo) {
        if (tipo) {
            await fetch(`/Cargos/RetornarModal?idCargo=${idCargo}&tipo=${tipo}`)
                .then(response => response.text())
                .then(result => {
                    let modalContainer = document.querySelector(`#modal${tipo}Cargo-${idCargo}-container`);
                    if (modalContainer) {
                        modalContainer.innerHTML = result;
                    }
                    else {
                        console.error("Não temos o container para o modal");
                    }
                })
                .catch(err => {
                    console.error(`Ocorreu um erro ao puxar o modal: ${err}`);
                });
        } else {
            console.error("Tipo de Modal Inválido");
        }
    }
    else {
        console.error(`idCargo inválida: ${idCargo}`);
    }
}

async function carregarModaisCargo(idCargo) {
    // O nome precisa bater com o nome da view, no sentido que, o sufixo será "CargoViewComponent". Em outras palavras, está acoplado.
    let modaisDisponiveis = ["Edit", "Delete"]

    const buttonName = `#button-${idCargo}`;
    const buttonElem = document.querySelector(buttonName);

    if (buttonElem) {
        try {
            if (!cargosCarregados.includes(idCargo)) {
                buttonElem.disabled = true;
                buttonElem.innerHTML = '<div class="spinner-border spinner-border-sm text-primary" role="status"><span class="visually-hidden">Loading...</span></div>';

                await Promise.all(modaisDisponiveis.map(tipo => carregarModalCargo(idCargo, tipo)));

                modaisDisponiveis.forEach(async (tipo) => {
                    await carregarModalCargo(idCargo, tipo)
                })

                // Execute after the carregarModalCargo is done (doesn't work)
                cargosCarregados.push(idCargo);

                buttonElem.disabled = false;
                buttonElem.innerHTML = '<i class="bi bi-gear text-primary fs-5"></i>';
            }
        } catch (error) {
            console.error(`Houve um erro ao carregar os cargos: ${error}`)
        }
    }
    else {
        console.error(`Não temos um elemento botão válido`);
    }
}
