// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { dotnet } from './dotnet.js'

var consoleInput = "";
var orderContent = "";

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
            AddConsole(orderContent + e);
        },
        getScript: AceGetValue();
    }
});


const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);
const text = exports.MyClass.Greeting();
console.log(text);

//document.getElementById('out').innerHTML = text;
document.getElementById('prompt').style.display = "none";

await dotnet.run();

function GetInput() {
    var result = consoleInput;
    consoleInput = "";
    return result;
}

function InputSet() {
    document.getElementById('prompt').style.display = "none";
    orderContent = document.getElementById('order').innerText;
    consoleInput = document.getElementById("consoleInput").value
}

function AddConsole(e) {
    document.getElementById('console').innerHTML += (e + "<br/>")
}

function Asq(e) {
    document.getElementById('prompt').style.display = "block";
    document.getElementById('order').innerHTML = e;
}