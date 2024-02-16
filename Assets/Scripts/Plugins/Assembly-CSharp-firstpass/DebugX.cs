using System.Diagnostics;
using UnityEngine;

public class DebugX
{
	private static readonly string _strDefaultAssertString = "Assertion failed!";

	[Conditional("ADMIN_DEBUG")]
	public static void Log(object message)
	{
		UnityEngine.Debug.Log(message);
	}

	public static void LogError(object message)
	{
		UnityEngine.Debug.LogError(message);
	}

	[Conditional("ADMIN_DEBUG")]
	public static void Assert(bool condition, object message = null)
	{
		if (!condition)
		{
			UnityEngine.Debug.LogWarning((message != null) ? message : _strDefaultAssertString);
		}
	}
}
