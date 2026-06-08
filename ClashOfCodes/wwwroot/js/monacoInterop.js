// This file bridges Blazor (C#) and Monaco Editor (JavaScript)
// Blazor cannot directly control Monaco — it uses JS interop to call these functions

window.monacoInterop = {
  // Called from Blazor to create the editor inside a div
  initEditor: function (elementId, language, initialCode) {
    require.config({
      paths: {
        vs: "https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.45.0/min/vs",
      },
    });

    require(["vs/editor/editor.main"], function () {
      // Store editor instance globally so we can access it later
      window._monacoEditor = monaco.editor.create(
        document.getElementById(elementId),
        {
          value: initialCode,
          language: language,
          theme: "vs-dark",
          fontSize: 14,
          minimap: { enabled: false },
          automaticLayout: true, // resizes with the container
        },
      );
    });
  },

  // Called from Blazor to read the current code in the editor
  getCode: function () {
    if (window._monacoEditor) {
      return window._monacoEditor.getValue();
    }
    return "";
  },

  // Called from Blazor to set the editor language (e.g. when user switches)
  setLanguage: function (language) {
    if (window._monacoEditor) {
      monaco.editor.setModelLanguage(window._monacoEditor.getModel(), language);
    }
  },
};
