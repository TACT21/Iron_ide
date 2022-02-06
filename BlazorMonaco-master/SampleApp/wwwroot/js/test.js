function syncDelay(milliseconds) {
    var start = new Date().getTime();
    var end = 0;
    while ((end - start) < milliseconds) {
        end = new Date().getTime();
    }
}

function GetInput(quest, timeout = 60) {
    for (var i = 0; i < timeout; i++) {
        DotNet.invokeMethodAsync('SampleApp', 'Clearinput').then(data => {
            if (data != "") {
                return deta
            }
        });
        syncsleep(1).then{
            console.log("NULL")
        }
    }
    return "Null"
}

function sleep(sec) {
    return new Promise((resolve, reject) => {
        setTimeout(() => {
            console.log(`wait: ${sec} sec`);
            resolve(sec);
        }, sec * 1000);
    });
}