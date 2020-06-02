using UnityEngine;

public static class SGT_CachedFind<T>
	where T : Object
{
	private static float lastCheck;
	private static T[]   all;
	
	public static T[] All(float maximumElapsed)
	{
		var globalTime = SGT_Helper.GlobalTime;
		var elapsed    = Mathf.Abs(globalTime - lastCheck);
		
		if (elapsed >= maximumElapsed || lastCheck <= 0.0f || Application.isPlaying == false)
		{
			lastCheck = globalTime;
			all       = Object.FindSceneObjectsOfType(typeof(T)) as T[];
		}
		
		if (all == null) all = new T[0];
		
		return all;
	}
}