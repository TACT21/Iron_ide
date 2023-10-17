import { initializeApp } from "https://www.gstatic.com/firebasejs/10.0.0/firebase-app.js";
import { getDatabase, ref, push, set, onChildAdded, onValue, remove, onChildRemoved, child, get, onChildChanged } from "https://www.gstatic.com/firebasejs/10.0.0/firebase-database.js";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
    apiKey: "AIzaSyAzEm-lZvEz2s5Ed3ov9OJrB4iEeN4c4Io",
    authDomain: "ironpythonide.firebaseapp.com",
    databaseURL: "https://ironpythonide-default-rtdb.firebaseio.com",
    projectId: "ironpythonide",
    storageBucket: "ironpythonide.appspot.com",
    messagingSenderId: "270835211248",
    appId: "1:270835211248:web:513dc7ed225f9cb5d99b35",
    measurementId: "G-7PSWR5K3PE"
};

if(!(window.IronIde)){
    window.IronIde = {};
}

var prints = [];
var exameeCount = 0;
var testerCount = 0;
var answer = "";
var result = "";
var ifream = null;
var testerInputManager = null;

//URL Params Manager
const params = new URLSearchParams(document.location.search.substring(1));

// Initialize Firebase
const app = initializeApp(firebaseConfig);

// Initialize Realtime Database and get a reference to the service
const database = getDatabase(app);

const dbRef = ref(database, `${params.get("projectId")}/exsams`);

function Compere(){
    if(!(document.getElementById("prompt"))){
        alert("実行環境展開場所が存在しません。再読み込みしてください。")
        return;
    }
    document.getElementById("EngineStoper").classList.add("hide");
    var fream = document.getElementById("promptfream");
    if (fream) {
        fream.remove()
    }
    ifream = freamCaller(window.IronIde.TestScript);
    testerInputManager = function (e) {
        switch (e.data.action) {
            case 'asq':
                ifream.contentWindow.postMessage({
                    action: 'GiveInput',
                    message: window.IronIde.Inputs[testerCount]
                }, '*',);
                testerCount = testerCount + 1;
                break;
            case 'print':
                if(e.data.message === "<p style = \"color:#7df0a3\">All Tasks is Compreate.</p>")
                {
                    exameeDoTask()
                }else{
                    answer = e.data.message;
                }
        }
    }
    window.addEventListener('message', testerInputManager);
}

function exameeDoTask(){
    window.removeEventListener('message', testerInputManager);
    ifream = freamCaller(window.IronIde.getValue());
    testerInputManager = function (e) {
        switch (e.data.action) {
            case 'asq':
                ifream.contentWindow.postMessage({
                    action: 'GiveInput',
                    message: window.IronIde.Inputs[testerCount]
                }, '*',);
                prints.push(e.data.message);
                prints.push(window.IronIde.Inputs[testerCount]);
                testerCount = testerCount + 1;
                break;
            case 'print':
                prints.push(e.data.message);
                if(e.data.message === "<p style = \"color:#7df0a3\">All Tasks is Compreate.</p>")
                {
                    answer = prints[prints.length - 2];
                    push(dbRef,{
                        data:`${today.getFullYear()}`,
                        log:`${prints.join("\n")}`,
                        answer:`${answer}`,
                        result:`${result}`
                    })
                    if(answer == result){
                        alert("正解です。")
                    }else{
                        alert("不正解です。")
                    }
                    document.getElementById("EngineStoper").classList.remove("hide");
                    document.getElementById("EngineStoper").click();
                }
        }
    }
    window.addEventListener('message', testerInputManager);
}

function freamCaller(script){
    var elem = document.getElementById("prompt");
    var curtain = document.createElement('iframe');
    curtain.classList.add("curtain");
    elem.appendChild(curtain);
    var iframe = document.createElement('iframe');
    iframe.id = "promptfream";
    iframe.src = "./console.html"
    iframe.style.width = "100%";
    iframe.style.height = "calc(300px - 2rem)";
    iframe.style.position = "absolute";
    iframe.style.bottom = "0";
    var result = elem.appendChild(iframe);
    elem.classList.remove("hide");
    window.addEventListener('message', function (e) {
        switch (e.data.action) {
            case 'ReqScript':
                console.log("post script")
                iframe.contentWindow.postMessage({
                    action: 'GiveScript',
                    message: script
                }, '*',);
            case "ConsoleWrite":
                console.log(e.message)
        }
    });
    return result;
}

window.IronIde.compere = Compere;

console.log("The module named Compere is loaded");