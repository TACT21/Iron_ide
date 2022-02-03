function syncDelay(milliseconds) {
    var start = new Date().getTime();
    var end = 0;
    while ((end - start) < milliseconds) {
        end = new Date().getTime();
    }
}

function GetInput(strimg) {
    console.log("Js")
    syncDelay(1000)
    return "This is test"
}
