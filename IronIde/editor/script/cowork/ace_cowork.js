var editor;
var elementId = "editor"
var fileId = ""

// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/10.0.0/firebase-app.js";
import { getDatabase, ref, push, set, onChildAdded, remove, onChildRemoved, child, get, onChildChanged } from "https://www.gstatic.com/firebasejs/10.0.0/firebase-database.js";
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

// Initialize Firebase
const app = initializeApp(firebaseConfig);
// Initialize Realtime Database and get a reference to the service
const database = getDatabase(app);

window.addEventListener("load", init)

function init() {
    //// Create ACE
    editor = ace.edit(elementId);
    editor.setTheme("ace/theme/textmate");
    var session = editor.getSession();
    session.setUseWrapMode(true);
    session.setUseWorker(false);
    session.setMode("ace/mode/python");

    const dbRef = ref(db, `files/${fileId}`);

    //Add Key input Listener
    editor.on("change", function (delta) {
        var cursor = editor.getCursorPosition();
        if ((delta.start.row === cursor.row) && (delta.start.column === cursor.column)) {
            if (delta.action === "insert") {
                dbRef.push({
                    a: "i",
                    s: {
                        r: delta.start.row,
                        c: delta.start.column
                    },
                    v: delta.lines
                });
            } else if (delta.action === "remove") {
                dbRef.push({
                    a: "r",
                    s: {
                        r: delta.start.row,
                        c: delta.start.column
                    },
                    e: {
                        r: delta.start.row,
                        c: delta.start.column
                    }
                });
            }
        }
    });

    //Add Chenge Listener
    onChildChanged(dbRef, function (data) {
        var value = data.val()
        if (value.a = "i") {
            editor.session.insert(
                {
                    row: value.s.r,
                    column: value.s.c
                },
                value.v);
        } else if (value.a = "r") {
            editor.session.remove(
                new Range(value.s.r, value.s.c, value.e.r, value.e.c)
            );
        }
    }
}

function aceGetValue() {
    return editor.getValue();
}

function aceSetValue(e) {
    console.log(e + "\n@Ace.js AceSetValue");
    editor.getSession().setValue(e);
}

window.ace = {
    getValue: aceGetValue(),
    setValue: aceSetValue(e)
};