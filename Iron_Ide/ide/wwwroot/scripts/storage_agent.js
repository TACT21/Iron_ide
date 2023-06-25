function SessionStorageWrite(key, value) {
    if (value = "") {
        sessionStorage.removeItem(key);
    } else {
        sessionStorage.setItem(key, value);
    }
}

function SessionStorageRead(key) {
    return sessionStorage.getItem(key);
}