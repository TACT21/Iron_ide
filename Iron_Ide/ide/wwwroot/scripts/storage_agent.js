function SessionStorageWrite(key, value) {
    if (value === "") {
        sessionStorage.removeItem(key);
    } else {
        console.log(value + "\n@storage_agent.js SessionStorageWrite");
        sessionStorage.setItem(key, String(value));
    }
}

function SessionStorageRead(key) {
    console.log(sessionStorage.getItem(key) + "\n@storage_agent.js SessionStorageRead");
    return sessionStorage.getItem(key);
}