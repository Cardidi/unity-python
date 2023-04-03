using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using UnityEngine;

namespace Exodrifter.UnityPython
{
    public static class UnityPythonExtension
    {
        public static dynamic ExecuteString(this ScriptEngine engine, string code, ScriptScope scope)
        {
            var src = engine.CreateScriptSourceFromString(code, SourceCodeKind.File);
            src.Execute(scope);
            return scope;
        }
        
        public static dynamic ExecuteString(this ScriptEngine engine, string code)
        {
            var scope = engine.CreateScope();
            return ExecuteString(engine, code, scope);
        }

    }
}