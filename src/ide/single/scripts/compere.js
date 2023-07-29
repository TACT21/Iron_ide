var inputs = [];
var answer
function Compere(){
    if(localStorageRead("result")){
        answer = localStorageRead("result");
        exameeDoTask();
        return;
    }
    const ConsoleWrap = document.getElementById("prompt");
    if(!(ConsoleWrap)){
        alert("実行環境展開場所が存在しません。再読み込みしてください。")
        return;
    }
    Array.prototype.forEach.call(ConsoleWrap.getElementsByTagName("ifream"), function(item) {
        item.remove();
    });
    document.getElementById("EngineStoper").classList.add("hide");
    var fream = document.getElementById("promptfream");
    if (fream) {
        fream.remove()
    }
    var json = JSON.stringify(localStorageRead("test"));
    if(json.inputs){
        inputs = json.inputs;
    }
    ifream = freamCaller(json.src);
    testerInputManager = function (e) {
        switch (e.data.action) {
            case 'asq':
                ifream.contentWindow.postMessage({
                    action: 'GiveInput',
                    message: window.IronIde.Inputs[testerCount]
                }, '*',);
                testerCount = testerCount + 1;
                break;
            case 'print':
                if(e.data.message === "<p style = \"color:#7df0a3\">All Tasks is Compreate.</p>")
                {
                    exameeDoTask()
                }else{
                    answer = e.data.message;
                }
        }
    }
    window.addEventListener('message', testerInputManager);
}

function exameeDoTask(){
    if(testerInputManager){
        window.removeEventListener('message', testerInputManager);
        document.getElementById("promptfream").remove();
    }
    ifream = freamCaller(window.IronIde.getValue());
    testerInputManager = function (e) {
        switch (e.data.action) {
            case 'asq':
                ifream.contentWindow.postMessage({
                    action: 'GiveInput',
                    message: window.IronIde.Inputs[testerCount]
                }, '*',);
                prints.push(e.data.message);
                prints.push(window.IronIde.Inputs[testerCount]);
                testerCount = testerCount + 1;
                break;
            case 'print':
                prints.push(e.data.message);
                if(e.data.message === "<p style = \"color:#7df0a3\">All Tasks is Compreate.</p>")
                {
                    answer = prints[prints.length - 2];
                    document.getElementById("EngineStoper").classList.remove("hide");
                    document.getElementById("EngineStoper").click();
                }
        }
    }
    window.addEventListener('message', testerInputManager);
}

var s = "";

function freamCaller(script){
    var elem = document.getElementById("prompt");
    var iframe = document.createElement('iframe');
    iframe.id = "promptfream";
    iframe.src = document.location.origin + "/console.html"
    iframe.style.width = "100%";
    iframe.style.height = "calc(300px - 2rem)";
    iframe.style.position = "absolute";
    iframe.style.bottom = "0";
    iframe.classList.add("curtain");
    var result = elem.appendChild(iframe);
    elem.classList.remove("hide");
    s = script;
    window.addEventListener('message', function (e) {
        switch (e.data.action) {
            case 'ReqScript':
                console.log("post script")
                iframe.contentWindow.postMessage({
                    action: 'GiveScript',
                    message: window.IronIde.getValue()
                }, '*',);
            case "ConsoleWrite":
                console.log(e.message)
        }
    });
    return result;
}
if(!(window.IronIde)){
    window.IronIde ={}
}

window.IronIde.compere = function(){
    Compere()
};

console.log("The module named Compere is loaded");