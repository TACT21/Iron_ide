const requestUrl = "_content/input.html"

function GetInput(guid) {
    const xhr = new XMLHttpRequest();
    const url = new URL(requestUrl, baseUrl);
    url.searchParams.set("action", "GetInput");
    url.searchParams.set("id", guid);
    xhr.open("GET", url, false);
    xhr.send(null);

    return JSON.stringify({ Status: xhr.status, Response: xhr.response });
}

function SetBaseUrl(url) {
    baseUrl = url;
}