var editor;

console.log("Load AceBridge")
function AceInit(id) {
    editor = ace.edit(id);
    editor.$blockScrolling = Infinity;
    editor.setOptions({
        enableBasicAutocompletion: true,
        enableSnippets: true,
        enableLiveAutocompletion: true
    });
    editor.setTheme("ace/theme/github");
    editor.getSession().setMode("ace/mode/python");
}

function AceGetValue() {
    return editor.getValue();
}

function AceSetValue(e) {
    console.log(e + "\n@Ace.js AceSetValue");
    editor.getSession().setValue(e);
}