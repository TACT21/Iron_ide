//from https://qiita.com/KokiSakano/items/a122bc0a1a368c697643#drop%E5%AE%9F%E8%A3%85
const ddZone = document.getElementById('dd-zone');
const name = document.getElementById('name');

var files =[]

const handleFiles = () => {
    for (let i = 0; i < files.length; i++) {
        const liName = document.createElement('li');
        liName.textContent = files[i].name;
        name.appendChild(liName);
        const liWebkit = document.createElement('li');
        liWebkit.textContent = files[i].webkitRelativePath;
        webkit.appendChild(liWebkit);
    }
};

ddZone.addEventListener('dragover', (event) => event.preventDefault());
const onDrop = async (event) => {
    event.preventDefault();

    // filesの初期化
    files = [];

    // 最上階層から再起的に低い階層へファイルを取得するまで呼び出す
    const searchFile = async (entry) => {
        // ファイルのwebkitRelativePathにパスを登録する
        if (entry.isFile) {
            const file = await new Promise((resolve) => {
                entry.file((file) => {
                    Object.defineProperty(file, "webkitRelativePath", {
                        // fullPathは/から始まるので二文字目から抜き出す
                        value: entry.fullPath.slice(1),
                    });
                    resolve(file);
                });
            });
            files.push(file);
            // ファイルが現れるまでこちらの分岐をループし続ける
        } else if (entry.isDirectory) {
            const dirReader = entry.createReader();
            let allEntries = [];
            const getEntries = () =>
                new Promise((resolve) => {
                    dirReader.readEntries((entries) => {
                        resolve(entries);
                    });
                });
            // readEntriesは100件ずつの取得なので、再帰で0件になるまで取ってくるようにする
            // https://developer.mozilla.org/en-US/docs/Web/API/FileSystemDirectoryReader/readEntries
            const readAllEntries = async () => {
                const entries = await getEntries();
                if (entries.length > 0) {
                    allEntries = [...allEntries, ...entries];
                    await readAllEntries();
                }
            };
            await readAllEntries();
            for (const entry of allEntries) {
                await searchFile(entry);
            }
        }
    };

    const items = event.dataTransfer.items;
    const calcFullPathPerItems = Array.from(items).map((item) => {
        return new Promise((resolve) => {
            const entry = item.webkitGetAsEntry();
            // nullの時は何もしない
            if (!entry) {
                resolve;
                return;
            }
            resolve(searchFile(entry));
        });
    });

    await Promise.all(calcFullPathPerItems);
    handleFiles();
};
ddZone.addEventListener('drop', onDrop);