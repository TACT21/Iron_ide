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

var files = {};

if(!(window.IronIde)){
    window.IronIde = {};
}

//URL Params Manager
const params = new URLSearchParams(document.location.search.substring(1));

// Initialize Firebase
const app = initializeApp(firebaseConfig);

// Initialize Realtime Database and get a reference to the service
const database = getDatabase(app);

const dbRef = ref(database, `${params.get("projectId")}/files`);

//Add File Add Listener
onChildAdded(dbRef, function (data) {
    AppendFile(data.key,data.val()["name"])
    files[data.key] = data.val()["name"];
});

//Add File Remove Listener
onChildRemoved(dbRef, function (data) {
    window.IronIde.RemoveFile(data.key)
});

// get input[type=file]
var fileInput = document.getElementById('userfile');
var currentFileName = "";

fileInput.addEventListener('change', function (e) {
    var file = e.target.files[0];
    if (typeof e.target.files[0] !== 'undefined') {
        let reader = new FileReader();
        reader.readAsText(file);
        reader.addEventListener("load", () => {
            CreateFile(file.name, reader.result);
        });
    }
}, false);

window.IronIde.DisableRename = function(){
    window.IronIde.Rename = null
}

window.IronIde.EnableRename = function(){
    window.IronIde.Rename = async function () {
        set(
            child(child(dbRef,currentFileName),"name"),
            document.getElementById("fileName").value
        )
    }
}

window.IronIde.Delete = async function () {
    remove(child(dbRef,currentFileName))
}

function chengeFile(aim) {
    document.getElementById("editor").src=`./cowork_editor.html?projectId=${params.get("projectId")}&fileId=${aim}`
    currentFileName = aim;
    document.getElementById("fileName").value = files[aim];
}

function AppendFile(path,name) {
    console.log("ADD BTN!");
    const addButton = document.createElement('button');
    addButton.classList.add("file");
    addButton.classList.add("list");
    addButton.classList.add("item");
    addButton.setAttribute('onclick', "chengeFile('" + path + "')");
    addButton.innerText = name;
    document.getElementById("FileList").appendChild(addButton)
    chengeFile(path);
}

window.IronIde.chengeFile = chengeFile;

window.IronIde.newFileSaver = function () {
    CreateFile("newfile.py","");
};

function CreateFile(name,content){
    var file = push(dbRef,{
        name:`${name}`,
        history:null
    });
    push(child(child(dbRef,file.key),"history"),{
        a: "i",
        s: {
            r: 0,
            c: 0
        },
        v: String(content).replace("\r\n","\n").replace("\r","\n").split("\n")
    });

}

//Take File List
onValue(dbRef, (snapshot) => {
    const data = snapshot.val();
    if(!data){
        CreateFile("newfile.py","");
    }
},
{onlyOnce:true});

console.log("IronIde file manager is loaded");