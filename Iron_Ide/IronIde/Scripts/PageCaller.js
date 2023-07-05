function EngineCall() {

    var elem = document.getElementsByTagName("iframe");
    if (elem.length !== 0) {
        elem[0].remove();
    }
    var iframe = document.createElement('iframe');
    iframe.classList.add("prompt");
    iframe.src = "./console.html"
    document.body.appendChild(iframe);
    var a = AceGetValue();
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