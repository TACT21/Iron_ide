var editor;
var id = "editor"

window.addEventListener("load", init)

function init() {
    var config = {
        apiKey: "AIzaSyAzEm-lZvEz2s5Ed3ov9OJrB4iEeN4c4Io",
        authDomain: "ironpythonide.firebaseapp.com",
        projectId: "ironpythonide",
        storageBucket: "ironpythonide.appspot.com",
        messagingSenderId: "270835211248",
        appId: "1:270835211248:web:513dc7ed225f9cb5d99b35",
        measurementId: "G-7PSWR5K3PE"
    };

    const app = initializeApp(firebaseConfig);

    //// Get Firebase Database reference.

    firebase.initializeApp(config);

    //// Get Firebase Database reference.
    var firepadRef = getExampleRef();


    /*const div = document.createElement('div');
    div.style.height = "600px";
    div.id = id;
    document.body.appendChild(div);*/
    editor = ace.edit(id);
    editor.$blockScrolling = Infinity;
    editor.setOptions({
        enableBasicAutocompletion: true,
        enableSnippets: true,
        enableLiveAutocompletion: true
    });
    editor.setTheme("ace/theme/github");
    editor.getSession().setMode("ace/mode/python");
    // Create a random ID to use as our user ID (we must give this to firepad and FirepadUserList).
    var userId = Math.floor(Math.random() * 9999999999).toString();
    var firepad = Firepad.fromACE(firepadRef, editor, {
        userId: userId
    });
    //// Create FirepadUserList (with our desired userId).
    var firepadUserList = FirepadUserList.fromDiv(firepadRef.child('users'),
        document.getElementById('userlist'), userId);
}


function aceGetValue() {
    return editor.getValue();
}

function aceSetValue(e) {
    console.log(e + "\n@Ace.js AceSetValue");
    editor.getSession().setValue(e);
}

function getExampleRef() {
    var ref = firebase.database().ref();
    var hash = window.location.hash.replace(/#/g, '');
    if (hash) {
        ref = ref.child(hash);
    } else {
        ref = ref.push(); // generate unique location.
        window.location = window.location + '#' + ref.key; // add it as a hash to the URL.
    }
    if (typeof console !== 'undefined') {
        console.log('Firebase data: ', ref.toString());
    }
    return ref;
}
