using UnityEngine;
using System.Collections;
using System.Diagnostics;

public static class Log {

	public static void i ( object message ) {
		UnityEngine.Debug.Log ( "[" + new StackFrame ( 1, true ).GetMethod ( ).DeclaringType.Name + "] " + message );
	}


	public static void w ( object message ) {
		UnityEngine.Debug.LogWarning ( "[" + new StackFrame ( 1, true ).GetMethod ( ).DeclaringType.Name + "] " + message );
	}


	public static void e ( object message ) {
		UnityEngine.Debug.LogError ( "[" + new StackFrame ( 1, true ).GetMethod ( ).DeclaringType.Name + "] " + message );
	}

}
