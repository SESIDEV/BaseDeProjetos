

class DragAndDrop {

    constructor() {
        this.valor = 10;
    }

    allowDrop(ev) {
        if (ev.target.getAttribute("droppable") == "false") {
            ev.dataTransfer.dropEffect = "none"; // dropping is not allowed
            ev.preventDefault();
        }
        else {
            ev.dataTransfer.dropEffect = "all"; // drop it like it's hot
            ev.preventDefault();
        }
    }

    drag(ev) {
        ev.dataTransfer.setData("text", ev.target.id);
        ev.target.setAttribute("droppable", "false")
    }

    drop(ev) {
        ev.preventDefault();
        let data = ev.dataTransfer.getData("text");
        ev.target.appendChild(document.getElementById(data));
    }
}

