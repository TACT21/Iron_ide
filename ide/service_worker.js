// キャッシュファイルの指定
var CACHE_NAME = 'pwa-sample-caches';
var urlsToCache = [
    'page_caller.js',
    "file_maneger.js",
    "ace.js",
    "https://cdnjs.cloudflare.com/ajax/libs/ace/1.2.0/ace.js",
    "https://cdnjs.cloudflare.com/ajax/libs/ace/1.2.0/ext-language_tools.js",
    "file_piker.css",
    "gernal.css",
    "Editor.css",
    "1.jpg",
    "2.jpg",
    "3.jpg",
    "4.jpg",
    "5.jpg",
    "editor.html"
];

// インストール処理
self.addEventListener('install', function (event) {
    event.waitUntil(
        caches
            .open(CACHE_NAME)
            .then(function (cache) {
                return cache.addAll(urlsToCache);
            })
    );
});

// リソースフェッチ時のキャッシュロード処理
self.addEventListener('fetch', function (event) {
    event.respondWith(
        caches
            .match(event.request)
            .then(function (response) {
                return response ? response : fetch(event.request);
            })
    );
});
