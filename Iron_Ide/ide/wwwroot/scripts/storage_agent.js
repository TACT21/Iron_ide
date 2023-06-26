function SessionStorageWrite(key, value) {
    if (value === "") {
        sessionStorage.removeItem(key);
    } else {
        console.log(value + "\n@storage_agent.js SessionStorageWrite");
        sessionStorage.setItem(key, String(value));
    }
}

function SessionStorageRead(key) {
    return sessionStorage.getItem(key);
}