var elementId = "editor"
var done = false;

// Import the functions you need from the SDKs you need
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
const params = new URLSearchParams(document.location.search.substring(1));
// Initialize Firebase
const app = initializeApp(firebaseConfig);
// Initialize Realtime Database and get a reference to the service
const database = getDatabase(app);

if(params.get("fileId") && params.get("projectId")){
    document.getElementById("NoHere").style.display = "none";
    if(window.IronIde){
        window.IronIde = {};
    }
    window.addEventListener("load", init)
}else{
    document.getElementById("editor").style.display = "none";
}


function init() {
    if(!(window.IronIde)){
        window.IronIde = {};
    }
    //set library
    window.IronIde.getUuid = () =>
    ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, (c) =>
      (
        c ^
        (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4)))
      ).toString(16)
    );

    //User id set
    if(!(window.IronIde.userId)){
        window.IronIde.userId = `guest${window.IronIde.getUuid()}`
    }

    var editor;
    //// Create ACE
    editor = ace.edit(elementId);
    editor.setTheme("ace/theme/textmate");
    var session = editor.getSession();
    session.setUseWrapMode(true);
    session.setUseWorker(false);
    session.setMode("ace/mode/python");

    console.log("getValue ADDED!");
    //add Ace bridges
    window.parent.IronIde.getValue = function () {
        return editor.getValue();
    };


    const dbRef = ref(database, 
        `${params.get("projectId")}/files/${params.get("fileId")}/history`);


    //Add Chenge Listener
    onChildAdded(dbRef, function (data) {
        var value = data.val()
        if (done && value.a == "i" && value.u != window.IronIde.userId) {
            console.log("cloud is chenged by anothor");
            InsertInput(GetPosition(value.s.r, value.s.c),value.v);
        } else if (done && value.a == "r" && value.u != window.IronIde.userId) {
            console.log("cloud is chenged by anothor");
            Remove(
                GetPosition(value.s.r, value.s.c),
                GetPosition(value.e.r, value.e.c)
            );
        }
    });

    
    //get File historys
    onValue(dbRef, (snapshot) => {
        const data = snapshot.val();
        for (const [key, value] of Object.entries(data)) {
            console.log(value);
            if (value.e) {
                Remove(
                    GetPosition(value.s.r, value.s.c),
                    GetPosition(value.e.r, value.e.c)
                );
            }else if (value.a = "i") {
                console.log("insert");
                InsertInput(GetPosition(value.s.r, value.s.c),value.v);
            } 
        }
        console.log("Chenges apply");
        done = true;
    },
    {onlyOnce:true});

    //One of Ace managers
    function InsertInput(start,content){
        var value = "";
        if(Array.isArray(content)){
            value = content.join('\n')
        }else if(typeof content == "string"){
            value = content;
        }
        editor.session.insert(start,value);
    }

    //One of Ace managers
    function Remove(start,end){
        console.log("rv");
        editor.session.remove({start:start,end:end});
    }

    function GetPosition(row,column){
        return{
            row:row,
            column:column
        }
    }
    //Add Key input Listener
    var d = null;
    var cursor = null;
    document.body.addEventListener('keyup',
    event => {
        console.log("cloud is chenged by me");
        if (done && d) {
            if ((d.start.row === cursor.row) && (d.start.column === cursor.column) && d.action === "insert") {
                push(dbRef,{
                    a: "i",
                    s: {
                        r: d.start.row,
                        c: d.start.column
                    },
                    v: d.lines,
                    u:window.IronIde.userId
                });
            } 
        }
    });

    editor.on("change", function (delta) {
        d = null;
        cursor = editor.getCursorPosition();
        if (done && (delta.start.row === cursor.row) && (delta.start.column === cursor.column)) {
            if (delta.action === "insert") {
                d = delta;
            } else if (delta.action === "remove") {
                console.log("cloud is chenged by me");
                push(dbRef,{
                    a: "r",
                    s: {
                        r: delta.start.row,
                        c: delta.start.column
                    },
                    e: {
                        r: delta.end.row,
                        c: delta.end.column
                    },
                    u:window.IronIde.userId
                });
            }
        }
    });
}
