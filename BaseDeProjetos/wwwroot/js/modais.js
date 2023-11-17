let modalCarregando = false;

function carregarModalCFF(event, idCFF) {
    if (modalCarregando) {
        return; // Exit function if it's already executing
    }

    modalCarregando = true; // Set the flag to true to indicate the function is executing

    let sourceElement = event.target;
    let modalCFFcontainer = document.querySelector(`#modalEditCFFProjeto-${idCFF}-container`);

    if (modalCFFcontainer != null) {
        if ((modalCFFcontainer.innerHTML != null || modalCFFcontainer.innerHTML != undefined) && modalCFFcontainer.innerHTML.trim() === '') {
            sourceElement.disabled = true;
            let previousInner = sourceElement.innerHTML;
            sourceElement.innerHTML = '<div class="spinner-border text-light"></div>';
            fetch(`/Projetos/RetornarModalEditCFF?idCFF=${idCFF}`)
                .then(response => response.text())
                .then(result => {
                    sourceElement.innerHTML = previousInner;
                    modalCFFcontainer.innerHTML = result;
                    sourceElement.disabled = false;
                    console.log(modalCFFcontainer);
                })
                .finally(() => {
                    modalCarregando = false; // Reset the flag to false after function has executed
                });
        } else {
            modalCarregando = false; // Reset the flag if no fetch call is made
        }
    }
    else {
        console.error("ModalCFFContainer is null for idCFF: ", idCFF);
    }

}

function carregarModalRubrica(event, idRubrica) {
    if (modalCarregando) {
        return; // Exit function if it's already executing
    }

    modalCarregando = true; // Set the flag to true to indicate the function is executing

    let sourceElement = event.target;
    let modalRubricasContainer = document.querySelector(`#modalEditRubricasProjeto-${idRubrica}-container`);

    if ((modalRubricasContainer.innerHTML != null || modalRubricasContainer.innerHTML != undefined) && modalRubricasContainer.innerHTML.trim() === '') {
        sourceElement.disabled = true;
        let previousInner = sourceElement.innerHTML;
        sourceElement.innerHTML = '<div class="spinner-border text-light"></div>';
        fetch(`/Projetos/RetornarModalEditRubricas?idRubrica=${idRubrica}`)
            .then(response => response.text())
            .then(result => {
                sourceElement.innerHTML = previousInner;
                modalRubricasContainer.innerHTML = result;
                sourceElement.disabled = false;
                console.log(modalRubricasContainer);
            })
            .finally(() => {
                modalCarregando = false; // Reset the flag to false after function has executed
            });
    } else {
        modalCarregando = false; // Reset the flag if no fetch call is made
    }
}
