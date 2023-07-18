var editor;
var id = "editor"

window.addEventListener("load", init)

function init() {
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
    window.IronIde = {};
    window.IronIde.getValue = function () {
        return editor.getValue();
    };
    window.IronIde.setValue = function (e) {
        console.log(e + "\n@Ace.js AceSetValue");
        editor.getSession().setValue(e);
    };
}