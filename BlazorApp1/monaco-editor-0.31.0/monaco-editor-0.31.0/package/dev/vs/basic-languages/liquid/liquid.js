/*!-----------------------------------------------------------------------------
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Version: 0.31.0(252e010eb73ddc2fa1a37c1dade7bf35d87106cd)
 * Released under the MIT license
 * https://github.com/microsoft/monaco-editor/blob/main/LICENSE.txt
 *-----------------------------------------------------------------------------*/
define("vs/basic-languages/liquid/liquid",[],()=>{
var moduleExports = (() => {
  var __create = Object.create;
  var __defProp = Object.defineProperty;
  var __getOwnPropDesc = Object.getOwnPropertyDescriptor;
  var __getOwnPropNames = Object.getOwnPropertyNames;
  var __getProtoOf = Object.getPrototypeOf;
  var __hasOwnProp = Object.prototype.hasOwnProperty;
  var __markAsModule = (target) => __defProp(target, "__esModule", { value: true });
  var __commonJS = (cb, mod) => function __require() {
    return mod || (0, cb[Object.keys(cb)[0]])((mod = { exports: {} }).exports, mod), mod.exports;
  };
  var __export = (target, all) => {
    __markAsModule(target);
    for (var name in all)
      __defProp(target, name, { get: all[name], enumerable: true });
  };
  var __reExport = (target, module, desc) => {
    if (module && typeof module === "object" || typeof module === "function") {
      for (let key of __getOwnPropNames(module))
        if (!__hasOwnProp.call(target, key) && key !== "default")
          __defProp(target, key, { get: () => module[key], enumerable: !(desc = __getOwnPropDesc(module, key)) || desc.enumerable });
    }
    return target;
  };
  var __toModule = (module) => {
    return __reExport(__markAsModule(__defProp(module != null ? __create(__getProtoOf(module)) : {}, "default", module && module.__esModule && "default" in module ? { get: () => module.default, enumerable: true } : { value: module, enumerable: true })), module);
  };

  // build/fillers/monaco-editor-core-amd.ts
  var require_monaco_editor_core_amd = __commonJS({
    "build/fillers/monaco-editor-core-amd.ts"(exports, module) {
      module.exports = self.monaco;
    }
  });

  // src/basic-languages/liquid/liquid.ts
  var liquid_exports = {};
  __export(liquid_exports, {
    conf: () => conf,
    language: () => language
  });

  // src/fillers/monaco-editor-core.ts
  var monaco_editor_core_exports = {};
  __markAsModule(monaco_editor_core_exports);
  __reExport(monaco_editor_core_exports, __toModule(require_monaco_editor_core_amd()));

  // src/basic-languages/liquid/liquid.ts
  var EMPTY_ELEMENTS = [
    "area",
    "base",
    "br",
    "col",
    "embed",
    "hr",
    "img",
    "input",
    "keygen",
    "link",
    "menuitem",
    "meta",
    "param",
    "source",
    "track",
    "wbr"
  ];
  var conf = {
    wordPattern: /(-?\d*\.\d\w*)|([^\`\~\!\@\$\^\&\*\(\)\=\+\[\{\]\}\\\|\;\:\'\"\,\.\<\>\/\s]+)/g,
    brackets: [
      ["<!--", "-->"],
      ["<", ">"],
      ["{{", "}}"],
      ["{%", "%}"],
      ["{", "}"],
      ["(", ")"]
    ],
    autoClosingPairs: [
      { open: "{", close: "}" },
      { open: "%", close: "%" },
      { open: "[", close: "]" },
      { open: "(", close: ")" },
      { open: '"', close: '"' },
      { open: "'", close: "'" }
    ],
    surroundingPairs: [
      { open: "<", close: ">" },
      { open: '"', close: '"' },
      { open: "'", close: "'" }
    ],
    onEnterRules: [
      {
        beforeText: new RegExp(`<(?!(?:${EMPTY_ELEMENTS.join("|")}))(\\w[\\w\\d]*)([^/>]*(?!/)>)[^<]*$`, "i"),
        afterText: /^<\/(\w[\w\d]*)\s*>$/i,
        action: {
          indentAction: monaco_editor_core_exports.languages.IndentAction.IndentOutdent
        }
      },
      {
        beforeText: new RegExp(`<(?!(?:${EMPTY_ELEMENTS.join("|")}))(\\w[\\w\\d]*)([^/>]*(?!/)>)[^<]*$`, "i"),
        action: { indentAction: monaco_editor_core_exports.languages.IndentAction.Indent }
      }
    ]
  };
  var language = {
    defaultToken: "",
    tokenPostfix: "",
    builtinTags: [
      "if",
      "else",
      "elseif",
      "endif",
      "render",
      "assign",
      "capture",
      "endcapture",
      "case",
      "endcase",
      "comment",
      "endcomment",
      "cycle",
      "decrement",
      "for",
      "endfor",
      "include",
      "increment",
      "layout",
      "raw",
      "endraw",
      "render",
      "tablerow",
      "endtablerow",
      "unless",
      "endunless"
    ],
    builtinFilters: [
      "abs",
      "append",
      "at_least",
      "at_most",
      "capitalize",
      "ceil",
      "compact",
      "date",
      "default",
      "divided_by",
      "downcase",
      "escape",
      "escape_once",
      "first",
      "floor",
      "join",
      "json",
      "last",
      "lstrip",
      "map",
      "minus",
      "modulo",
      "newline_to_br",
      "plus",
      "prepend",
      "remove",
      "remove_first",
      "replace",
      "replace_first",
      "reverse",
      "round",
      "rstrip",
      "size",
      "slice",
      "sort",
      "sort_natural",
      "split",
      "strip",
      "strip_html",
      "strip_newlines",
      "times",
      "truncate",
      "truncatewords",
      "uniq",
      "upcase",
      "url_decode",
      "url_encode",
      "where"
    ],
    constants: ["true", "false"],
    operators: ["==", "!=", ">", "<", ">=", "<="],
    symbol: /[=><!]+/,
    identifier: /[a-zA-Z_][\w]*/,
    tokenizer: {
      root: [
        [/\{\%\s*comment\s*\%\}/, "comment.start.liquid", "@comment"],
        [/\{\{/, { token: "@rematch", switchTo: "@liquidState.root" }],
        [/\{\%/, { token: "@rematch", switchTo: "@liquidState.root" }],
        [/(<)([\w\-]+)(\/>)/, ["delimiter.html", "tag.html", "delimiter.html"]],
        [/(<)([:\w]+)/, ["delimiter.html", { token: "tag.html", next: "@otherTag" }]],
        [/(<\/)([\w\-]+)/, ["delimiter.html", { token: "tag.html", next: "@otherTag" }]],
        [/</, "delimiter.html"],
        [/\{/, "delimiter.html"],
        [/[^<{]+/]
      ],
      comment: [
        [/\{\%\s*endcomment\s*\%\}/, "comment.end.liquid", "@pop"],
        [/./, "comment.content.liquid"]
      ],
      otherTag: [
        [
          /\{\{/,
          {
            token: "@rematch",
            switchTo: "@liquidState.otherTag"
          }
        ],
        [
          /\{\%/,
          {
            token: "@rematch",
            switchTo: "@liquidState.otherTag"
          }
        ],
        [/\/?>/, "delimiter.html", "@pop"],
        [/"([^"]*)"/, "attribute.value"],
        [/'([^']*)'/, "attribute.value"],
        [/[\w\-]+/, "attribute.name"],
        [/=/, "delimiter"],
        [/[ \t\r\n]+/]
      ],
      liquidState: [
        [/\{\{/, "delimiter.output.liquid"],
        [/\}\}/, { token: "delimiter.output.liquid", switchTo: "@$S2.$S3" }],
        [/\{\%/, "delimiter.tag.liquid"],
        [/raw\s*\%\}/, "delimiter.tag.liquid", "@liquidRaw"],
        [/\%\}/, { token: "delimiter.tag.liquid", switchTo: "@$S2.$S3" }],
        { include: "liquidRoot" }
      ],
      liquidRaw: [
        [/^(?!\{\%\s*endraw\s*\%\}).+/],
        [/\{\%/, "delimiter.tag.liquid"],
        [/@identifier/],
        [/\%\}/, { token: "delimiter.tag.liquid", next: "@root" }]
      ],
      liquidRoot: [
        [/\d+(\.\d+)?/, "number.liquid"],
        [/"[^"]*"/, "string.liquid"],
        [/'[^']*'/, "string.liquid"],
        [/\s+/],
        [
          /@symbol/,
          {
            cases: {
              "@operators": "operator.liquid",
              "@default": ""
            }
          }
        ],
        [/\./],
        [
          /@identifier/,
          {
            cases: {
              "@constants": "keyword.liquid",
              "@builtinFilters": "predefined.liquid",
              "@builtinTags": "predefined.liquid",
              "@default": "variable.liquid"
            }
          }
        ],
        [/[^}|%]/, "variable.liquid"]
      ]
    }
  };
  return liquid_exports;
})();
return moduleExports;
});
