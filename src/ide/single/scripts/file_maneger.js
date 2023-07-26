var files = []
var currentFileName = ""

window.addEventListener("load", function () {
    // ドラッグ&ドロップエリアの取得
    var fileArea = document.getElementById('Filer');

    // input[type=file]の取得
    var fileInput = document.getElementById('userfile');

    fileArea.addEventListener('dragover', function (e) {
        e.preventDefault();
        fileArea.classList.add('dragover');
    });

    fileArea.addEventListener('dragleave', function (e) {
        e.preventDefault();
        fileArea.classList.remove('dragover');
    });

    fileArea.addEventListener('drop', function (e) {
        e.preventDefault();
        fileArea.classList.remove('dragover');
        var files = e.dataTransfer.files;
        fileInput.files = files;
    });

    fileInput.addEventListener('change', function (e) {
        var file = e.target.files[0];
        if (typeof e.target.files[0] !== 'undefined') {
            let reader = new FileReader();
            reader.readAsText(file);
            reader.addEventListener("load", () => {
                if (sessonStorageRead(file.name)) {
                    document.getElementById("object_url").innerText = URL.createObjectURL(file)
                    document.getElementById("fileName").value = "";
                    document.getElementById("error").classList.remove("hide");
                } else {
                    newFileSaver(file.name, reader.result);
                }
            });
        }
    }, false);
});

async function rename() {
    let file = await fetch(document.getElementById("object_url").innerText).then(r => r.blob());
    //URL 廃棄
    URL.revokeObjectURL(document.getElementById("object_url").innerText)
    document.getElementById("object_url").innerText = "";
    //file操作
    file.name = document.getElementById("fileName").value
    let reader = new FileReader();
    reader.readAsText(file);
    reader.addEventListener("load", () => {
        newFileSaver(file.name, reader.result);
    });
}

function chengeFile(aim) {
    sessonStorageSave(currentFileName, aceGetValue());
    aceSetValue(sessonStorageRead(aim));
    currentFileName = aim;
    document.getElementById("fileName").innerText = aim;
}

function newFileSaver(path, content) {
    sessonStorageSave(path, content);
    files.push(path);
    const addButton = document.createElement('button');
    addButton.classList.add("file");
    addButton.classList.add("list");
    addButton.classList.add("item");
    addButton.setAttribute('onclick', "chengeFile('" + path + "')");
    addButton.innerText = path;
    document.getElementById("FileList").appendChild(addButton)
    chengeFile(path);
}

function sessonStorageSave(id, mess) {
    if (mess === "") {
        window.sessionStorage.removeItem(id);
    } else {
        window.sessionStorage.setItem(id, mess);
    }
}

function sessonStorageRead(id) {
        return sessionStorage.getItem(id);
}

function localStorageSave(id, mess) {
    if (mess === "") {
        window.localStorage.removeItem(id);
    } else {
        window.localStorage.setItem(id, mess);
    }
}

function localStorageRead(id) {
    return localStorage.getItem(id);
}

function fileZipper() {
    if (document.getElementById("JsZip")){
        var zip = new JSZip();
        files.forEach((e) => {
            zip.file(e, localStorageSave(e));
        });
        zip.generateAsync({ type: "blob" })
            .then(function (blob) {
                downLoadHelper(blob, "your_project.zip");
            });
    } else {
        var script = document.createElement('script');
        script.src = 'https://cdnjs.cloudflare.com/ajax/libs/jszip/3.5.0/jszip.min.js';
        script.id = "JsZip"
        script.onload = function () {
            fileZipper();
        }
        document.head.appendChild(script);
    }
}

function projFileSaver(isDownload = false) {
    var json = []
    files.forEach((e) => {
        json.add(
            {
                "name": e,
                "content": sessonStorageRead(e)
            }
        )
    });
    if (isDownload) {
        downLoadHelper(new Blob([JSON.stringify(json)], { "endings": "native" }), "your_project.ironprj")
    }
    return JSON.stringify(json);
}

function projFileLoader(json) {
    var proj = JSON.parse(json);
    proj.forEach((e) => {
        newFileSaver(e.nane, e.content)
    })
}

function projFileLoadHelper() {
    var input = document.createElement("input");
    input.type = "file";
    input.addEventListener("input", function (e) {
        let reader = new FileReader();
        reader.readAsText(e.target.files);
        reader.addEventListener("load", () => {
            projFileLoader(reader.result);
        });
    });
    input.click();
}

function downLoadHelper(content, name) {
    if (document.getElementById("FileSaver")) {
        if (typeof (saveAs) == "function") {
            saveAs(content, name);
        }
    } else {
        var script = document.createElement('script');
        script.src = 'https://cdnjs.cloudflare.com/ajax/libs/FileSaver.js/1.3.3/FileSaver.min.js';
        script.id = "FileSaver"
        script.onload = function () {
            downLoadHelper(content, name);
        }
        document.head.appendChild(script);
    }
}