var cacheName = 'IronIde 0.0.4';
// キャッシュファイルの指定
var CACHE_NAME = 'IronIde';
var urlsToCache = [
    "/.stamp",
    "/Console.css",
    "/console.html",
    "/dotnet.js",
    "/dotnet.js.symbols",
    "/dotnet.timezones.blat",
    "/dotnet.wasm",
    "/dotnet.worker.js",
    "/file_list.txt",
    "/icudt_CJK.dat",
    "/icudt_EFIGS.dat",
    "/icudt_no_CJK.dat",
    "/img1.png",
    "/img2.png",
    "/index.css",
    "/index.html",
    "/index.js",
    "/login_maneger.js",
    "/main.js",
    "/mono-config.json",
    "/rogo.png",
    "/service_worker.js",
    "/ide/img/1.jpg",
    "/ide/img/2.jpg",
    "/ide/img/3.jpg",
    "/ide/img/4.jpg",
    "/ide/img/5.jpg",
    "/ide/img/6.jpg",
    "/ide/img/7.jpg",
    "/ide/img/8.jpg",
    "/ide/shered/scripts/page_caller.js",
    "/ide/shered/styles/file_piker.css",
    "/ide/shered/styles/gernal.css",
    "/ide/single/editor.html",
    "/ide/single/scripts/ace.js",
    "/ide/single/scripts/file_maneger.js",
    "/ide/single/scripts/compere.js",
    "/ide/single/styles/Editor.css",
    "/managed/IronIde.dll",
    "/managed/IronIde.pdb",
    "/managed/IronPython.dll",
    "/managed/IronPython.Modules.dll",
    "/managed/IronPython.SQLite.dll",
    "/managed/IronPython.Wpf.dll",
    "/managed/Microsoft.CSharp.dll",
    "/managed/Microsoft.Dynamic.dll",
    "/managed/Microsoft.Scripting.dll",
    "/managed/Microsoft.Scripting.Metadata.dll",
    "/managed/Microsoft.VisualBasic.Core.dll",
    "/managed/Microsoft.VisualBasic.dll",
    "/managed/Microsoft.Win32.Primitives.dll",
    "/managed/Microsoft.Win32.Registry.dll",
    "/managed/Mono.Unix.dll",
    "/managed/Mono.Unix.pdb",
    "/managed/mscorlib.dll",
    "/managed/netstandard.dll",
    "/managed/System.AppContext.dll",
    "/managed/System.Buffers.dll",
    "/managed/System.CodeDom.dll",
    "/managed/System.Collections.Concurrent.dll",
    "/managed/System.Collections.dll",
    "/managed/System.Collections.Immutable.dll",
    "/managed/System.Collections.NonGeneric.dll",
    "/managed/System.Collections.Specialized.dll",
    "/managed/System.ComponentModel.Annotations.dll",
    "/managed/System.ComponentModel.DataAnnotations.dll",
    "/managed/System.ComponentModel.dll",
    "/managed/System.ComponentModel.EventBasedAsync.dll",
    "/managed/System.ComponentModel.Primitives.dll",
    "/managed/System.ComponentModel.TypeConverter.dll",
    "/managed/System.Configuration.dll",
    "/managed/System.Console.dll",
    "/managed/System.Core.dll",
    "/managed/System.Data.Common.dll",
    "/managed/System.Data.DataSetExtensions.dll",
    "/managed/System.Data.dll",
    "/managed/System.Diagnostics.Contracts.dll",
    "/managed/System.Diagnostics.Debug.dll",
    "/managed/System.Diagnostics.DiagnosticSource.dll",
    "/managed/System.Diagnostics.FileVersionInfo.dll",
    "/managed/System.Diagnostics.Process.dll",
    "/managed/System.Diagnostics.StackTrace.dll",
    "/managed/System.Diagnostics.TextWriterTraceListener.dll",
    "/managed/System.Diagnostics.Tools.dll",
    "/managed/System.Diagnostics.TraceSource.dll",
    "/managed/System.Diagnostics.Tracing.dll",
    "/managed/System.dll",
    "/managed/System.Drawing.dll",
    "/managed/System.Drawing.Primitives.dll",
    "/managed/System.Dynamic.Runtime.dll",
    "/managed/System.Formats.Asn1.dll",
    "/managed/System.Formats.Tar.dll",
    "/managed/System.Globalization.Calendars.dll",
    "/managed/System.Globalization.dll",
    "/managed/System.Globalization.Extensions.dll",
    "/managed/System.IO.Compression.Brotli.dll",
    "/managed/System.IO.Compression.dll",
    "/managed/System.IO.Compression.FileSystem.dll",
    "/managed/System.IO.Compression.ZipFile.dll",
    "/managed/System.IO.dll",
    "/managed/System.IO.FileSystem.AccessControl.dll",
    "/managed/System.IO.FileSystem.dll",
    "/managed/System.IO.FileSystem.DriveInfo.dll",
    "/managed/System.IO.FileSystem.Primitives.dll",
    "/managed/System.IO.FileSystem.Watcher.dll",
    "/managed/System.IO.IsolatedStorage.dll",
    "/managed/System.IO.MemoryMappedFiles.dll",
    "/managed/System.IO.Pipes.AccessControl.dll",
    "/managed/System.IO.Pipes.dll",
    "/managed/System.IO.UnmanagedMemoryStream.dll",
    "/managed/System.Linq.dll",
    "/managed/System.Linq.Expressions.dll",
    "/managed/System.Linq.Parallel.dll",
    "/managed/System.Linq.Queryable.dll",
    "/managed/System.Memory.dll",
    "/managed/System.Net.dll",
    "/managed/System.Net.Http.dll",
    "/managed/System.Net.Http.Json.dll",
    "/managed/System.Net.HttpListener.dll",
    "/managed/System.Net.Mail.dll",
    "/managed/System.Net.NameResolution.dll",
    "/managed/System.Net.NetworkInformation.dll",
    "/managed/System.Net.Ping.dll",
    "/managed/System.Net.Primitives.dll",
    "/managed/System.Net.Quic.dll",
    "/managed/System.Net.Requests.dll",
    "/managed/System.Net.Security.dll",
    "/managed/System.Net.ServicePoint.dll",
    "/managed/System.Net.Sockets.dll",
    "/managed/System.Net.WebClient.dll",
    "/managed/System.Net.WebHeaderCollection.dll",
    "/managed/System.Net.WebProxy.dll",
    "/managed/System.Net.WebSockets.Client.dll",
    "/managed/System.Net.WebSockets.dll",
    "/managed/System.Numerics.dll",
    "/managed/System.Numerics.Vectors.dll",
    "/managed/System.ObjectModel.dll",
    "/managed/System.Private.CoreLib.dll",
    "/managed/System.Private.DataContractSerialization.dll",
    "/managed/System.Private.Uri.dll",
    "/managed/System.Private.Xml.dll",
    "/managed/System.Private.Xml.Linq.dll",
    "/managed/System.Reflection.DispatchProxy.dll",
    "/managed/System.Reflection.dll",
    "/managed/System.Reflection.Emit.dll",
    "/managed/System.Reflection.Emit.ILGeneration.dll",
    "/managed/System.Reflection.Emit.Lightweight.dll",
    "/managed/System.Reflection.Extensions.dll",
    "/managed/System.Reflection.Metadata.dll",
    "/managed/System.Reflection.Primitives.dll",
    "/managed/System.Reflection.TypeExtensions.dll",
    "/managed/System.Resources.Reader.dll",
    "/managed/System.Resources.ResourceManager.dll",
    "/managed/System.Resources.Writer.dll",
    "/managed/System.Runtime.CompilerServices.Unsafe.dll",
    "/managed/System.Runtime.CompilerServices.VisualC.dll",
    "/managed/System.Runtime.dll",
    "/managed/System.Runtime.Extensions.dll",
    "/managed/System.Runtime.Handles.dll",
    "/managed/System.Runtime.InteropServices.dll",
    "/managed/System.Runtime.InteropServices.JavaScript.dll",
    "/managed/System.Runtime.InteropServices.RuntimeInformation.dll",
    "/managed/System.Runtime.Intrinsics.dll",
    "/managed/System.Runtime.Loader.dll",
    "/managed/System.Runtime.Numerics.dll",
    "/managed/System.Runtime.Serialization.dll",
    "/managed/System.Runtime.Serialization.Formatters.dll",
    "/managed/System.Runtime.Serialization.Json.dll",
    "/managed/System.Runtime.Serialization.Primitives.dll",
    "/managed/System.Runtime.Serialization.Xml.dll",
    "/managed/System.Security.AccessControl.dll",
    "/managed/System.Security.Claims.dll",
    "/managed/System.Security.Cryptography.Algorithms.dll",
    "/managed/System.Security.Cryptography.Cng.dll",
    "/managed/System.Security.Cryptography.Csp.dll",
    "/managed/System.Security.Cryptography.dll",
    "/managed/System.Security.Cryptography.Encoding.dll",
    "/managed/System.Security.Cryptography.OpenSsl.dll",
    "/managed/System.Security.Cryptography.Primitives.dll",
    "/managed/System.Security.Cryptography.X509Certificates.dll",
    "/managed/System.Security.dll",
    "/managed/System.Security.Principal.dll",
    "/managed/System.Security.Principal.Windows.dll",
    "/managed/System.Security.SecureString.dll",
    "/managed/System.ServiceModel.Web.dll",
    "/managed/System.ServiceProcess.dll",
    "/managed/System.Text.Encoding.CodePages.dll",
    "/managed/System.Text.Encoding.dll",
    "/managed/System.Text.Encoding.Extensions.dll",
    "/managed/System.Text.Encodings.Web.dll",
    "/managed/System.Text.Json.dll",
    "/managed/System.Text.RegularExpressions.dll",
    "/managed/System.Threading.Channels.dll",
    "/managed/System.Threading.dll",
    "/managed/System.Threading.Overlapped.dll",
    "/managed/System.Threading.Tasks.Dataflow.dll",
    "/managed/System.Threading.Tasks.dll",
    "/managed/System.Threading.Tasks.Extensions.dll",
    "/managed/System.Threading.Tasks.Parallel.dll",
    "/managed/System.Threading.Thread.dll",
    "/managed/System.Threading.ThreadPool.dll",
    "/managed/System.Threading.Timer.dll",
    "/managed/System.Transactions.dll",
    "/managed/System.Transactions.Local.dll",
    "/managed/System.ValueTuple.dll",
    "/managed/System.Web.dll",
    "/managed/System.Web.HttpUtility.dll",
    "/managed/System.Windows.dll",
    "/managed/System.Xml.dll",
    "/managed/System.Xml.Linq.dll",
    "/managed/System.Xml.ReaderWriter.dll",
    "/managed/System.Xml.Serialization.dll",
    "/managed/System.Xml.XDocument.dll",
    "/managed/System.Xml.XmlDocument.dll",
    "/managed/System.Xml.XmlSerializer.dll",
    "/managed/System.Xml.XPath.dll",
    "/managed/System.Xml.XPath.XDocument.dll",
    "/managed/WindowsBase.dll",
    "/supportFiles/0_runtimeconfig.bin",
    "/supportFiles/1_dotnet.js.symbols",
    "https://cdnjs.cloudflare.com/ajax/libs/ace/1.2.0/ace.js",
    "https://cdnjs.cloudflare.com/ajax/libs/ace/1.2.0/ext-language_tools.js",
];

// インストール処理
self.addEventListener('install', function (event) {
    if(self.location.origin.indexOf("127.0.0.1") == -1){
        event.waitUntil(
            caches
                .open(CACHE_NAME)
                .then(function (cache) {
                    try{
                        return cache.addAll(urlsToCache);
                    }catch(e){
                        console.log(e);
                    }
                })
        );
    }
});

self.addEventListener('activate', (event) => {
    if(self.location.origin.indexOf("127.0.0.1") == -1){

        event.waitUntil(
            caches
                .open(CACHE_NAME)
                .then(function (cache) {
                    try{
                        return cache.addAll(urlsToCache);
                    }catch(e){
                        console.log(e);
                    }
                })
        );
    }
});

// リソースフェッチ時のキャッシュロード処理
self.addEventListener('fetch', function (event) {
    if(self.location.origin.indexOf("127.0.0.1") == -1){
        event.respondWith(
            caches
                .match(event.request)
                .then(function (response) {
                    return response ? response : fetch(event.request);
                })
        );
    }
});

/*caches.match("/Console.css").then(function(e){
    var i = null;
    iterator = e.headers.keys()
    while(true){
        iteratorResult = iterator.next(); // 順番に値を取りだす
        if(iteratorResult.done) break; // 取り出し終えたなら、break
        if(iteratorResult.value == "date"){
            break
        }
        i = i + 1;
    }
    if(i){
        var index = 0;
        iterator = e.headers.values()
        while(true){
            iteratorResult = iterator.next(); // 順番に値を取りだす
            if(iteratorResult.done) break; // 取り出し終えたなら、break
            if(index === i){
                var date = new Date(iteratorResult.value)
                console.log(date.toUTCString());
            }
            index = index + 1;
        }
    }
})*/