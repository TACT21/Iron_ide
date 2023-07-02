const script = document.createElement('script');
 
script.src = 'https://cdnjs.cloudflare.com/ajax/libs/ace/1.2.0/ace.js';
 
document.head.appendChild(script);

script.addEventListener("load",LoadAce,false);

function LoadAce(){
    var editor = ace.edit("editor");
    editor.setTheme("ace/theme/monokai");
    editor.setFontSize(14);
    editor.getSession().setMode("ace/mode/html");
    editor.getSession().setUseWrapMode(true);
    editor.getSession().setTabSize(2);
}