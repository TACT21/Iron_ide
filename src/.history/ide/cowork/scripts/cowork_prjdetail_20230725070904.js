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
const params = new URLSearchParams(document.location.search.substring(1));

// Initialize Firebase
const app = initializeApp(firebaseConfig);

// Initialize Realtime Database and get a reference to the service
const database = getDatabase(app);

const dbRef = ref(database, `${params.get("projectId")}`);

onValue(
    child(dbRef,"detail"),
    function(snapshot){
        const data = snapshot.val();
        if(data){
            document.getElementById("projectDetailContent").innerHTML = data;
        }
    }
)

onValue(
    child(dbRef,"test"),
    function(snapshot){
        const data = snapshot.val();
        if(data){
            var script = document.createElement("script");
            script.src = "./scripts/compere.js"
            script.type = "module"
            document.head.appendChild(script);
            window.IronIde.TestScript = data;
            document.getElementById("projectDetailContent").innerHTML += "<br/><button style='btn' onclick='window.IronIde.compere()'></button>";
        }
    }
)

window.IronIde.EditMd = function(){
    document.getElementById("editor").src="./md_editor.html"
}

console.log("The module named projectDetail is loaded");