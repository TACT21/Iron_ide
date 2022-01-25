var test = new Map()
var wait = 1000

self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

async function Getinput(timeout){
    if(timeout % wait != 0){//切り上げ処置
        timeout = timeout +(timeout - (timeout % wait))
    }
    //SampleApp アプリケーションアセンブラのGetinputを実行
    for(i = 0;i<timeout / wait; i++){
        DotNet.invokeMethodAsync('SampleApp', 'Getinput')
        .then(data => {
            if(data != ""){
                return data;
            }
        });
        await delay(wait);
    }
    return null
}

const spPath = "_content/input.html";

function IsSpecial(request) {
    let url = new URL(request.url);
    if (url.pathname.endsWith(spPath)) {
        return true;
    } else {
        return false;
    }
}

async function MakeResponse(request) {
    let url = new URL(request.url);
    //?action = GetInput
    const action = url.searchParams.get("action");
    if (action == "GetInput") {
        const value = await Getinput(30000);
        const response = new Response(value);
        response.status = 200;
        return response;
    }
}

async function onFetch(event) {
    // 書き換え対象の特殊リクエストかどうか確かめ、そうならば書き換えたレスポンスを返す
    if (IsSpecial(event.request)) {
        let response = await MakeResponse(event.request);
        return response;
    }
    return await fetch(event.request);
}

