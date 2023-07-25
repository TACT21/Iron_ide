function EngineCall() {
    var fream = document.getElementById("promptfream");
    if (fream) {
        fream.remove()
    }
    var elem = document.getElementById("prompt");
    var iframe = document.createElement('iframe');
    iframe.id = "promptfream";
    iframe.src = "../../console.html"
    iframe.style.width = "100%";
    iframe.style.height = "calc(300px - 2rem)";
    iframe.style.position = "absolute";
    iframe.style.bottom = "0";
    elem.appendChild(iframe);
    elem.classList.remove("hide");
    var a = window.IronIde.getValue();
    window.addEventListener('message', function (e) {
        switch (e.data.action) {
            case 'ReqScript':
                console.log("post script")
                iframe.contentWindow.postMessage({
                    action: 'GiveScript',
                    message: a
                }, '*',);
            case "ConsoleWrite":
                console.log(e.message)
        }
    });
}

function EngineDel() {
    document.getElementById("promptfream").remove();
    document.getElementById("prompt").classList.add("hide");
}