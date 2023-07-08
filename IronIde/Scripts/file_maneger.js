var files = {}

// ドラッグ&ドロップエリアの取得
var fileArea = document.getElementById('dropArea');

// input[type=file]の取得
var fileInput = document.getElementById('uploadFile');

// ドラッグオーバー時の処理
fileArea.addEventListener('dragover', function (e) {
    e.preventDefault();
    fileArea.classList.add('dragover');
});

// ドラッグアウト時の処理
fileArea.addEventListener('dragleave', function (e) {
    e.preventDefault();
    fileArea.classList.remove('dragover');
});

// ドロップ時の処理
fileArea.addEventListener('drop', function (e) {
    e.preventDefault();
    fileArea.classList.remove('dragover');
    // ドロップしたファイルの取得
    var files = e.dataTransfer.files;
    // 取得したファイルをinput[type=file]へ
    fileInput.files = files;
});

// input[type=file]に変更があれば実行
// もちろんドロップ以外でも発火します
fileInput.addEventListener('change', function (e) {
    var file = e.target.files[0];

    if (typeof e.target.files[0] !== 'undefined') {
        let reader = new FileReader();
        FileReader.readAsText(file);
        if (SessonStorageRead(file.name) === "") {

        } else {
            document.getElementById("object_url").innerText = URL.createObjectURL(file)
            document.getElementById("fileName").value = "";
            document.getElementById("error").classList.remove("hide");
        }
    }
}, false);

function Rename() {

}

function FileZipper() {

}

function ProjFiler

function SessonStorageSave(id, mess) {
    if (mess === "") {
        window.sessionStorage.removeItem(id);
    } else {
        window.sessionStorage.setItem(id, mess);
    }
}

function SessonStorageRead(id) {
        return sessionStorage.getItem(id);
}

function LocalStorageSave(id, mess) {
    if (mess === "") {
        window.localStorage.removeItem(id);
    } else {
        window.localStorage.setItem(id, mess);
    }
}

function LocalStorageRead(id) {
    return localStorage.getItem(id);
}