function wait(time) {
    return new Promise((resolve, reject) => {
        setTimeout(() => {
            resolve(`wait: ${time}`);
        }, time);
    });
}

function Get() {
    DotNet.invokeMethodAsync('Ide.Components', 'GetCmd').then(data => {
        if (deta != "") {
            return Promise.resolve(data)
        } else {
            return wait(1000).then((value) => {
                return Get();
            })
        }
    });
}


function Input() {
    Get().then(data => {
        if (data != "") {
            return data
        } else {
            return "NULL"
        }
    });
    return "NULL"
}