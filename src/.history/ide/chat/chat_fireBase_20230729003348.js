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

//URL Params Manager
const params = new URLSearchParams(window.location.search.substring(1));

// Initialize Firebase
const app = initializeApp(firebaseConfig);

// Initialize Realtime Database and get a reference to the service
const database = getDatabase(app);

//set library
window.IronIde.getUuid = () =>
([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, (c) =>
    (
    c ^
    (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4)))
    ).toString(16)
);

if(!(window.IronIde.userId)){
    window.IronIde.userId = `guest${window.IronIde.getUuid()}`
}

const dbRef = ref(database, `${params.get("projectId")}/chat`);

window.IronIde.chat = function(mess){
    push(dbRef,{
        'u': window.IronIde.userId,
        'm': mess
    });
}

onChildAdded(dbRef, function (data) {
    var value = data.val()
    if (value.u != window.IronIde.userId) {
        document.getElementById("bms_messages")
        MakeMassageElement(value.m,value.u).classList.add("bms_right")
    } else if (value.a == "r" && value.u != window.IronIde.userId) {
        document.getElementById("bms_messages")
        MakeMassageElement(value.m,value.u).classList.add("bms_left")
    }
});

function MakeMassageElement(content,name){
    var card = document.createElement("div");
    card.classList.add("bms_message");
    if(name){
        var title = document.createElement("div");
        title.classList.add("bms_message_name");
        title.innerText = name;
        card.appendChild(title);
    }
    
    var content = document.createElement("div");
    content.classList.add("bms_message_content");

    var text = document.createElement("div");
    text.classList.add("bms_message_text");

    content.appendChild(text);
    card.appendChild(content);

    return document.getElementById("bms_messages").appendChild(card);
}