using System.Collections.Generic;

namespace Exodrifter.UnityPython
{
    public struct UnityPythonCreationSetting
    {
        /// <summary>
        /// Should using UnityEngine.dll in python runtime?
        /// </summary>
        public readonly bool IncludeUnityEngine;

        /// <summary>
        /// Should using UnityEditor.dll in python runtime?<br />
        /// Only valid in editor.
        /// </summary>
        public readonly bool IncludeUnityEditorIfPossible;

        /// <summary>
        /// Specific namespace which will be using in python runtime.
        /// </summary>
        public readonly string[] CustomIncluding;

        /// <summary>
        /// Options in create python service.
        /// </summary>
        public readonly IDictionary<string, object> Options;
        
        /// <summary>
        /// Redirect python standard output stream to unity logger.
        /// </summary>
        public readonly bool RedirectStandardOutputStream;
        
        /// <summary>
        /// Redirect python standard error stream to unity logger.
        /// </summary>
        public readonly bool RedirectStandardErrorStream;

        public static UnityPythonCreationSetting CreateNew(
            bool includeUnityEngine = true,
            bool includeUnityEditorIfPossible = false,
            string[] customIncluding = null,
            IDictionary<string, object> options = null,
            bool redirectStandardErrorStream = true,
            bool redirectStandardOutputStream = true)
        {
            return new UnityPythonCreationSetting(
                includeUnityEngine,
                includeUnityEditorIfPossible,
                customIncluding,
                options,
                redirectStandardErrorStream,
                redirectStandardOutputStream);
        }

        private UnityPythonCreationSetting(
            bool includeUnityEngine,
            bool includeUnityEditorIfPossible,
            string[] customIncluding,
            IDictionary<string, object> options,
            bool redirectStandardErrorStream,
            bool redirectStandardOutputStream)
        {
            IncludeUnityEngine = includeUnityEngine;
            IncludeUnityEditorIfPossible = includeUnityEditorIfPossible;
            CustomIncluding = customIncluding;
            Options = options;
            RedirectStandardErrorStream = redirectStandardErrorStream;
            RedirectStandardOutputStream = redirectStandardOutputStream;
        }
    }
}