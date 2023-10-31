function formatarValoresDashboard(valor) {
    if (valor) {
        if (valor >= 100000000) {
            return `${(valor / 1000000).toFixed(1)} MI`;
        }
        if (valor >= 1000000) {
            return `${(valor / 1000000).toFixed(2)} MI`;
        }
        if (valor >= 100000) {
            return `${(valor / 1000).toFixed(1)} mil`;
        }
        if (valor >= 10000) {
            return `${(valor / 1000).toFixed(2)} mil`;
        }
    }
    else {
        return 0;
    }

    return valor.toLocaleString(undefined, { minimumIntegerDigits: 1, minimumFractionDigits: 0 });
}  