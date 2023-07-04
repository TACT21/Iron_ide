function EngineCall() {
    var iframe = document.createElement('iframe');
    iframe.classList.add("prompt");
    iframe.src = "./console.html"
    document.body.appendChild(ifream);
    var a = AceGetValue();
    window.addEventListener('message', function (e) {
        switch (e.data.action) {
            case 'ReqScript':
                iframe.postMessage({
                    action: 'GiveScript',
                    message: a
                }, '*',);
        }
    });
}