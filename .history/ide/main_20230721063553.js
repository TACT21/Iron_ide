// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { dotnet } from './dotnet.js'

var script = "";
var receve = false;

window.addEventListener('message', function (e) {
    switch (e.data.action) {
        case 'GiveScript':
            script = e.data.message;
            receve = true;
            break;
    }
});
console.log("Request Script")

window.parent.postMessage({
    action: 'ReqScript',
    message: ''
}, '*',);

while (true) {
    await sleep(1000);
    if (receve) {
        AddConsole("Script has been posted");
        break;
    }
}


const { setModuleImports, getAssemblyExports, getConfig } = await dotnet
    .withDiagnosticTracing(false)
    .withApplicationArgumentsFromQuery()
    .create();

setModuleImports('main.js', {
    window: {
        location: {
            href: () => globalThis.window.location.href
        }
    },
    ironPython: {
        getInput: () => GetInput(),
        addConsole: (e) => AddConsole(e),
        askQuestion: (e) => Asq(e),
        clearQuestion: (e) => function (e) {
            AddConsole(document.getElementById('asq_result').innerText +" "+ e);
            document.getElementById('asq_result').innerText = "";
        },
        getScript: () => { return script; }
    }
});


const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);
const text = exports.MyClass.Greeting();
console.log(text);
exports.MyClass.Ignition();

//document.getElementById('out').innerHTML = text;
document.getElementById('input').style.display = "none";

function Run() {
    exports.MyClass.Ignition();
}

await dotnet.run();

function GetInput() {
    var result = document.getElementById('input_result').innerText;
    document.getElementById('input_result').innerText = "";
    if (result) {
        console.log(result)
    } else {
        console.log("Null")
    }
    return result;
}


function AddConsole(e) {
    document.getElementById('console').innerHTML += (e + "<br/>")
}

function Asq(e) {
    document.getElementById('input').style.display = "block";
    document.getElementById('order').innerHTML = e;
}

function sleep(milliSeconds) {
    return new Promise((resolve) => {
        setTimeout(() => resolve(), milliSeconds);
    });
}