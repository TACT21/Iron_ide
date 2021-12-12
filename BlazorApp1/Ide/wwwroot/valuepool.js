export function Wake_up_monaco() {
    require(['vs/editor/editor.main'], function () {
        var editor = monaco.editor.create(document.getElementById('container'), {
            value: ['function x() {', '\tprint("Hello world!")', '}'].join('\n'),
            language: 'javascript'
        });
    });
}

