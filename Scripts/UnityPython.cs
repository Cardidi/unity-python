using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Exodrifter.UnityPython
{
	/// <summary>
	/// Convenience class for creating a Python engine integrated with Unity.
	///
	/// All scripts executed by the ScriptEngine created by this class will:
	/// * Redirect output to Unity's console
	/// * Be able to import any class in a `UnityEngine*` namespace
	/// * Be able to import any class in a `UnityEditor*` namespace, if the script
	///   is running in the UnityEditor
	/// </summary>
	public static class UnityPython
    {
    
    	public static ScriptEngine CreateEngine(UnityPythonCreationSetting setting)
    	{
    		var engine = Python.CreateEngine(setting.Options);
    
    		// Redirect IronPython IO
    		if (setting.RedirectStandardOutputStream)
    		{
    			var infoStream = new MemoryStream();
    			var infoWriter = new UniPyLogWriter(Debug.Log, infoStream);
    			engine.Runtime.IO.SetOutput(infoStream, infoWriter);
    		}
    
    		if (setting.RedirectStandardErrorStream)
    		{
    			var errorStream = new MemoryStream();
    			var errorWriter = new UniPyLogWriter(Debug.LogError, errorStream);
    			engine.Runtime.IO.SetErrorOutput(errorStream, errorWriter);
    		}
    
    		// Load assemblies for the `UnityEngine*` namespaces
    		if (setting.IncludeUnityEngine)
    		{
    			foreach (var assembly in GetAssembliesInNamespace("UnityEngine"))
    			{
    				engine.Runtime.LoadAssembly(assembly);
    			}
    		}
    
    		// Load assemblies for the `UnityEditor*` namespaces
    		if (setting.IncludeUnityEditorIfPossible)
    		{
    #if UNITY_EDITOR
    			foreach (var assembly in GetAssembliesInNamespace("UnityEditor"))
    			{
    				engine.Runtime.LoadAssembly(assembly);
    			}
    #endif
    		}
    		else
    		{
    #if !UNITY_EDITOR
    			Debug.Log("Can not load namespace of UnityEditor! Please set UnityPythonCreationSetting.IncludeUnityEditorIfPossible to false.");
    #endif
    		}
    		
    		// Load Specific given namespace
    		if (setting.CustomIncluding != null)
    		{
    			foreach (var ns in setting.CustomIncluding)
    			{
    				foreach (var assembly in GetAssembliesInNamespace(ns))
    				{
    					engine.Runtime.LoadAssembly(assembly);
    				}
    			}
    		}
    
    
    		return engine;
    	}
    
    
    	public static ScriptEngine CreateEngine() => CreateEngine(UnityPythonCreationSetting.CreateNew());
    
    	public static ScriptEngine CreateEngine(params string[] customInclude)
    		=> CreateEngine(UnityPythonCreationSetting.CreateNew(customIncluding: customInclude));
    	
    	public static ScriptEngine CreateEngine(IDictionary<string, object> options = null, params string[] customInclude)
    		=> CreateEngine(UnityPythonCreationSetting.CreateNew(options: options, customIncluding: customInclude));
    	
    
    	/// <summary>
    	/// Get a list of all loaded assemblies in the current AppDomain for a
    	/// namespace beginning with the specified string.
    	/// </summary>
    	/// <param name="prefix">The beginning of the namespace.</param>
    	/// <returns>All matching assemblies.</returns>
    	private static IEnumerable<Assembly> GetAssembliesInNamespace(string prefix)
    	{
    		return AppDomain.CurrentDomain.GetAssemblies()
    			.SelectMany(t => t.GetTypes())
    			.Where(t => t.Namespace != null && t.Namespace.StartsWith(prefix))
    			.Select(t => t.Assembly)
    			.Distinct();
    	}
    }

}
