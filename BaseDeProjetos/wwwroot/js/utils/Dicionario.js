class Dicionario {
	constructor() {
		this.dataStore = [];
	}

	add(key, value) {
		this.dataStore[key] = value;
	}

	find(key) {
		return this.dataStore[key];
	}
}

export default Dicionario;