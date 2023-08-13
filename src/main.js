// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { dotnet } from './dotnet.js'

var script = "";
var receve = false;
var test = false;
const limit = 5;

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

const params = new URLSearchParams(window.location.search.substring(1));
if(params.get("test")){
    test = true;
    window.addEventListener('message', function (e) {
        switch (e.data.action) {
            case 'GiveInput':
                document.getElementById('input_result').innerText = e.data.message;
                break;
        }
    });
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

await DotNetRun(limit)

console.log("done");

async function DotNetRun(token){
    if(token > 0){
        await dotnet.run().catch(async ()=> {console.error(e); await DotNetRun(token - 1)}) 
    }else{
        return null;
    }
}

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
    window.parent.postMessage({
        action: 'print',
        message: e
    }, '*',);
}

function Asq(e) {
    if(test){        
        window.parent.postMessage({
            action: 'asq',
            message: e
        }, '*',);
    }else{
        document.getElementById('input').style.display = "block";
        document.getElementById('order').innerHTML = e;
    }
}

function sleep(milliSeconds) {
    return new Promise((resolve) => {
        setTimeout(() => resolve(), milliSeconds);
    });
}

addEventListener("unhandledrejection", (event) => {
    console.log(event.reason.message);
    var criterion = "Uncaught TypeError: Failed to execute 'decode' on 'TextDecoder': The provided ArrayBufferView value must not be shared.";
    if(event.reason.message.indexOf(criterion) != -1){
        location.reload();
    }
});

addEventListener("error", (event) => {
    console.log(event.message);
    var criterion = "Uncaught TypeError: Failed to execute 'decode' on 'TextDecoder': The provided ArrayBufferView value must not be shared.";
    if(event.message.indexOf(criterion) != -1){
        location.reload();
    }
});