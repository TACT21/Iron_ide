/*!-----------------------------------------------------------------------------
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Version: 0.31.0(252e010eb73ddc2fa1a37c1dade7bf35d87106cd)
 * Released under the MIT license
 * https://github.com/microsoft/monaco-editor/blob/main/LICENSE.txt
 *-----------------------------------------------------------------------------*/
define("vs/basic-languages/coffee/coffee",[],()=>{
var moduleExports = (() => {
  var __defProp = Object.defineProperty;
  var __markAsModule = (target) => __defProp(target, "__esModule", { value: true });
  var __export = (target, all) => {
    __markAsModule(target);
    for (var name in all)
      __defProp(target, name, { get: all[name], enumerable: true });
  };

  // src/basic-languages/coffee/coffee.ts
  var coffee_exports = {};
  __export(coffee_exports, {
    conf: () => conf,
    language: () => language
  });
  var conf = {
    wordPattern: /(-?\d*\.\d\w*)|([^\`\~\!\@\#%\^\&\*\(\)\=\$\-\+\[\{\]\}\\\|\;\:\'\"\,\.\<\>\/\?\s]+)/g,
    comments: {
      blockComment: ["###", "###"],
      lineComment: "#"
    },
    brackets: [
      ["{", "}"],
      ["[", "]"],
      ["(", ")"]
    ],
    autoClosingPairs: [
      { open: "{", close: "}" },
      { open: "[", close: "]" },
      { open: "(", close: ")" },
      { open: '"', close: '"' },
      { open: "'", close: "'" }
    ],
    surroundingPairs: [
      { open: "{", close: "}" },
      { open: "[", close: "]" },
      { open: "(", close: ")" },
      { open: '"', close: '"' },
      { open: "'", close: "'" }
    ],
    folding: {
      markers: {
        start: new RegExp("^\\s*#region\\b"),
        end: new RegExp("^\\s*#endregion\\b")
      }
    }
  };
  var language = {
    defaultToken: "",
    ignoreCase: true,
    tokenPostfix: ".coffee",
    brackets: [
      { open: "{", close: "}", token: "delimiter.curly" },
      { open: "[", close: "]", token: "delimiter.square" },
      { open: "(", close: ")", token: "delimiter.parenthesis" }
    ],
    regEx: /\/(?!\/\/)(?:[^\/\\]|\\.)*\/[igm]*/,
    keywords: [
      "and",
      "or",
      "is",
      "isnt",
      "not",
      "on",
      "yes",
      "@",
      "no",
      "off",
      "true",
      "false",
      "null",
      "this",
      "new",
      "delete",
      "typeof",
      "in",
      "instanceof",
      "return",
      "throw",
      "break",
      "continue",
      "debugger",
      "if",
      "else",
      "switch",
      "for",
      "while",
      "do",
      "try",
      "catch",
      "finally",
      "class",
      "extends",
      "super",
      "undefined",
      "then",
      "unless",
      "until",
      "loop",
      "of",
      "by",
      "when"
    ],
    symbols: /[=><!~?&%|+\-*\/\^\.,\:]+/,
    escapes: /\\(?:[abfnrtv\\"'$]|x[0-9A-Fa-f]{1,4}|u[0-9A-Fa-f]{4}|U[0-9A-Fa-f]{8})/,
    tokenizer: {
      root: [
        [/\@[a-zA-Z_]\w*/, "variable.predefined"],
        [
          /[a-zA-Z_]\w*/,
          {
            cases: {
              this: "variable.predefined",
              "@keywords": { token: "keyword.$0" },
              "@default": ""
            }
          }
        ],
        [/[ \t\r\n]+/, ""],
        [/###/, "comment", "@comment"],
        [/#.*$/, "comment"],
        ["///", { token: "regexp", next: "@hereregexp" }],
        [/^(\s*)(@regEx)/, ["", "regexp"]],
        [/(\()(\s*)(@regEx)/, ["@brackets", "", "regexp"]],
        [/(\,)(\s*)(@regEx)/, ["delimiter", "", "regexp"]],
        [/(\=)(\s*)(@regEx)/, ["delimiter", "", "regexp"]],
        [/(\:)(\s*)(@regEx)/, ["delimiter", "", "regexp"]],
        [/(\[)(\s*)(@regEx)/, ["@brackets", "", "regexp"]],
        [/(\!)(\s*)(@regEx)/, ["delimiter", "", "regexp"]],
        [/(\&)(\s*)(@regEx)/, ["delimiter", "", "regexp"]],
        [/(\|)(\s*)(@regEx)/, ["delimiter", "", "regexp"]],
        [/(\?)(\s*)(@regEx)/, ["delimiter", "", "regexp"]],
        [/(\{)(\s*)(@regEx)/, ["@brackets", "", "regexp"]],
        [/(\;)(\s*)(@regEx)/, ["", "", "regexp"]],
        [
          /}/,
          {
            cases: {
              "$S2==interpolatedstring": {
                token: "string",
                next: "@pop"
              },
              "@default": "@brackets"
            }
          }
        ],
        [/[{}()\[\]]/, "@brackets"],
        [/@symbols/, "delimiter"],
        [/\d+[eE]([\-+]?\d+)?/, "number.float"],
        [/\d+\.\d+([eE][\-+]?\d+)?/, "number.float"],
        [/0[xX][0-9a-fA-F]+/, "number.hex"],
        [/0[0-7]+(?!\d)/, "number.octal"],
        [/\d+/, "number"],
        [/[,.]/, "delimiter"],
        [/"""/, "string", '@herestring."""'],
        [/'''/, "string", "@herestring.'''"],
        [
          /"/,
          {
            cases: {
              "@eos": "string",
              "@default": { token: "string", next: '@string."' }
            }
          }
        ],
        [
          /'/,
          {
            cases: {
              "@eos": "string",
              "@default": { token: "string", next: "@string.'" }
            }
          }
        ]
      ],
      string: [
        [/[^"'\#\\]+/, "string"],
        [/@escapes/, "string.escape"],
        [/\./, "string.escape.invalid"],
        [/\./, "string.escape.invalid"],
        [
          /#{/,
          {
            cases: {
              '$S2=="': {
                token: "string",
                next: "root.interpolatedstring"
              },
              "@default": "string"
            }
          }
        ],
        [
          /["']/,
          {
            cases: {
              "$#==$S2": { token: "string", next: "@pop" },
              "@default": "string"
            }
          }
        ],
        [/#/, "string"]
      ],
      herestring: [
        [
          /("""|''')/,
          {
            cases: {
              "$1==$S2": { token: "string", next: "@pop" },
              "@default": "string"
            }
          }
        ],
        [/[^#\\'"]+/, "string"],
        [/['"]+/, "string"],
        [/@escapes/, "string.escape"],
        [/\./, "string.escape.invalid"],
        [/#{/, { token: "string.quote", next: "root.interpolatedstring" }],
        [/#/, "string"]
      ],
      comment: [
        [/[^#]+/, "comment"],
        [/###/, "comment", "@pop"],
        [/#/, "comment"]
      ],
      hereregexp: [
        [/[^\\\/#]+/, "regexp"],
        [/\\./, "regexp"],
        [/#.*$/, "comment"],
        ["///[igm]*", { token: "regexp", next: "@pop" }],
        [/\//, "regexp"]
      ]
    }
  };
  return coffee_exports;
})();
return moduleExports;
});
